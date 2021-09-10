using HRIS.Application.DTOs.StoreProduct;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IStoreProductService
    {
        int CreateStoreProduct(CreateStoreProductDTO model);
        int DeleteStoreProduct(int StoreProductID);
        List<StoreProductDTO> GetAllStoreProducts();
        StoreProductDTO GetStoreProductByID(int StoreProductID);
        int UpdateStoreProduct(UpdateStoreProductDTO model);
    }
}
