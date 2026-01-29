using Microsoft.AspNetCore.Mvc;
using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.Interfaces;

namespace TesteFullStack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionService.GetAllAsync();
            return Ok(transactions);
        }

        [HttpGet("totais-por-pessoa")]
        public async Task<IActionResult> GetTotalsByPerson()
        {
            var result = await _transactionService.GetTotalsByPersonAsync();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionRequest request)
        {
            var created = await _transactionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }
    }
}
