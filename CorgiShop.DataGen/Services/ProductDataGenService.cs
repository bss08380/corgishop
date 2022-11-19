using CorgiShop.Repo.Model;

namespace CorgiShop.DataGen.Services;

public class ProductDataGenService : IProductDataGenService
{
    private readonly string[] Prefixes = { "NEW", "Cute", "Adorable", "Dazzling", "Corgilicious", "Striped", "Squishy", "Mini", "Fuzzy", "Fluffy", "Angry", "Authentic", "Crazy", "Goofy", "Rare", "Collectible" };
    private readonly string[] Breeds = { "Pembroke", "Cardigan", "Welsh Pembroke", "Welsh Cardigan"};
    private readonly string[] ToyType = { "Plush", "Action Figure", "Statue", "Stickers", "Painting", "Gift Card", "Stuffed Animal" };
    private readonly string[] Postfixes = { "DELUX", "5 Pack", "10 Pack", "(Limited Edition)", "ULTRA EDITION" };

    public async Task GenerateProducts(CorgiShopDbContext dbContext, int productCount)
    {
        for (var i = 0; i < productCount; i++)
        {
            await dbContext.Products.AddAsync(CreateFakeCorgiProduct());
        }
        await dbContext.SaveChangesAsync();
    }

    private Product CreateFakeCorgiProduct()
    {
        var rand = new Random();
        return new Product()
        {
            Name = CreateFakeProductName(),
            Description = "This is a fake product with a fake name created by a fake(ish) person for fake reasons - do not take it seriously!",
            Stock = rand.Next(5, 50),
            Price = rand.Next(10, 200) + 0.99M,
            IsDeleted = rand.Next(0, 100) > 98 //delete a few for testing purposes
        };
    }

    private string CreateFakeProductName()
    {
        var prefix = GetRandomizedValue(Prefixes, null, 85);
        var breed = GetRandomizedValue(Breeds, null, 100);
        var toyType = GetRandomizedValue(ToyType, null, 100);
        var postfix = GetRandomizedValue(Postfixes, null, 10);

        var nameParts = new List<string>();
        if (prefix != null) nameParts.Add(prefix);
        nameParts.Add(breed!);
        nameParts.Add(toyType!);
        if (postfix != null) nameParts.Add(postfix);

        return string.Join(" ", nameParts);
    }

    private T GetRandomizedValue<T>(T[] values, T defaultValue, int chanceOfAnyValue = 100)
    {
        var rand = new Random();
        if (rand.Next(0, 100) + chanceOfAnyValue < 100) return defaultValue;
        return values[rand.Next(0, values.Length - 1)];
    }
}