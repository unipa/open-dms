/*
namespace OpenDMS.Infrastructure.Repositories
{
    public class PostITRepository : IPostItRepository
	{
		private SingleTenantDbContext ds;

		public PostITRepository(SingleTenantDbContext ds)
		{
			this.ds = ds;
		}


		private PostIt Read(DbDataReader dr)
		{
			PostIt p = new PostIt();
			p.DataAsNumber =dr.GetInt32("NDATA");
			p.OraAsNumber = dr.GetInt32("NORA");
			p.IdDocumento = Convert.ToInt32(dr["NNUDC"].ToString());
			p.UserId = dr["NUSER"].ToString();
			p.Message = dr["NNOTA"].ToString();
			p.Id = Convert.ToInt32(dr["NNUID"].ToString());
			return p;
		}

		public async Task<PostIt> GetById(Int32 id)
		{
            return await ds.Connection.QueryFirstOrDefaultAsync("SELECT NNUID Id,NDATA DataAsNumber, NORA OraAsNumber, NUSER Utente, NNUDC IdDocumento, NNOTA Testo FROM POSTIT0F WHERE NNUID=" + id.ToString());
		}


	    public async Task<IEnumerable<PostIt>> GetByDocument(int documentId, int pageIndex, int pageSize)
		{
            return (await ds.Connection.QueryAsync<PostIt>($"SELECT NNUID Id,NDATA DataAsNumber, NORA OraAsNumber, NUSER Utente, NNUDC IdDocumento, NNOTA Testo FROM POSTIT0F WHERE NNUDC={documentId} ORDER BY NDATA DESC,NNUID DESC")).Skip(pageIndex*pageSize).Take(pageSize);
		}

		public async Task<Int32> CountByDocument(Int32 documentId)
		{
            return await ds.Connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM POSTIT0F WHERE NNUDC={documentId} ORDER BY NDATA DESC,NNUID DESC");
		}

		public async Task Add(PostIt postIt)
		{
            postIt.Id = ds.NextID("POST-IT", "ID", "");
			postIt.Data = DateTime.UtcNow;
            postIt.Message = postIt.Message.Replace("\n", "<BR/>");
			await ds.Connection.ExecuteAsync("INSERT INTO POSTIT0F (NNUID,NNUDC,NDATA,NUSER,NNOTA,NORA) VALUES (@Id,@IdDocumento,@DataAsNumber,@Utente,@Testo,@OraAsNumber)", postIt);
		}

		public async Task Delete(Int32 Id)
		{
            PostIt N =  await GetById(Id);
			string sql = string.Format("DELETE FROM POSTIT0F WHERE NNUID ={0}", Id);
			Int32 r = ds.Execute(sql);
	        //return r;
		}

	}
}
*/