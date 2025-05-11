using System.ComponentModel.DataAnnotations;
using WalletApi.Domain.Enums;

namespace WalletApi.Presentation.DTO;

public class UserCreateDto
{
    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    public UserType UserType { get; set; } = UserType.USER;
}