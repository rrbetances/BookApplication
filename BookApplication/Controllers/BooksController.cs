using BookApplication.Models.Dtos;
using BookApplication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await bookService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await bookService.GetByIdAsync(id);

            if (result != null && result.IsSuccess)
                return Ok(result);

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDto book)
        {
            ResponseDto? response = await bookService.CreateAsync(book);

            if (response != null && response.IsSuccess)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDto book)
        {
            ResponseDto? response = await bookService.GetByIdAsync(id);

            if (response != null && response.IsSuccess)
            {
                book.Id = id;
                await bookService.UpdateAsync(book);

                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ResponseDto? response = await bookService.GetByIdAsync(id);

            if (response != null && response.IsSuccess)
            {
                await bookService.DeleteAsync(id);
                return Ok();
            }

            return BadRequest();
        }
    }
}
