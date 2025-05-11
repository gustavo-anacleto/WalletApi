using AutoMapper;
using WalletApi.Domain.Entity;
using WalletApi.Presentation.DTO;

namespace WalletApi.Presentation.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserCreateDto, KeycloakUserDto>()
            .ForMember(ku => ku.Credentials, opt => opt
                .MapFrom(src => new List<CredentialDto>()
                {
                    new CredentialDto()
                    {
                        Value = src.Password
                    }
                }))
            .ForMember(ku => ku.Attributes, opt => opt.MapFrom(src => src.Name));
       
        CreateMap<User, UserListDto>()
            .ForMember(u => u.Balance, opt => opt
                .MapFrom(src => src.Wallet.Balance));
    }
}