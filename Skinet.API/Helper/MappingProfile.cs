using AutoMapper;
using Skinet.API.DTOs.Identity;
using Skinet.API.Features.Carts.Responses;
using Skinet.API.Features.Favorites.Responses;
using Skinet.API.Features.Orders.Responses;
using Skinet.API.Features.ProductBrands.Commands.Create;
using Skinet.API.Features.ProductBrands.Responses;
using Skinet.API.Features.Products.Commands.Create;
using Skinet.API.Features.Products.Commands.Update;
using Skinet.API.Features.Products.Responses;
using Skinet.API.Features.ProductTypes.Commands.Create;
using Skinet.API.Features.ProductTypes.Responses;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Cart;
using Skinet.Core.Entities.Order;

namespace Skinet.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>())
                .ForMember(dest => dest.ProductBrand, src => src.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, src => src.MapFrom(src => src.ProductType.Name));

            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());


            CreateMap<ProductType, ProductTypeResponse>();
            CreateMap<ProductBrand, ProductBrandResponse>();


            CreateMap<CreateProductTypeCommand, ProductType>();
            CreateMap<CreateProductBrandCommand, ProductBrand>();



            CreateMap<AddressModel, UserOrderAddress>().ReverseMap();

            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(d => d.ProductName, o => o.MapFrom(i => i.ItemOrdered.ProductName))
                .ForMember(d => d.ProductId, o => o.MapFrom(i => i.ItemOrdered.ProductItemId))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<UserOrderAddress, UserOrderAddressResponse>().ReverseMap();

            CreateMap<Order, OrderResponse>()
                .ForMember(m => m.DeliveryMethod, d => d.MapFrom(n => n.DeliveryMethod.ShortName))
                .ForMember(m => m.Price, i => i.MapFrom(c => c.DeliveryMethod.Price))
                .ForMember(m => m.ShippingAddress, o => o.MapFrom(c => c.ShipToAddress))
                .ForMember(m => m.Items, o => o.MapFrom(c => c.OrderItems));


            CreateMap<FavoriteItem, FavoriteItemResponse>();

            CreateMap<DeliveryMethod, DeliveryMethodResponse>();


            CreateMap<Cart, CartResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Items.Sum(item => item.TotalPrice) + src.ShippingPrice))
                .ReverseMap();



            CreateMap<CartItem, CartItemResponse>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<CartItemPictureUrlResolver>())
                .ReverseMap();



        }


    }
}


