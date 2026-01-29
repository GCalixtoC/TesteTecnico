using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.DTOs.Responses;
using TesteFullStack.Application.Interfaces;
using TesteFullStack.Domain.Entities;
using TesteFullStack.Domain.Interfaces;

namespace TesteFullStack.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(x => new CategoryResponse
            {
               Id = x.Id,
               Description = x.Description,
               Purpose = x.Purpose,
            });
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            var entity = new Category
            {
                Description = request.Description,
                Purpose = request.Purpose,
            };

            await _categoryRepository.AddAsync(entity);

            return new CategoryResponse
            {
                Id = entity.Id,
                Description= entity.Description,
                Purpose= entity.Purpose,
            };
        }
    }
}
