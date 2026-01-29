using TesteFullStack.Domain.Common;

namespace TesteFullStack.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public string? Type { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }  

        public Guid PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
 