using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Infrastructure.Database.Repositories
{
    public class TranslationRepository : ITranslationRepository
    {
        private readonly IApplicationDbContextFactory contextFactory;
        private readonly ApplicationDbContext DS;

        public TranslationRepository(IApplicationDbContextFactory contextFactory)
        {
            this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
            this.contextFactory = contextFactory;
        }


        public Task Clone(string FromLanguage, string ToLanguage)
        {
            throw new NotImplementedException();
        }

        public Task<string> Export(string culture)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Get(string category, string Name, string culture = "it-IT")
        {
            var l = await DS.TranslatedTexts.AsNoTracking().FirstOrDefaultAsync (l=>l.LanguageId==culture && l.CategoryId==category && l.Text==Name);
            if (l == null) return Name; else return l.Value;
        }

        public Task Import(string dictionaryData, string Language)
        {
            throw new NotImplementedException();
        }

        public Task<List<CultureInfo>> Languages()
        {
            throw new NotImplementedException();
        }
    }
}
