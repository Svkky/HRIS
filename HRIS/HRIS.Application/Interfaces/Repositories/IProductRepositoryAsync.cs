using HRIS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<BranchProduct>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
