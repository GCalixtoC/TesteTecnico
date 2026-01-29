using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.DTOs.Responses;

namespace TesteFullStack.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionResponse>> GetAllAsync();
        Task<TransactionResponse> CreateAsync(CreateTransactionRequest request);
        Task<PersonTotalsListResponse> GetTotalsByPersonAsync();
    }
}
