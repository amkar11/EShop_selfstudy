using AutoMapper;
using ShoppingCart.Domain.Dto;
using ShoppingCart.Domain.Models;
using User.Domain.Models;
using User.Domain.PasswordHasher;
namespace Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegistrationViewModel, UserDb>()
          .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        CreateMap<ShopCartItem, ShopCartDto>();
    }
}
