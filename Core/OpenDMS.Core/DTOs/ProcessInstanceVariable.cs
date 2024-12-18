using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Core.DTOs
{
    public class ProcessInstanceVariable
    {
        public string ProcessId { get; set; }

        // ID Istanza del processo
        public string ProcessInstanceId { get; set; }

        // VariableId fa riferimento ad un metadato globale
        public string? VariableId { get; set; }

        public string VariableName { get; set; }

        public string Value { get; set; }

        public string LookupValue { get; set; }

    }
}
