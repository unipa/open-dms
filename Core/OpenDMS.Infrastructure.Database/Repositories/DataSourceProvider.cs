using Elmi.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;
using System.Data.Common;

namespace OpenDMS.Infrastructure.Repositories
{

    /// <summary>
    /// Classe per operazioni CRUD sulle ExternalDatasource
    /// </summary>
    public class DataSourceProvider : IDataSourceProvider
    {
        private ApplicationDbContext ctx;

        public DataSourceProvider(IApplicationDbContextFactory factory)
        {
            this.ctx = (ApplicationDbContext)factory.GetDbContext();
        }

        public ExternalDatasource this[string Name]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public ExternalDatasource this[string Company, string Name]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public async Task<ExternalDatasource> Get(string Id)
        {
            var result = await ctx.ExternalDataSources.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (result != null)
            {
                result.Password = CryptoUtils.Decrypt(result.Password);
            }
            return result;
        }

        public async Task<Dictionary<string, ExternalDatasource>> GetAll()
        {

            var list = await ctx.ExternalDataSources.AsNoTracking().ToListAsync();

            var dict = new Dictionary<string, ExternalDatasource>();

            foreach (ExternalDatasource extDs in list)
            {
                dict.Add(extDs.Id, extDs);
            }
            return dict;
        }

        public async Task<int> Set(string Id, ExternalDatasource Value)
        {
            if (string.IsNullOrEmpty(Value.Name))
            {
                throw new Exception("Non è stato assegnato un nome a questa connessione");
            }

            bool flagCreazione = string.IsNullOrEmpty(Value.Id);
            if (flagCreazione)
            {
                Value.Id = Guid.NewGuid().ToString();
            }
            Value.Password = CryptoUtils.Encrypt(Value.Password);

            var source = await Get(Value.Id);
            if (source == null)
            {
                //caso di insert 
                ctx.ExternalDataSources.Add(Value);
                return await ctx.SaveChangesAsync();
            }
            else
            {
                //caso di update 
                source.Name = Value.Name;
                source.Password = Value.Password;
                source.Driver = Value.Driver;
                source.Provider = Value.Provider;
                source.UserName = Value.UserName;
                source.Connection = Value.Connection;
                return await ctx.SaveChangesAsync();
            }

        }

        public async Task<int> Delete(string Id)
        {
            var element = await Get(Id);
            if (element != null)
            {
                ctx.Remove(element);
                return await ctx.SaveChangesAsync();
            }
            else throw new Exception("Datasource non trovato");

        }


        public async Task TestConnection(ExternalDatasource Source)
        {
            var factory = DbProviderFactories.GetFactory(Source.Driver);
            IDbConnection connection = factory.CreateConnection();
            connection.ConnectionString = Source.ConnectionString;
            try
            {
                connection.Open();
                connection.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<string[]>> Query(string Id, string Query, int pageSize = 50, int pageIndex = 0)
        {
            ExternalDatasource Source = await Get(Id);

            List<string[]> Results = new List<string[]>();
            using (DataSource DS = new DataSource(Source.ConnectionString, Source.Driver))
            {
                using (var reader = DS.Select(Query, pageIndex * pageSize, pageSize))
                {
                    string[] fieldsArray = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        fieldsArray[i] = reader.GetName(i);
                    }
                    Results.Add(fieldsArray);

                    while (reader.Read())
                    {
                        fieldsArray = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            fieldsArray[i] = reader[i].ToString();
                            if (reader.GetDataTypeName(i).ToLower().Contains("date") && !String.IsNullOrEmpty(fieldsArray[i]) )
                            {
                                var dt = reader.GetDateTime(i);
                                fieldsArray[i] = dt.ToString("yyyy-MM-dd"); // fieldsArray[i].Substring(6, 4) + "-" + fieldsArray[i].Substring(3, 2) + "-" + fieldsArray[i].Substring(0, 2);
                            }
                        }
                        Results.Add(fieldsArray);
                    }
                }
            }
            return Results;
        }
    }
}
