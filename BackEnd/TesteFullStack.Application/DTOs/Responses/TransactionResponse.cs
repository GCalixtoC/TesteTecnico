using TesteFullStack.Domain.Entities;

namespace TesteFullStack.Application.DTOs.Responses
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }
        public decimal Value { get; set; }
        public string? Type { get; set; }

        public string? PersonName { get; set; }       
        public string? CategoryPurpose { get; set; }

    }
}
