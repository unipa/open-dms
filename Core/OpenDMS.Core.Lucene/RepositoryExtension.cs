using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.LuceneIndexer.Extractors;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.LuceneIndexer
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddLucene(this IServiceCollection Services)
        {
            Services.AddTransient<IContentTextExtractor, EMLTextExtractor>();
            Services.AddTransient<IContentTextExtractor, ImageTextExtractor>();
            Services.AddTransient<IContentTextExtractor, OfficeTextExtractor>();
            Services.AddTransient<IContentTextExtractor, PDFTextExtractor>();
            Services.AddTransient<IContentTextExtractor, PlainTextExtractor>();
            Services.AddTransient<ISearchEngine, LuceneDocumentIndexer>();
            return Services;
        }
    }
}
