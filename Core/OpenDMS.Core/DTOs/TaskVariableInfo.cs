using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    /// <summary>
    /// Variabili da gestire per il form di default (FormId vuoto)
    /// </summary>
    public class TaskVariableInfo
    {
        /// <summary>
        /// Id Variabile
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Etichetta
        /// </summary>
        public string LookupValue { get; set; } = string.Empty;

        /// <summary>
        /// Tipo di metadato (es. $db)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// True = Obbligatorio
        /// </summary>
        public bool Mandatory { get; set; } = false;

    }
}
