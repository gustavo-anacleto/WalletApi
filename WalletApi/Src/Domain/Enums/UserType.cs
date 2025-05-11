using System.Runtime.Serialization;

namespace WalletApi.Domain.Enums;

public enum UserType
{
    [EnumMember(Value = "ADMIN")] ADMIN,

    [EnumMember(Value = "USER")] USER
}