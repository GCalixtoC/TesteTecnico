using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.DTOs.Responses;
using TesteFullStack.Application.Interfaces;
using TesteFullStack.Domain.Entities;
using TesteFullStack.Domain.Interfaces;

namespace TesteFullStack.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IPersonRepository personRepository,
            ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _personRepository = personRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<TransactionResponse>> GetAllAsync()
        {
            var transactions = await _transactionRepository.GetWithIncludesAsync();

            return transactions.Select(x => new TransactionResponse
            {
                Id = x.Id,
                Description = x.Description,
                Value = x.Value,
                Type = x.Type,
                PersonName = x.Person?.Name,               
                CategoryPurpose = x.Category?.Purpose,
            });
        }

        public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request)
        {
            // Busca pessoa e categoria (valida se existem)
            var person = await _personRepository.GetByIdAsync(request.PersonId)
              ?? throw new Exception("Pessoa não encontrada");

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
                ?? throw new Exception("Categoria não encontrada");

            //  Regra de negócio — menor de idade só pode registrar despesas
            var type = request.Type.ToLower();
             if(person.Age < 18 && type != "despesa")
                throw new Exception("Menores de idade só podem registrar despesas.");

            // Regra de negócio — tipo deve ser compatível com a categoria
            var purpose = category.Purpose!.ToLower();

            bool tipoInvalidado;
                if(type == "despesa" && purpose == "receita")
                throw new Exception("Categoria incompatível com o tipo da transação.");

            //Criação
            var transaction = new Transaction
            {
                Description = request.Description,
                Value = request.Value,
                Type = request.Type,
                CategoryId = request.CategoryId,
                PersonId = request.PersonId,
            };

            await _transactionRepository.AddAsync(transaction);

            // Retorno DTO
            return new TransactionResponse
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Value = transaction.Value,
                Type = transaction.Type,
                PersonName = person.Name,
                CategoryPurpose = category.Purpose,
            };

        }

        public async Task<PersonTotalsListResponse> GetTotalsByPersonAsync()
        {
            var pessoas = await _personRepository.GetAllAsync();
            var transacoes = await _transactionRepository.GetAllAsync();

            var lista = new List<PersonTotalsResponse>();

            foreach (var pessoa in pessoas)
            {
                var transacoesPessoa = transacoes
                    .Where(t => t.PersonId == pessoa.Id)
                    .ToList();

                decimal totalReceitas = transacoesPessoa
                    .Where(t => t.Type!.ToLower() == "receita")
                    .Sum(t => t.Value);

                decimal totalDespesas = transacoesPessoa
                    .Where(t => t.Type!.ToLower() == "despesa")
                    .Sum(t => t.Value);

                lista.Add(new PersonTotalsResponse
                {
                    PersonId = pessoa.Id,
                    Name = pessoa.Name,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas
                });
            }

            var totalGeral = new TotalGeralResponse
            {
                TotalReceitas = lista.Sum(p => p.TotalReceitas),
                TotalDespesas = lista.Sum(p => p.TotalDespesas)
            };

            return new PersonTotalsListResponse
            {
                Pessoas = lista,
                TotalGeral = totalGeral
            };
        }
    }
}
