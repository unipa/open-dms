using System.Text.Json.Serialization;
using OpenDMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Models;
using iText.Layout.Font;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Core.Interfaces;

namespace OpenDMS.Core.DTOs
{
    public enum FormType 
    {
        none, 
        formjs,
        formhtml,
        formio
    }

    public class FormSchema
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        //public string Html { get; set; }
        public string Data  { get; set; }
        public string Version { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UpdateUser { get; set; }
        public string FormType { get; set; } = "HTML";

        //public List<KeyValuePair<string,string>> Data{ get; set; } = new List<KeyValuePair<string, string>>();


        public FormSchema()
        {
            this.Id = 0;
            this.Name = "";
            this.Key = "";
            this.Schema = "";
            //this.Html = "";
            this.Data = "";
            this.Version = "";
            this.UpdateUser = "";
            this.LastUpdate = DateTime.UtcNow;
        }
        internal class SchemaFieldConstraint
        {
            public bool required { get; set; }
        }

  

        public void UpdateVariables(string variables)
        {
            if (!String.IsNullOrEmpty(variables))
                this.Data = variables;
        }

        //public static FormSchema Default(bool Mandatory) 
        //{
        //    FormSchema F = new FormSchema();
        //    StringBuilder schema = new StringBuilder();
        //    schema.Append("{ \"components\": [");
        //    schema.Append("{ \"label\": \" Risposta"+(Mandatory ? " (Obbligatoria)" : "") +" \", \"type\": \"textarea\", \"fieldtype\": \"$$p\", \"id\": \"Justification\", \"key\": \"Justification\", \"validate\": { \"required\": " + Mandatory.ToString().ToLower()+" } }");
        //    schema.Append("], \"type\": \"default\", \"id\": \"default\", \"schemaVersion\": 8 }");
        //    F.Schema = schema.ToString();
        //    F.Data = "{ \"Justification\" : \"\" }";
        //    F.FormType = FormType.formjs;
        //    F.Version = "1.00";
        //    F.LastUpdate = DateTime.UtcNow;
        //    return F;
        //}
        public static string Default()
        {
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "events", "Default.html")))
                return "";
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "events", "Default.html"));
        }

        //public static FormSchema AskForApproval (string Title, bool RejectNoteMandatory)
        //{
        //    string scelte = "";
        //    string defaultValue = "1";
        //    Title = "Scegli un'opzione:";
        //    scelte = scelte + "{ \"label\" : \"Si\", \"value\" : \"1\" }";
        //    scelte = scelte + ",{ \"label\" : \"No\", \"value\" : \"0\" }";
        //    StringBuilder schema = new StringBuilder();
        //    schema.AppendLine("{ \"components\": [");
        //    schema.AppendLine($@"{{
        //                  ""values"": 
        //                  [
        //                        {scelte}
        //                  ],
        //                  ""label"": ""{Title}"",
        //                  ""type"": ""radio"",
        //                  ""id"": ""Choise"",
        //                  ""key"": ""Choise"",
        //                  ""validate"": {{
        //                    ""required"": true
        //                  }},
        //                  ""defaultValue"": ""{defaultValue}""
        //                }}");

        //    schema.AppendLine(",{ \"label\": \"Annotazioni" + (RejectNoteMandatory ? " (Informazione Obbligatoria)" : "") + "\", \"type\": \"textarea\", \"fieldtype\": \"$$p\", \"id\": \"Justification\", \"key\": \"Justification\", \"validate\": { \"required\": " + RejectNoteMandatory.ToString().ToLower() + " } }");
        //    schema.AppendLine("], \"type\": \"default\", \"id\": \"default\", \"schemaVersion\": 8 }");

        //    FormSchema F = new FormSchema();
        //    F.Schema = schema.ToString();
        //    F.Data = "{ \"Justification\" : \"\",\"Choise\": \""+defaultValue+"\" }";
        //    F.FormType = FormType.formjs;
        //    F.Version = "1.00";
        //    F.LastUpdate = DateTime.UtcNow;
        //    //F.Data.Add(new("description",""));
        //    return F;
        //}

        public static string GetTemplate (string Template)
        {
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "events", Template + ".html")))
                Template = "Event";
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "events", Template + ".html")))
                Template = "Default";
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "events", Template + ".html")))
                return "";
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "events", Template + ".html"));
        }


        //public static FormSchema AskForEvent(string Title, bool RejectNoteMandatory)
        //{
        //    FormSchema F = new FormSchema();
        //    StringBuilder schema = new StringBuilder();
        //    Title = "Motivazione";

        //    schema.Append("{ \"components\": [");
        //    string Obbligatorio = RejectNoteMandatory ? " (Informazione Obbligatoria)" : "";
        //    schema.Append("{ \"label\": \"** Se ritieni di non dovere o potere effettuare l'azione richiesta indica una motiviazione e archivia questa attività **\", \"type\": \"text\", \"id\": \"title\", \"key\": \"title\" }");
        //    schema.Append($",{{ \"label\": \"{Title} {Obbligatorio}\", \"type\": \"textarea\", \"fieldtype\": \"$$p\", \"id\": \"Justification\", \"key\": \"Justification\", \"validate\": {{ \"required\": {RejectNoteMandatory.ToString().ToLower() } }} }}");

        //    schema.Append("], \"type\": \"default\", \"id\": \"default\", \"schemaVersion\": 8 }");
        //    F.Schema = schema.ToString();
        //    F.Data = "{ \"Justification\" : \"\" }";
        //    F.FormType = FormType.formjs;
        //    F.Version = "1.00";
        //    F.LastUpdate = DateTime.UtcNow;
        //    //F.Data.Add(new("description",""));
        //    return F;
        //}

    }
}
