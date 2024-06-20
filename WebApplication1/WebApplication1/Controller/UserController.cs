using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Services;

namespace WebApplication1.Controller;

public class UserController:ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO register)
    {
        if (!await _userService.CreateRegister(register))
        {
            return BadRequest("client already exists");
        }

        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginDTO login)
    {
        if (!await _userService.LoginUser(login))
        {
            return BadRequest("login failed");
        }

        return NoContent();
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
    {
        try
        {
            var refresh = await _userService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);
            return Ok(refresh);
        }
        catch (Exception e)
        {
            return BadRequest("error");
        }
    }
    
    
    
    
}