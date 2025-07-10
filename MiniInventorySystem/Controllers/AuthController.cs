
using Microsoft.AspNetCore.Mvc;
using MiniInventorySystem.DTO;
using MiniInventorySystem.Models;
using MiniInventorySystem.Services;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;
    private readonly JwtService _jwtService;
    public AuthController(AuthService service, JwtService jwtService)
    {
        _service = service;
        _jwtService = jwtService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] LoginDto request)
    {
        var exists = await _service.GetUserByName(request.Username);
        if (exists != null)
            return BadRequest("User already exists");

        var newUser = new Users
        {
            Username = request.Username,
            PasswordHash = request.Password
        };

        await _service.RegisterUser(request);
        return Ok("User registered.");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var exists = await _service.GetUserByName(request.Username);
        if (exists is null)
            return BadRequest("User not found!");

        var valid = await _service.IsValidUserAsync(request);
        if (!valid)
            return BadRequest("User name or password is incorrect!");

        var token = _jwtService.GenerateToken(request.Username);
        return Ok(new { token });
    }
}


