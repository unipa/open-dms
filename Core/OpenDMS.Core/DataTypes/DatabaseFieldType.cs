using Elmi.Core.DataAccess;
using OpenDMS.Domain;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class DatabaseFieldType : IDataTypeManager
    {
        public string DataTypeValue => "$db";
        public string DataTypeName => "Tabella Database Esterno";
        public string Icon => "icoDatabaseDataType";
        public string ControlType => "lookup";
        public bool IsInternal => false;
        public bool IsSearchable => false;
        public bool IsCalculated => false;
        public bool IsBlob => false;
        public bool IsPerson => false;
        public string AdminWebComponent => "AdminDatabaseInputField";
        public string WebComponent => "DatabaseInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup", Customized=false, DataType = DataTypeValue, Description = "Tabella Database", Id = DataTypeValue, Name = "Tabella Database", Title = "Tabella Database" }
        };

        public IDataSourceProvider DatasourceProvider { get; }

        public DatabaseFieldType(IDataSourceProvider datasourceProvider)
        {
            DatasourceProvider = datasourceProvider;
        }

        public string Deserialize(string Value, bool Cifrato = false)
        {
            return Value;
        }

        public async Task<FieldTypeValue> Calculate(FieldType M, string Value, Document document)
        {
            throw new NotImplementedException();
        }


        public async Task<FieldTypeValue> Lookup(FieldType M, string Value)
        {
            DatabaseProperties props = Newtonsoft.Json.JsonConvert.DeserializeObject< DatabaseProperties>(M.CustomProperties);
            ExternalDatasource conn = await DatasourceProvider.Get(props.ConnectionId);
            string C = props.Codice;
            string D = props.Decodifica;
            string E = props.AltriCampi;
            string T = props.Tabella;
            if (!string.IsNullOrEmpty(props.Sql))
                T = "("+ props.Sql + ") A";

            T += $" WHERE ({C}={Value.Quoted()})";
            if (!String.IsNullOrEmpty(props.Where))
                T += " AND " + props.Where;

            string SQL = String.IsNullOrEmpty(E) ? $"SELECT {D} FROM {T}" : $"SELECT {D}, {E} FROM {T}";
            FieldTypeValue FV = new FieldTypeValue();
            using (DataSource DS = new DataSource(conn.ConnectionString, conn.Driver))
            {
                if (!String.IsNullOrEmpty(E))
                {
                    var Items = DS.Select<FieldTypeValue>(SQL, 1, (reader, row) =>
                    {
                        FV.LookupValue = reader[0].ToString();
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            FV.Fields.Add(new LookupTable() { Id = reader.GetName(i), Description = reader[i].ToString(), TableId = reader[i].GetType().ToString() });
                        }
                        return FV;
                    }).ToList(); 
                }
                else
                    FV.LookupValue = DS.ReadString(SQL);

            }
            FV.FormattedValue = Value;
            FV.Value = Value;
            FV.IsValid = true;
            FV.FieldTypeId = M;
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> Items = new List<FieldTypeValue>();

            DatabaseProperties props = Newtonsoft.Json.JsonConvert.DeserializeObject<DatabaseProperties>(M.CustomProperties);
            ExternalDatasource conn = await DatasourceProvider.Get(props.ConnectionId);
            string C = props.Codice;
            string D = props.Decodifica;
            string T = props.Tabella;
            if (!string.IsNullOrEmpty(props.Sql))
                T = "(" + props.Sql + ") A";
            T += $" WHERE ({D} LIKE {(SearchValue + "%").Quoted()})";
            if (!String.IsNullOrEmpty(props.Where))
                T += " AND " + props.Where;

            string SQL = $"SELECT {C} A1, {D} A2 FROM {T} ORDER BY {D},{C}";
            if (MaxResults == 0) MaxResults = 8;
            using (DataSource DS = new DataSource(conn.ConnectionString, conn.Driver))
            {
                Items = DS.Select<FieldTypeValue>(SQL, MaxResults, (reader, row) =>
                {
                    FieldTypeValue FV = new FieldTypeValue();
                    FV.LookupValue = reader["A2"].ToString();
                    FV.Value = reader["A1"].ToString();
                    FV.FormattedValue = FV.Value;
                    FV.IsValid = true;
                    FV.FieldTypeId = M;
                    return FV;
                }).ToList();
            }
            return Items;
        }

        public string Serialize(string Value, bool Cifrato = false)
        {
            return Value;
        }

        public string Validate(FieldType M, string Value)
        {
            return "";
        }

        class DatabaseProperties
        {
            public string ConnectionId { get; set; }
            public string Tabella { get; set; }
            public string Sql { get; set; }
            public string Codice { get; set; }
            public string Decodifica { get; set; }
            public string AltriCampi { get; set; }
            public string Where { get; set; }
        }

    }
}
