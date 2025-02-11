using AutoMapper;
using Skinet.API.DTOs;
using Skinet.API.DTOs.Basket;
using Skinet.API.DTOs.Folder;
using Skinet.API.DTOs.Identity;
using Skinet.API.DTOs.Order;
using Skinet.API.Features.Baskets.Commands.Create;
using Skinet.API.Features.Baskets.Models;
using Skinet.API.Features.Orders.Models;
using Skinet.API.Features.ProductBrands.Commands.Create;
using Skinet.API.Features.ProductBrands.Models;
using Skinet.API.Features.Products.Commands.Create;
using Skinet.API.Features.Products.Commands.Update;
using Skinet.API.Features.Products.Models;
using Skinet.API.Features.ProductTypes.Commands.Create;
using Skinet.API.Features.ProductTypes.Models;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Entities.Order;

namespace Skinet.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>())
                .ForMember(dest => dest.ProductBrand, src => src.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, src => src.MapFrom(src => src.ProductType.Name));


            //CreateMap<Address, AddressDto>().ReverseMap();
            //CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            //CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();





            //CreateMap<AddressDto, UserOrderAddress>();


            //CreateMap<Order, OrderToReturnDto>()
            //    .ForMember(dest => dest.DeliveryMethod, src => src.MapFrom(N => N.DeliveryMethod.ShortName))
            //    .ForMember(dest => dest.Price, src => src.MapFrom(C => C.DeliveryMethod.Price));


            //CreateMap<OrderItem, OrderItemDto>()
            //    .ForMember(dest => dest.ProductName, src => src.MapFrom(I => I.ItemOrdered.ProductName))
            //    .ForMember(dest => dest.ProductId, src => src.MapFrom(I => I.ItemOrdered.ProductItemId))
            //    .ForMember(dest => dest.PictureUrl, src => src.MapFrom<OrderItemPictureUrlResolver>());


            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());


            CreateMap<ProductType, ProductTypeModel>();
            CreateMap<ProductBrand, ProductBrandModel>();


            CreateMap<CreateProductTypeCommand, ProductType>();
            CreateMap<CreateProductBrandCommand, ProductBrand>();
            CreateMap<CustomerBasket, CustomerBasketModel>().ReverseMap();
            CreateMap<BasketItemModel, BasketItem>();
            CreateMap<CreateBasketCommand, CustomerBasket>().ReverseMap();
            
            
            CreateMap<AddressModel, UserOrderAddress>().ReverseMap();

            CreateMap<OrderItem, OrderItemModel>()
                .ForMember(d => d.ProductName, o => o.MapFrom(i => i.ItemOrdered.ProductName))
                .ForMember(d => d.ProductId, o => o.MapFrom(i => i.ItemOrdered.ProductItemId))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<UserOrderAddress, UserOrderAddressModel>().ReverseMap();

            CreateMap<Order, OrderModel>()
                .ForMember(m => m.DeliveryMethod, d => d.MapFrom(n => n.DeliveryMethod.ShortName))
                .ForMember(m => m.Price, i => i.MapFrom(c => c.DeliveryMethod.Price))
                .ForMember(m => m.ShippingAddress, o => o.MapFrom(c => c.ShipToAddress)) 
                .ForMember(m => m.Items, o => o.MapFrom(c => c.OrderItems)) 
                .ForMember(m => m.Total, o => o.MapFrom(c => c.GetTotal)); 

        

        }


    }
}


