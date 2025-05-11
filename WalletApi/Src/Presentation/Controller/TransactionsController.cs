using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.Service;
using WalletApi.Presentation.DTO;

namespace WalletApi.Presentation.Controller;

[Authorize(Roles = "USER")]
[ApiController]
[Route("transactions")]
public class TransactionsController(TransactionService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDto dto)
    {
        await service.CreateTransaction(dto);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilterDto filterDto)
    {
        return Ok(await service.Search(filterDto));
    }
}