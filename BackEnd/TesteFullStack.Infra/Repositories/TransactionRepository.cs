using Microsoft.EntityFrameworkCore;
using TesteFullStack.Domain.Entities;
using TesteFullStack.Domain.Interfaces;
using TesteFullStack.Infra.Data;

namespace TesteFullStack.Infra.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Transaction>> GetWithIncludesAsync()
        {
            return await _context.Transactions
                .Include(t => t.Person)
                .Include(t => t.Category)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
