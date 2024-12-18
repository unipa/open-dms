using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
/// <summary>
/// Descrizione di riepilogo per BancheDatiProvider
/// </summary>
/// 
namespace OpenDMS.Core.BusinessLogic
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IList<Company>> GetByUser(UserProfile userInfo)
        {
            return await _repository.GetAll();
        }
        public async Task<IList<Company>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Company> GetById(int cbd)
        {
            return await _repository.GetById(cbd);
        }


        public async Task Create(Company bd)
        {
            await _repository.Insert(bd);
        }

        public async Task Update(Company bd)
        {
            await _repository.Update(bd);
        }

        public async Task Delete(Company bd)
        {
            if ((await _repository.GetAll()).Count < 1) throw new InvalidOperationException();
            await _repository.Delete(bd.Id);
        }

        public async Task CheckDemo()
        {
            var BList = await GetAll();
            if (BList == null || BList.Count <= 0)
            {
                Company B = new Company() { Id = 1, Description = "Ambiente Demo" };
                await Create(B);
            }
        }


    }
}