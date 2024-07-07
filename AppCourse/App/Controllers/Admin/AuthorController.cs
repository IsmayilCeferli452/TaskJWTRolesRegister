using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Authors;
using Service.DTOs.Admin.BookAuthors;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    public class AuthorController : BaseController
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _authorService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _authorService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto model)
        {
            await _authorService.CreateAsync(model);

            return CreatedAtAction(nameof(Create), model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _authorService.DeleteAsync(id);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] AuthorEditDto model)
        {
            await _authorService.EditAsync(id, model);

            return Ok();
        }
    }
}
