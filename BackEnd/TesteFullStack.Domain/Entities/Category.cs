using TesteFullStack.Domain.Common;

namespace TesteFullStack.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string? Description { get; set; }
        public string? Purpose { get; set; }
    }
}
