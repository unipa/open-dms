namespace Elmi.Core.DataAccess.Interfaces;

public interface IDatabaseBuilder
{
    ISchemaBuilder Build(ISchemaBuilder SchemaBuilder);
}
