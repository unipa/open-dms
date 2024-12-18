using System.Text.Json.Serialization;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Tasks;

/// <summary>
/// Elenco originale dei destinatari di un task.
/// Potrebbe differire dagli userTask quando una attività assegnata in CC ad un gruppo
/// genera userTasks per ogni membro del gruppo
/// Inoltre uno userTask in carico ad un gruppo viene modificato quando reclamato da un utente
/// </summary>
public class TaskRecipient
{
    public int Id { get; set; }

    public int TaskItemId { get; set; }

    [StringLength(64)]
    public string ProfileId { get; set; } = "";
    public ProfileType ProfileType { get; set; } = 0;
    public bool CC { get; set; } = false;

    [JsonIgnore]
    public virtual TaskItem TaskItem { get; set; }
}
