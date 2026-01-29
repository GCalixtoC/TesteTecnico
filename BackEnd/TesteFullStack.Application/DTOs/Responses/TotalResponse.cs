namespace TesteFullStack.Application.DTOs.Responses
{
    public class PersonTotalsResponse
    {
        public Guid PersonId { get; set; }
        public string? Name { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo => TotalReceitas - TotalDespesas;
    }

    public class TotalGeralResponse
    {
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal SaldoLiquido => TotalReceitas - TotalDespesas;
    }

    public class PersonTotalsListResponse
    {
        public IEnumerable<PersonTotalsResponse> Pessoas { get; set; } = new List<PersonTotalsResponse>();
        public TotalGeralResponse TotalGeral { get; set; } = new TotalGeralResponse();
    }
}
