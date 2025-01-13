namespace OrderManagement.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        private Product() { } // Pentru EF Core

        public Product(string name, string description, decimal price, int stock)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }

        public void ReduceStock(int quantity)
        {
            if (Stock < quantity)
                throw new InvalidOperationException("Not enough stock.");
            Stock -= quantity;
        }
    }
}
