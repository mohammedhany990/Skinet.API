using MediatR;
using Skinet.Core.Entities;
using Skinet.Core.Helper;
using System.ComponentModel.DataAnnotations;

namespace Skinet.API.Features.Products.Commands.Create
{
    public class CreateProductCommand : IRequest<BaseResponse<string>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
    }
}
