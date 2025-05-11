using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WalletApi.Presentation.DTO;

public record BalanceUpdateDto(
    [Required] long UserId,
    [Required] decimal Amount
)
{
}