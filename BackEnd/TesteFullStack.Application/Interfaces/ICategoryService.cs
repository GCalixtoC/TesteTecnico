using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.DTOs.Responses;

namespace TesteFullStack.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
    }
}
