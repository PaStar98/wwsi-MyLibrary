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
            .RuleFor(x => x.Publisher, f => f.Company.CompanyName())
            .RuleFor(x => x.Year, f => f.Date.Past(10).Year)
            .RuleFor(x => x.ISBN, f => f.Commerce.Ean13())
            .RuleFor(x => x.Genre, f => f.Commerce.Categories(1).First())
            .RuleFor(x => x.ShelfId, f => f.Random.Int(1, 100))
            .RuleFor(x => x.Pages, f => f.Random.Int(100, 1000))
            .RuleFor(x => x.Copies, f => f.Random.Int(1, 10));

        return faker.GenerateLazy(count);
    }
}