using TesteFullStack.Domain.Common;

namespace TesteFullStack.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
    }
}
