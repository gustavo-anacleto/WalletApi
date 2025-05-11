using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WalletApi.Application.Exceptions;
using WalletApi.Domain.Entity;
using WalletApi.Infrastructure.Configuration;
using WalletApi.Presentation.DTO;

namespace WalletApi.Application.Service;

public class TransactionService(
    AppDbContext context,
    UserService userService,
    WalletService walletService,
    IMapper mapper)
{
    public async Task CreateTransaction(TransactionCreateDto dto)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var sender = await userService.FindTokenUser();

            await walletService.RemoveAmountFromBalance(sender, dto.Amount);

            var receiverBalanceUpdateDto = new BalanceUpdateDto(dto.ReceiverId, dto.Amount);

            await walletService.AddAmountToBalance(receiverBalanceUpdateDto);

            var transactionEntity = new Transaction
            {
                SenderId = sender.Id,
                ReceiverId = dto.ReceiverId,
                Amount = dto.Amount
            };

            context.Transactions.Add(transactionEntity);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            var innerExceptionMessage = e.InnerException?.Message ?? e.Message;
            throw new BadRequestException($"Transaction Error: {innerExceptionMessage}");
        }
    }

    public async Task<List<TransactionDto>> Search(TransactionFilterDto filterDto)
    {
        var user = await userService.FindTokenUser();

        List<Transaction> transactions;

        var transactionsQuery = context.Transactions
            .Where(t => t.SenderId == user.Id || t.ReceiverId == user.Id)
            .Include(t => t.Receiver)
            .Include(t => t.Sender)
            .OrderByDescending(t => t.Date);
        
        if (filterDto.NeedFilter)
        {
            if (filterDto.StartDate > filterDto.EndDate)
            {
                throw new BadRequestException("Start Date cannot be greater than end date");
            }

            if (filterDto.EndDate > DateTime.UtcNow)
            {
                throw new BadRequestException("End date cannot be earlier than current date");
            }

            transactions = await transactionsQuery
                .Where(t => t.Date >= filterDto.StartDate.UtcDateTime &&t.Date < filterDto.EndDate.UtcDateTime.AddDays(1))
                .ToListAsync();
        }
        else
        {
            transactions = await transactionsQuery.ToListAsync();
        }


        return mapper.Map<List<TransactionDto>>(transactions);
    }
}