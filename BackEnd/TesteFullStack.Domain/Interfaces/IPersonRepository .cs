using TesteFullStack.Domain.Entities;

namespace TesteFullStack.Domain.Interfaces
{
    public interface IPersonRepository : IRepositoryBase<Person>
    {
        Task DeletePersonAndTransactionsAsync(Guid personId);
    }
}
