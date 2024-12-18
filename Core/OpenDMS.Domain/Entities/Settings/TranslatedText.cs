using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Settings;

[PrimaryKey(nameof(LanguageId), nameof(CategoryId), nameof(Text))]
public class TranslatedText
{
    [StringLength(6)]
    public string LanguageId { get; set; }

    [StringLength(64)]
    public string CategoryId { get; set; }

    public string Text { get; set; }
    public string Value { get; set; }

}

