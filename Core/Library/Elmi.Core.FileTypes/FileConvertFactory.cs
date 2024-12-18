using Elmi.Core.PreviewGenerators;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elmi.Core.FileConverters
{
    public class FileConvertFactory : IFileConvertFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<string, Dictionary<string, IFileConverter>> converters = new Dictionary<string, Dictionary<string, IFileConverter>>();
        private readonly Dictionary<string, HashSet<string>> invertedConverters = new Dictionary<string, HashSet<string>>();

        public FileConvertFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            foreach (IFileConverter converter in serviceProvider.GetServices(typeof(IFileConverter)))
            {
                string[] fromList = converter.FromFile;
                if (fromList != null)
                    foreach (var upperFrom in fromList)
                    { 
                        var from = upperFrom.ToLower();
                        string to = converter.ToFile.ToLower();
                        if (!converters.ContainsKey(from))
                            converters.Add(from, new Dictionary<string, IFileConverter>());
                        // Se ci sono più convertitori per la stessa estensione, prendo l'ultimo gestito
                        if (converters[from].ContainsKey(to))
                            converters[from].Remove(to);
                        converters[from].Add(to, converter);


                        if (!invertedConverters.ContainsKey(to))
                            invertedConverters.Add(to, new HashSet<string>());
                        // Se ci sono più convertitori per la stessa estensione, prendo l'ultimo gestito
                        if (invertedConverters[to].Contains(from))
                            invertedConverters[to].Remove(from);
                        invertedConverters[to].Add(from);

                    }
            }
        }

        public string GetFileSignature(Stream dataStream)
        {
            dataStream.Position = 0;
            byte[] data= new byte[6];
            int n = dataStream.Read(data, 0, 6);
            string mn = System.Text.Encoding.UTF8.GetString(data).ToLower();
            if (mn.StartsWith("<?xml")) return "xml";
            if (mn.StartsWith("%pdf")) return "pdf";
            if (mn.StartsWith("%png")) return "png";
            if (mn.StartsWith("<html")) return "html";
            if (mn.StartsWith("pk")) return "zip";
            if (mn.StartsWith("gif")) return "gif";

            if (mn.StartsWith("rar!")) return "rar";
            if (mn.StartsWith("bm")) return "bmp";

            if (mn.StartsWith("7z")) return "7z";
            if (mn.StartsWith("ustar")) return "tar";
            return "";
        }

        public async Task<IFileConverter> Get(string FromExtension, string ToExtension)
        {
            // mi assicuro che non ci siano riferimenti a file firmati
            var From = FromExtension.Replace(".","").ToLower();
            var To = ToExtension.Replace(".", "").ToLower();
            if (From == To)
                return converters["*"]["*"];

            IFileConverter found = null;
            if (converters.ContainsKey(From))
                if (converters[From].ContainsKey(To))
                    found = converters[From][To];
                else
                {
                    // Cerco un convertitore intermedio
                    // Partendo dal formato di destinazione verifico se c'è un convertitore 
                    // in grado di produrre un nuovo formato di input differente
                    foreach (var intermediateTo in invertedConverters[To])
                    {
                        if (converters[From].ContainsKey(intermediateTo))
                        {
                            found = new CompositeFileConverter (
                                converters[From][intermediateTo],
                                intermediateTo,
                                converters[intermediateTo][To]);
                            break;
                        }
                    }

                }

            if (found == null)
            {
                From = "txt";
                if (converters.ContainsKey(From))
                    if (converters[From].ContainsKey(To))
                        found = converters[From][To];
            }
            return found;
        }
    }
}
