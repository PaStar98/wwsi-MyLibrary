using MyLibrary.Api.Data.Entities;

namespace MyLibrary.Api.Data.Dtos;

public class BookUpdateRequestDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public int? Year { get; set; }
    public string? ISBN { get; set; }
    public string? Genre { get; set; }
    public int? ShelfId { get; set; }
    public int? Pages { get; set; }
    public int? Copies { get; set; }
}

public static class BookUpdateRequestDtoMapper
{
    public static void Map(BookUpdateRequestDto bookDto, Book book)
    {
        if (bookDto.Title != null)
        {
            book.Title = bookDto.Title;
        }
        if (bookDto.Author != null)
        {
            book.Author = bookDto.Author;
        }
        if (bookDto.Publisher != null)
        {
            book.Publisher = bookDto.Publisher;
        }
        if (bookDto.Year != null)
        {
            book.Year = bookDto.Year.Value;
        }
        if (bookDto.ISBN != null)
        {
            book.ISBN = bookDto.ISBN;
        }
        if (bookDto.Genre != null)
        {
            book.Genre = bookDto.Genre;
        }
        if (bookDto.ShelfId != null)
        {
            book.ShelfId = bookDto.ShelfId.Value;
        }
        if (bookDto.Pages != null)
        {
            book.Pages = bookDto.Pages.Value;
        }
        if (bookDto.Copies != null)
        {
            book.Copies = bookDto.Copies.Value;
        }
    }
}
