using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLibrary.Api.Data.Common;

namespace MyLibrary.Api.Data.Entities;

public class Book : BaseModel
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Publisher { get; set; }
    public int Year { get; set; }
    public required string ISBN { get; set; }
    public required string Genre { get; set; }
    public int ShelfId { get; set; }
    public int Pages { get; set; }
    public int Copies { get; set; }

    public class Configuration : BaseConfiguration<Book>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Title)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);
            builder.Property(x => x.Author)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);
            builder.Property(x => x.Publisher)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);
            builder.Property(x => x.Year)
                .IsRequired()
                .HasMaxLength(4);
            builder.Property(x => x.ISBN)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(13);
            builder.Property(x => x.Genre)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
            builder.Property(x => x.ShelfId)
                .IsRequired();
            builder.Property(x => x.Pages)
                .IsRequired()
                .HasMaxLength(4);
            builder.Property(x => x.Copies)
                .IsRequired()
                .HasMaxLength(4);
        }
    }
}