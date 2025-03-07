namespace Skinet.Core.Entities.Cart
{
    public class CartItem
    {
        public CartItem()
        {

        }
        public string Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
        public string PictureUrl { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice => Quantity * Price;


        public CartItem(int productId, string productName, decimal price, int quantity, string brand, string type, string picture)
        {
            Id = Guid.NewGuid().ToString();
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;

            ProductBrand = brand;
            ProductType = type;
            PictureUrl = picture;
        }
    }
}