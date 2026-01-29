using TesteFullStack.Domain.Entities;
using TesteFullStack.Domain.Interfaces;
using TesteFullStack.Infra.Data;

namespace TesteFullStack.Infra.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
