namespace WalletApi.Domain.Entity;

public class Wallet
{
    public long Id { get; set; }

    public decimal Balance { get; set; }

    public long UserId { get; set; }
    public User User { get; set; } = null!;
}