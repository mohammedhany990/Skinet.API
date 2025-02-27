namespace Skinet.API.Features.Carts.Responses
{
    public class CartItemResponse
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
        public string PictureUrl { get; set; }
    }
}
