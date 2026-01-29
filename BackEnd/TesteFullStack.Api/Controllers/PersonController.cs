using Microsoft.AspNetCore.Mvc;
using TesteFullStack.Application.DTOs.Requests;
using TesteFullStack.Application.Interfaces;

namespace TesteFullStack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personservice;

        public PersonController(IPersonService personService)
        {
            _personservice = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _personservice.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var person = await _personservice.GetByIdAsync(id);
            return person == null ? NotFound() : Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonRequest request)
        {
            var created = await _personservice.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePersonRequest request)
        {
            var updated = await _personservice.UpdateAsync(request);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _personservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
