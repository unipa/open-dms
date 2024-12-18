namespace Elmi.Core.DataAccess.Interfaces
{
    public interface ISchemaBuilder
    {
        bool Initialized { get; }
        bool TableExist { get; }

        ISchemaBuilder AddBoolean(string FieldName, string Description, bool DefaultValue = false);
        ISchemaBuilder AddDate(string FieldName, string Description, bool Nullable = true);
        ISchemaBuilder AddDecimal(string FieldName, string Description, int Length, int Mantissa, bool Nullable = false, decimal DefaultValue = 0);
        ISchemaBuilder AddIdentity(string FieldName, string Description);
        ISchemaBuilder AddIndex(string IndexName, params string[] FieldName);
        ISchemaBuilder AddInteger(string FieldName, string Description, bool Nullable = true, int DefaultValue = int.MinValue);
        ISchemaBuilder AddPrimaryKey(params string[] FieldName);
        ISchemaBuilder AddString(string FieldName, string Description, int Length, bool Nullable = true, string DefaultValue = "");
        ISchemaBuilder AddText(string FieldName, string Description, bool Nullable = true);
        ISchemaBuilder AddUniqueIndex(string IndexName, params string[] FieldName);
        string[] Columns();
        ISchemaBuilder Create();
        ISchemaBuilder CreateTable(string Name, string Description);
        ISchemaBuilder Drop();
        ISchemaBuilder DropTable(string Name);
        bool Exist();
        void OnOpen(IDataSource datasource);
        ISchemaBuilder RemoveIndex(string IndexName);
        ISchemaBuilder View(string Name, string Description);
        ISchemaBuilder OnCreate(string SqlScript);
        ISchemaBuilder WithColumn(string FieldName, string Description, string Type, bool Nullable = true, string DefaultValue = "");

        string ToString();
        void Build();
        Task BuildAsync();
    }
}