using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLibrary.Api.Data.Common;

namespace MyLibrary.Api.Data.Entities;

public class Book : BaseModel
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string? Description { get; set; }
    
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
            builder.Property(x => x.Description)
                .IsUnicode()
                .HasMaxLength(2047);
        }
    }
}