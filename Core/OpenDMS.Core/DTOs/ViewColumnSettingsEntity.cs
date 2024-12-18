
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Domain.Entities
{

    [Table("ViewSettings")]
    [PrimaryKey(nameof(ViewId), nameof(Id), nameof(UserId))]
    public class ViewColumnSettingsEntity: ViewColumnSettings
    {
        // Elementi personalizzabili
        public string ViewId { get; set; } = "";
        public string Id { get; set; } = "";
        public string UserId { get; set; } = "";
        public int ColumnIndex { get; set; } = 0;

        public ViewColumnSettingsEntity(ViewColumnSettings s)
        {
            Width = s.Width;
            Title = s.Title;
            SortType = s.SortType;
            Visible = s.Visible;
        }

        public ViewColumnSettingsEntity()
        {

        }
    }
}