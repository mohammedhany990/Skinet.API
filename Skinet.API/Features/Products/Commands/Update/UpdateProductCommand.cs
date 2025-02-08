using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Products.Commands.Update
{
    public class UpdateProductCommand : IRequest<BaseResponse<string>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? PictureUrl { get; set; }
        public decimal? Price { get; set; }
        public int? ProductBrandId { get; set; }
        public int? ProductTypeId { get; set; }
    }
}
