﻿namespace Catalog.Products.Models;
public class Product : Aggregate<Guid>
{
    public string Name { get; private set; } = default!;
    public List<string> Category { get; private set; } = new();
    public string Description { get; private set; } = default!;
    public string ImageFile { get; private set; } = default!;
    public decimal Price { get; private set; }

    public static Product Create(Guid id , string name , List<string> category , string description , string imageFile , decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var prodcut = new Product
        {
            Id = id,
            Name = name,
            Category = category,
            Description = description,
            ImageFile = imageFile,
            Price = price
        };

        prodcut.AddDomainEvent(new ProductCreatedEvent(prodcut));

        return prodcut;
    }

    public void Update(string name, List<string> category, string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
        Price = price; 

        if(Price  != price)
        {
            Price = price;
            AddDomainEvent(new ProductPriceChangedEvent(this));
        }
    }

   
}
