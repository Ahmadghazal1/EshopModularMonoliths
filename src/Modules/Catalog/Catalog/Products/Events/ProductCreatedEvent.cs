namespace Catalog.Products.Events;
public record ProductCreatedEvent (Product Prodcut) 
        :IDomainEvent;

