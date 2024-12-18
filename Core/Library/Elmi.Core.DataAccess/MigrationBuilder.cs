using Elmi.Core.DataAccess.Interfaces;

namespace Elmi.Core.DataAccess;

public class MigrationBuilder
{
    private readonly ISchemaBuilder schemaBuilder;

    public static Dictionary<string, IList<IDatabaseBuilder>> Registry { get; private set; } = new Dictionary<string, IList<IDatabaseBuilder>>();

    public static void RegisterBuilders()
    {
        foreach (System.Reflection.Assembly A in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                foreach (Type T in A.GetTypes())
                {
                    if (T.IsClass && T.GetInterface("IDatabaseBuilder") != null)
                    {
                        object C = Activator.CreateInstance(T);
                        IDatabaseBuilder builder = (IDatabaseBuilder)C;
                        Register("",builder);
                    }
                }
            }
            catch { };
        }
    }

    public static IDatabaseBuilder Register(string DatabaseTemplate, IDatabaseBuilder builder)
    {
        if (!Registry.ContainsKey(DatabaseTemplate)) Registry.Add(DatabaseTemplate, new List<IDatabaseBuilder>());
        var Builders = Registry[DatabaseTemplate];
        Builders.Add(builder);
        return builder;
    }

    public MigrationBuilder(string connectionString, string provider)
    {
        this.schemaBuilder = new SchemaBuilder(connectionString, provider);
    }



    public void Build(String DatabaseTemplate)
    {
        string _ModelID = DatabaseTemplate + "";
        if (!Registry.ContainsKey(_ModelID)) return;

        var Builders = Registry[_ModelID];
        var builder = schemaBuilder;
        if (Builders != null)
        {
            if (!builder.Exist())
            {
                builder.Create();
            }
            foreach (var B in Builders)
                builder = B.Build(builder);

            //var hash = MessageDigest.Hash ( builder.ToString());
            //TODO: Cerca l'hash sul database 
            // se l'hash non esite applica la migration
            builder.Build();
        }
    }


    public async Task BuildAsync(String DatabaseTemplate)
    {
        string _ModelID = DatabaseTemplate + "";
        if (!Registry.ContainsKey(_ModelID)) return;

        var Builders = Registry[_ModelID];
        var builder = schemaBuilder;
        if (Builders != null)
        {
            foreach (var B in Builders)
                builder = B.Build(builder);
            await builder.BuildAsync();
        }
    }

}
