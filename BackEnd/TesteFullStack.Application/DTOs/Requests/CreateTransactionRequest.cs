namespace TesteFullStack.Application.DTOs.Requests
{
    public class CreateTransactionRequest
    {
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public string Type { get; set; } = null!;

        public Guid PersonId { get; set; }
        public Guid CategoryId { get; set; }
    }
}