using WalletApi.Domain.Entity;

namespace WalletApi.Presentation.DTO;

public class TransactionDto
{
    public long Id { get; set; }

    public UserDto Sender { get; set; } = null!;

    public UserDto Receiver { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
}