
using OpenDMS.Core.DTOs;
using OpenDMS.Core.ViewModel.ColumnTypes;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.ViewModel.CustomColumnTypes.Documents
{
    public class ContentTypeColumn : GenericImageColumn
    {
        private static List<string> FileTypeTooltips => new()
        {
            "Documento Generico", // 0
            "Word",
            "Excel",
            "Powerpoint",
            "File Video",
            "File Audio",   // 5
            "File di testo",
            "Pdf",
            "Processo",
            "Matrice Dimensionale",
            "Form", // 10
            "File compresso",
            "Messaggio di Posta",
            "Documento Mancante",
            "Fascicolo",
            "Immagine",  // 15
            "Template"
        };

        private static List<string> FileTypeIcons = new()
        {
            "<i class='fa fa-file' style='color:#aab'></i>",
            "<i class='fa fa-file-word-o' style='color:skyblue'></i>",
            "<i class='fa fa-file-excel-o' style='color:limegreen'></i>",
            "<i class='fa fa-file-powerpoint-o' style='color:coral'></i>",
            "<i class='fa fa-film' style='color:firebrick'></i>",
            "<i class='fa fa-file-audio-o' style='color:crimson'></i>",
            "<i class='fa fa-file-text-o' style='color:#aab'></i>",
            "<i class='fa fa-file-pdf-o' style='color:firebrick'></i>",
            "<i class='fa fa-cogs' style='color:#4bf'></i>",
            "<i class='fa fa-question-circle' style='color:#4bf'></i>",
            "<i class='fa fa-table' style='color:#777'></i>",
            "<i class='fa fa-file-archive-o' style='color:#aab'></i>",
            "<i class='fa fa-envelope' style='color:#4bf'></i>",
            "<i class='fa fa-warning' style='color:orange'></i>",
            "<i class='fa fa-folder' style='color:orange'></i>",
            "<i class='fa fa-image' style='color:brown'></i>",
            "<i class='fa fa-edit' style='color:gray'></i>"
        };


        public ContentTypeColumn() : base(
            DocumentColumn.ContentType,
            "",
            "Tipo Contenuto",
            "Documento",
            new() { DocumentColumn.ContentType, DocumentColumn.Icon, DocumentColumn.IconColor, DocumentColumn.FileName },
            (fields) =>
            {
                var cdc = fields[0];
                var icon = fields[1];
                var color = fields[2];
                var ext = Path.GetExtension(fields[3]);
                var index = 0;
                if (string.IsNullOrEmpty(icon))
                {
                    switch (cdc)
                    {
                        case "2": // Folder
                            index = 14;
                            break;
                        case "3": // Processo
                            index = 8;
                            break;
                        case "5": // DMN
                            index = 9;
                            break;
                        case "4": // Form
                            index = 10;
                            break;
                        case "6":
                            index = 16;
                            break;
                        default:
                            switch (ext)
                            {
                                case ".doc":
                                case ".odt":
                                case ".docx":
                                    index = 1;
                                    break;
                                case ".xls":
                                case ".xlsx":
                                    index = 2;
                                    break;
                                case ".ppt":
                                case ".pptx":
                                    index = 3;
                                    break;
                                case ".mp4":
                                case ".ogg":
                                    index = 4;
                                    break;
                                case ".wav":
                                    index = 5;
                                    break;
                                case ".log":
                                case ".xml":
                                case ".txt":
                                case ".htm":
                                case ".html":
                                    index = 6;
                                    break;
                                case ".pdf":
                                    index = 7;
                                    break;
                                case ".bpmn":
                                    index = 8;
                                    break;
                                case ".dmn":
                                    index = 9;
                                    break;
                                case ".formio":
                                case ".formjs":
                                case ".formhtml":
                                    index = 10;
                                    break;
                                case ".zip":
                                    index = 11;
                                    break;
                                case ".msg":
                                case ".eml":
                                    index = 12;
                                    break;
                                case ".bmp":
                                case ".jpg":
                                case ".jpeg":
                                case ".png":
                                case ".tiff":
                                    index = 15;
                                    break;
                                case "":
                                    index = 13;
                                    break;

                                default:
                                    break;
                            }
                            break;
                    }
                    icon = FileTypeIcons[index];
                }
                else
                {
                    if (string.IsNullOrEmpty(color))
                    {
                        color = "var(--primary-fg-02)";
                    }
                    icon = "<i class='" + icon + "' style='color:" + color + "'></i>";
                }
                return new SearchResultColumn() { Value = cdc, Description = icon, Tooltip = FileTypeTooltips[index] };
            }, -1
        )
        {
        }

    }

}
