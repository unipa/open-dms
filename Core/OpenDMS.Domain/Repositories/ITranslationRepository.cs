using System.Globalization;

namespace OpenDMS.Domain.Repositories;
public interface ITranslationRepository
{
    Task<string> Get(string category, string Name, string culture = "it-IT");
    Task Clone(string FromLanguage, string ToLanguage);
    Task<List<CultureInfo>> Languages();
    Task<string> Export(string culture);
    Task Import(string dictionaryData, string Language);
}

