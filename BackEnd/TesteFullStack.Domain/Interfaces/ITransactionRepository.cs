using TesteFullStack.Domain.Entities;

namespace TesteFullStack.Domain.Interfaces
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        Task<IEnumerable<Transaction>> GetWithIncludesAsync();
    }
}
