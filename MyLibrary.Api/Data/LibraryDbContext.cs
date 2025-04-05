using Microsoft.EntityFrameworkCore;
using MyLibrary.Api.Data.Entities;

namespace MyLibrary.Api.Data;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new Book.Configuration());
    }
}