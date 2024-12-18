using DocumentFormat.OpenXml.Office2010.Word;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.LuceneIndexer
{
    public class LuceneDocumentIndexer :ISearchEngine
    {
        private readonly ILogger<LuceneDocumentIndexer> logger;
        private readonly IConfiguration config;
        private readonly IServiceProvider serviceProvider;
        private readonly IVirtualFileSystemProvider fileSystemProvider;
        private readonly IDocumentService documentService;
        private readonly string percorso = "";

        public LuceneDocumentIndexer(
            ILogger<LuceneDocumentIndexer> logger,
            IConfiguration config,
            IServiceProvider serviceProvider,
            IVirtualFileSystemProvider fileSystemProvider,
            IDocumentService documentService)
        {
            this.logger = logger;
            this.config = config;
            this.serviceProvider = serviceProvider;
            this.fileSystemProvider = fileSystemProvider;
            this.documentService = documentService;
            var root = config["Documents:RootFolder"].ToString();
            if (string.IsNullOrEmpty(root)) { root = "/"; };
            percorso = Path.Combine(root, "Lucene");
        }

        public async Task Add(int imageId)
        {
            var u = UserProfile.SystemUser();
            //var d = await documentService.Load(documentId, u);
            //if (d.Id <= 0)
            //{
            //    logger.LogWarning("Documento con id:{0} non trovato. Indicizzazione non effettuata");
            //    return;
            //}
            //if (d.Image == null || d.Image.Id <= 0)
            //{
            //    logger.LogWarning("Documento con id:{0} senza contenuto. Indicizzazione non effettuata");
            //    return;
            //}
            bool res = false;
            try
            {
                var image = await documentService.GetContentInfo(imageId);
                await documentService.UpdateIndexingStatus(imageId, Domain.Enumerators.JobStatus.Running, u);
                String Filename = image.FileName.ToLower(); 
                string Ext = Path.GetExtension(Filename);

                var Content = "";
                var f = await fileSystemProvider.InstanceOf(image.FileManager);
                if (await f.Exists(image.FileName))
                {
                    using (var data = await f.ReadAsStream(image.FileName))
                    {
                        foreach (var Extractor in serviceProvider.GetServices<IContentTextExtractor>())
                        {
                            if (Extractor.FileTypeSupported(Ext))
                            {
                                data.Position = 0;
                                Content += await Extractor.GetText(data) + " ";
                            }
                        }
                    }
                }
                if (!String.IsNullOrEmpty(Content))
                {
                    using (IndexWriter Writer = new IndexWriter(GetDirectory(), GetAnalizer(), IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        var docs = await documentService.GetDocumentsFromContentId(imageId, u);
                        foreach (var d in docs)
                        {
                            var DocId = d.ToString();
                            Query idquery = new TermQuery(new Term("ID", DocId));

                            Document doc = new Document();

                            Searcher Searcher = new IndexSearcher(GetDirectory(), true);
                            TopScoreDocCollector collector = TopScoreDocCollector.Create(1, true);
                            Searcher.Search(idquery, collector);
                            ScoreDoc[] hits = collector.TopDocs().ScoreDocs;

                            if (hits.Length > 0)
                            {
                                doc = Searcher.Doc(hits[0].Doc);
                                doc.RemoveField("Content");
                                doc.Add(new Field("Content", Content.ToLower(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
                                Writer.UpdateDocument(new Term("ID", DocId), doc);
                            }
                            else
                            {
                                doc.Add(new Field("ID", DocId, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                                doc.Add(new Field("Content", Content.ToLower(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
                                Writer.AddDocument(doc);
                            }
                        }
                        Writer.Optimize();
                        try
                        {
                            Writer.Close();
                        } finally
                        {
                            if (IndexWriter.IsLocked(GetDirectory()))
                                IndexWriter.Unlock(GetDirectory());
                        }
                    }
                }
                await documentService.UpdateIndexingStatus(imageId, Domain.Enumerators.JobStatus.Completed, u); ;

            }
            catch (Exception ex)
            {
                await documentService.UpdateIndexingStatus(imageId, Domain.Enumerators.JobStatus.Failed, u);
                logger.LogError(ex, "LuceneSearchEngine -> " + ex.Message);
            }
            return;
        }


        public async Task Remove(int documentId)
        {
            using (IndexWriter Writer = new IndexWriter(GetDirectory(), GetAnalizer(), IndexWriter.MaxFieldLength.UNLIMITED))
            {
                Query idquery = new TermQuery(new Term("ID", documentId.ToString()));
                //BooleanQuery parser = new BooleanQuery();
                //parser.Clauses.Add(new BooleanClause(query1, Occur.MUST));
                //parser.Clauses.Add(new BooleanClause(query2, Occur.MUST));
                Writer.DeleteDocuments(idquery);
                try
                {
                    Writer.Close();
                }
                finally
                {
                    if (IndexWriter.IsLocked(GetDirectory()))
                        IndexWriter.Unlock(GetDirectory());
                }
            }
        }
        private Lucene.Net.Store.Directory GetDirectory()
        {
            try
            {
                if (!System.IO.Directory.Exists(percorso))
                {
                    System.IO.Directory.CreateDirectory(percorso);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "LuceneSearchEngine: GetDirectory -> " + e.Message);
            }
            return FSDirectory.Open(new System.IO.DirectoryInfo(percorso));
        }

        private Analyzer GetAnalizer()
        {
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }

        public async Task<String> GetContent(int documentId, UserProfile user)
        {
            String res = "";
            if ((await documentService.GetPermission(documentId, user, PermissionType.CanViewContent)).Authorization != Domain.Enumerators.AuthorizationType.Granted)
                return res;

            using (Searcher Searcher = new IndexSearcher(GetDirectory(), true))
            {
                Query idquery = new TermQuery(new Term("ID", documentId.ToString()));
                TopScoreDocCollector collector = TopScoreDocCollector.Create(1, true);
                Searcher.Search(idquery, collector);
                ScoreDoc[] hits = collector.TopDocs().ScoreDocs;
                if (hits.Length > 0)
                {
                    Document d = Searcher.Doc(hits[0].Doc);
                    res = d.Get("Content");
                }
            }
            return res;
        }

        public async Task<Dictionary<int, float>> Search(string SearchString, int PageSize, UserProfile user)
        {
            Dictionary<int, float> res = new Dictionary<int, float>();
            if (String.IsNullOrEmpty(SearchString))
            {
                return res;
            }

            TopScoreDocCollector collector = TopScoreDocCollector.Create(PageSize, true);
            BooleanQuery boolQuery = new BooleanQuery();
            String[] Ricerca = SearchString.Split(' ');
            for (int i = 0; i < Ricerca.Length; i++)
            {
                if (Ricerca[i].Trim() != "")
                {
                    FuzzyQuery queryContain = new FuzzyQuery(new Term("Content", Ricerca[i].Trim().ToLower()));
                    boolQuery.Add(queryContain, Occur.MUST);
                }
            }

            using (Searcher Searcher = new IndexSearcher(GetDirectory(), true))
            {
                Searcher.Search(boolQuery, collector);
                var TopDocs = collector.TopDocs();
                var max = TopDocs.MaxScore;
                if (max <= 0) max = 1;
                ScoreDoc[] hits = TopDocs.ScoreDocs;
                foreach (var risultato in hits)
                {
                    Document d = Searcher.Doc(risultato.Doc);
                    var k = int.Parse(d.Get("ID"));
                    if (!res.ContainsKey(k))
                    {
                        var s = risultato.Score / max;
                        var i = (float)Math.Round(s * 7);
                        res.Add(k, i);
                    }
                }
            }
            return res;
        }




        public async Task RebuildIndex()
        {
            try
            {
                if (System.IO.Directory.Exists(percorso))
                {
                    System.IO.Directory.Delete(percorso, true);
                    await documentService.ClearIndexing();
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "LuceneSearchEngine: GetDirectory -> " + e.Message);
            }
        }

    }
}