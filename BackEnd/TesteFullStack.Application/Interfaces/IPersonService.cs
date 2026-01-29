using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.DTOs.Responses;

namespace TesteFullStack.Application.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonResponse>> GetAllAsync();
        Task<PersonResponse?> GetByIdAsync(Guid id);
        Task<PersonResponse> CreateAsync(CreatePersonRequest request);
        Task<PersonResponse> UpdateAsync(UpdatePersonRequest request);
        Task DeleteAsync(Guid id);
    }
}
