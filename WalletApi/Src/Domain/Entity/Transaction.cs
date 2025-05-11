namespace WalletApi.Domain.Entity;

public class Transaction
{
    public long Id { get; set; }

    public long SenderId { get; set; }

    public User Sender { get; set; } = null!;

    public long ReceiverId { get; set; }

    public User Receiver { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
}