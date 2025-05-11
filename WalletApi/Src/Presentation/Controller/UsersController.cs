using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.Service;
using WalletApi.Presentation.DTO;

namespace WalletApi.Presentation.Controller;

[ApiController]
[Route("users")]
public class UsersController(UserService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        return Ok(await service.Longin(dto));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserCreateDto dto)
    {
        await service.CreateUser(dto);
        return Created();
    }
}