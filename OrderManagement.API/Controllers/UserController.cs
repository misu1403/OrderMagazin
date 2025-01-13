using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application;
using OrderManagement.Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var userId = await _userService.CreateUserAsync(dto);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        // Adaugă logică pentru a returna un utilizator după ID
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        // Adaugă logică pentru a returna toți utilizatorii
        var users = _userService.GetAllUsers();
        return Ok(users);
    }
}
