using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.DTOs.Responses;
using TesteFullStack.Application.Interfaces;
using TesteFullStack.Domain.Entities;
using TesteFullStack.Domain.Interfaces;

namespace TesteFullStack.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<PersonResponse>> GetAllAsync()
        {
            var people = await _personRepository.GetAllAsync();

            return people.Select(x => new PersonResponse
            {
                Id = x.Id,
                Name = x.Name,
                Age = x.Age
            });
        }

        public async Task<PersonResponse?> GetByIdAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);

            if (person == null) 
                return null;

            return new PersonResponse
            {
                Id = person.Id,
                Name = person.Name,
                Age = person.Age
            };
        }

        public async Task<PersonResponse> CreateAsync(CreatePersonRequest request)
        {
            var entity = new Person
            {
                Name = request.Name!,
                Age = request.Age
            };

            await _personRepository.AddAsync(entity);

            return new PersonResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Age = entity.Age
            };
        }

        public async Task<PersonResponse> UpdateAsync(UpdatePersonRequest request)
        {
            // Validação básica de entrada
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Id == Guid.Empty)
                throw new Exception("O identificador informado é inválido.");

            // Busca a pessoa
            var entity = await _personRepository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new Exception("Pessoa não encontrada.");

            // Atualização condicional (mantém valores anteriores se vier nulo)
            entity.Name = request.Name ?? entity.Name;
            entity.Age = request.Age ?? entity.Age;

            //  Atualiza o registro
            await _personRepository.UpdateAsync(entity);

            // Retorna o DTO atualizado
            return new PersonResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Age = entity.Age
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _personRepository.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Pessoa não encontrada.");

            await _personRepository.DeleteAsync(entity);
        }
    }
}
