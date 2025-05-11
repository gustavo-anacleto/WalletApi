using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.Service;
using WalletApi.Presentation.DTO;

namespace WalletApi.Presentation.Controller;

[Authorize(Roles = "USER")]
[ApiController]
[Route("wallets")]
public class WalletsController(Application.Service.WalletService walletService) : ControllerBase
{
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        return Ok(await walletService.GetBalance());
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPatch("balance")]
    public async Task<IActionResult> AddAmount([FromBody] BalanceUpdateDto dto)
    {
        await walletService.AddAmountToBalance(dto);
        return NoContent();
    }
}