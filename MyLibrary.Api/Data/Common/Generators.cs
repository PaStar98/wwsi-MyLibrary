using Bogus;
using MyLibrary.Api.Data.Entities;

namespace MyLibrary.Api.Data.Common;

public static class Generators
{
    private const int Seed = 123;

    public static IEnumerable<Book> GenerateBooks(int count)
    {
        var faker = new Faker<Book>()
            .UseSeed(Seed)
            .RuleFor(x => x.Id, f => Guid.CreateVersion7(f.Date.PastOffset()))
            .RuleFor(x => x.Title, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Author, f => f.Person.FullName)
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph(2));
        
        return faker.GenerateLazy(count);
    }
}