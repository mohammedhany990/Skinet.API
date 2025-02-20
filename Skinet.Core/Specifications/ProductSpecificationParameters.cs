namespace Skinet.Core.Specifications
{
    public class ProductSpecificationParameters
    {
        public string? sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        private int pageSize = 10;

        public string? search { get; set; }
        public string? TypeName { get; set; }
        public string? BrandName { get; set; }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 15 ? 15 : value;
        }
        public int PageIndex { get; set; } = 1;

    }
}
