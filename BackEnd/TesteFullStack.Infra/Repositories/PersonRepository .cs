using Microsoft.EntityFrameworkCore;
using TesteFullStack.Domain.Entities;
using TesteFullStack.Domain.Interfaces;
using TesteFullStack.Infra.Data;

namespace TesteFullStack.Infra.Repositories
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext context) : base(context) { }

        public async Task DeletePersonAndTransactionsAsync(Guid personId)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == personId);
            if (person == null) return;

            // Quando criarmos Transaction, removeremos as transações ligadas aqui
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}
