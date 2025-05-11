using System.ComponentModel.DataAnnotations;

namespace WalletApi.Presentation.DTO;

public record LoginDto(
    [Required(ErrorMessage = "Email is required")]
    string Email,
    [Required(ErrorMessage = "Password is required")]
    string Password
);