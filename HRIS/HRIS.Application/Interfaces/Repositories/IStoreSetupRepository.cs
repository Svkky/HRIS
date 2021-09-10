using HRIS.Application.DTOs;
using HRIS.Application.DTOs.Store;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IStoreSetupRepository
    {
        Task<int> CreateStore(StoreSetUpDTO model);
        List<StoreSetUpDTO> GetAllStores();
        StoreSetUpDTO GetStoreByID(int StoreID);
        int UpdateStore(StoreUpdateDTO model);
    }
}