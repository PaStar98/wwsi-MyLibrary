using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Api.Data;
using MyLibrary.Api.Data.Entities;

namespace MyLibrary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController(LibraryDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooks(string? title = null)
    {
        var booksQuery = context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            booksQuery = booksQuery.Where(b => b.Title.Contains(title));
        }
        
        var books = await booksQuery.ToListAsync();
        
        return Ok(books);
    }
}