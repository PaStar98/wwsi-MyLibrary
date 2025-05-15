using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Api.Data;
using MyLibrary.Api.Data.Dtos;
using MyLibrary.Api.Data.Entities;

namespace MyLibrary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class BooksController(LibraryDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooks(string? title = null, string? author = null, string? isbn = null, string? genre = null)
    {
        var booksQuery = context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            booksQuery = booksQuery.Where(b => b.Title.Contains(title));
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            booksQuery = booksQuery.Where(b => b.Author.Contains(author));
        }

        if (!string.IsNullOrWhiteSpace(isbn))
        {
            booksQuery = booksQuery.Where(b => b.ISBN.Contains(isbn));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            booksQuery = booksQuery.Where(b => b.Genre.Contains(genre));
        }

        var books = await booksQuery.ToListAsync();

        Response.Headers.Append("Total-Count", books.Count.ToString());

        return Ok(books);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBookById(Guid id)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> AddBook(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBook(Guid id, [FromBody] BookUpdateRequestDto bookUpdateRequestDto)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        if (bookUpdateRequestDto.ISBN != book.ISBN)
        {
            var existingBookWithIsbn = await context.Books
                .FirstOrDefaultAsync(b => b.ISBN == bookUpdateRequestDto.ISBN);

            if (existingBookWithIsbn != null)
            {
                return Conflict("A book with the same ISBN already exists.");
            }
        }

        if (!string.IsNullOrWhiteSpace(bookUpdateRequestDto.Title) && !string.IsNullOrWhiteSpace(bookUpdateRequestDto.Author))
        {
            var existingBookWithTitleAndAuthor = await context.Books
                .FirstOrDefaultAsync(b => b.Title == bookUpdateRequestDto.Title && b.Author == bookUpdateRequestDto.Author);

            if (existingBookWithTitleAndAuthor != null)
            {
                return Conflict("A book with the same title and author already exists.");
            }
        }

        BookUpdateRequestDtoMapper.Map(bookUpdateRequestDto, book);

        context.Entry(book).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(Guid id)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}/borrow")]
    public async Task<ActionResult> BorrowBook(Guid id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        if (book.Copies > 0)
        {
            book.Copies--;
            context.Books.Update(book);
            await context.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return Conflict("No copies available to borrow.");
        }
    }

    [HttpPatch("{id}/return")]
    public async Task<ActionResult> ReturnBook(Guid id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        book.Copies++;
        context.Books.Update(book);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}/e-book")]
    public async Task<ActionResult> GetEBook(Guid id)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        // Assuming the external e-book URL is stored in the EBookUrl property
        var eBookUrl = "https://student.wwsi.edu.pl/upload/inne/Obowiazek_informacyjny.pdf";  // This should be the URL of the PDF file

        if (string.IsNullOrWhiteSpace(eBookUrl))
        {
            return NotFound("E-book URL not available.");
        }

        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(eBookUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound("Unable to retrieve e-book from the provided URL.");
                }

                var fileBytes = await response.Content.ReadAsByteArrayAsync();

                // Set the Content-Disposition header to trigger download
                Response.Headers.Add("Content-Disposition", "attachment; filename=e-book.pdf");

                return File(fileBytes, "application/pdf");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private bool BookExists(Guid id)
    {
        return context.Books.Any(e => e.Id == id);
    }
}