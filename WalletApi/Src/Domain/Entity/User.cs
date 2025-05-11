namespace WalletApi.Domain.Entity;

public class User
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Wallet Wallet { get; set; } = null!;
}