using AutoMapper;
using WalletApi.Domain.Entity;
using WalletApi.Presentation.DTO;

namespace WalletApi.Presentation.Mapper;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<TransactionDto, Transaction>().ReverseMap();
    }
}