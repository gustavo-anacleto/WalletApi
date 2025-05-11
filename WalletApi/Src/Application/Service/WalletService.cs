using Microsoft.EntityFrameworkCore;
using WalletApi.Application.Exceptions;
using WalletApi.Domain.Entity;
using WalletApi.Infrastructure.Configuration;
using WalletApi.Presentation.DTO;

namespace WalletApi.Application.Service;

public class WalletService(AppDbContext context, UserService userService)
{
    public async Task<decimal> GetBalance()
    {
        var user = await userService.FindTokenUser();

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var wallet = await context.Wallets.FirstAsync(w => w.UserId == user.Id);
        ;

        return wallet.Balance;
    }

    public async Task AddAmountToBalance(BalanceUpdateDto dto)
    {
        if (dto.Amount <= 0)
        {
            throw new BadRequestException("Amount must be greater than 0");
        }

        var user = await context.Users.FirstAsync(u => u.Id == dto.UserId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var wallet = await context.Wallets.FirstAsync(w => w.UserId == user.Id);

        wallet.Balance += dto.Amount;

        await context.SaveChangesAsync();
    }


    public async Task RemoveAmountFromBalance(User user, decimal amount)
    {
        if (amount <= 0)
        {
            throw new BadRequestException("Amount must be greater than 0");
        }

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var wallet = await context.Wallets.FirstAsync(w => w.UserId == user.Id);

        if (wallet.Balance < amount)
        {
            throw new BadRequestException($"Insufficient balance: {wallet.Balance}");
        }

        wallet.Balance -= amount;

        await context.SaveChangesAsync();
    }
}