using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.BookAuthors;
using Service.DTOs.Admin.Books;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _bookService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDto model)
        {
            await _bookService.CreateAsync(model);

            return CreatedAtAction(nameof(Create), model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _bookService.DeleteAsync(id);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] BookEditDto model)
        {
            await _bookService.EditAsync(id, model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddToGroup([FromQuery] BookAuthorCreateDto model)
        {
            await _bookService.AddGroupAsync(model);

            return CreatedAtAction(nameof(AddToGroup), model);
        }
    }
}
