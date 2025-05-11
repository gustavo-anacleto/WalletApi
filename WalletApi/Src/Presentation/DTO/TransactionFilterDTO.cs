using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WalletApi.Presentation.DTO;

public class TransactionFilterDto
{
    public bool NeedFilter { get; set; } = false;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}