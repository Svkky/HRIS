using HRIS.Application.DTOs.Category;
using System.Collections.Generic;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface ICategoryService
    {
        int CreateCategory(CreateCategoryDTO model);
        int UpdateCategory(UpdateCategoryDTO model);
        int DeleteCategory(DeleteCategoryDTO model);
        List<CategoryDTO> GetAllCategory();
        CategoryVm GetAllCategoryRecord();
        CategoryDTO GetCategoryById(int categoryId);
    }
}
