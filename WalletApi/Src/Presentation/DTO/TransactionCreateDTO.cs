using System.ComponentModel.DataAnnotations;
using WalletApi.Domain.Entity;

namespace WalletApi.Presentation.DTO;

public record TransactionCreateDto(
    [Required] long ReceiverId,
    [Required] decimal Amount
)
{
}