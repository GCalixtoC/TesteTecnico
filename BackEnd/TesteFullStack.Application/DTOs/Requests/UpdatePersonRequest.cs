namespace TesteFullStack.Application.DTOs.Requests
{
    public class UpdatePersonRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
    }
}
