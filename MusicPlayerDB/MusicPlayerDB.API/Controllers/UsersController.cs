using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using System.Security.Claims;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserCredentialsDTO userCredentialsDTO)
    {
        if (userCredentialsDTO == null)
        {
            return BadRequest("Invalid user data.");
        }

        var result = await _usersService.RegisterUserAsync(userCredentialsDTO);
        if (result.Successfull)
        {
            return Ok(new { UserId = result.Data });
        }
        return Conflict(new { Message = result.ErrorMessage });
    }

    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    [HttpPost("login")]
    public async Task<IResult> LoginUser([FromBody] UserCredentialsDTO userCredentialsDTO)
    {
        if (string.IsNullOrEmpty(userCredentialsDTO.Login) || string.IsNullOrEmpty(userCredentialsDTO.Password))
        {
            return Results.BadRequest("Login and password must be provided.");
        }

        var response = await _usersService.AuthenticateUserAsync(userCredentialsDTO);

        if (response.Successfull)
        {
            Response.Cookies.Append("my-cookie", response.Data.Item1, new CookieOptions { SameSite = SameSiteMode.Lax});

            return Results.Ok(new {roleId = response.Data.Item2});
        }

        return Results.Unauthorized();
    }

    /// <summary>
    /// Получение информации о пользователе.
    /// </summary>
    [HttpGet("current-user")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("Invalid token: User ID is missing.");
        }

        var result = await _usersService.GetUserInfo(userId);
        if (result.Successfull)
        {
            return Ok(result.Data);
        }

        return NotFound(new { Message = result.ErrorMessage });
    }

    /// <summary>
    /// Получение полной информации о пользователе.
    /// </summary>
    [HttpGet("current-user-full-info")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetCurrentUserFullInfo()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("Invalid token: User ID is missing.");
        }

        var result = await _usersService.GetUserDetails(userId);
        if (result.Successfull)
        {
            return Ok(result.Data);
        }

        return NotFound(new { Message = result.ErrorMessage });
    }

    /// <summary>
    /// Получение информации о пользователе.
    /// </summary>
    [HttpGet("check")]
    public Task<IActionResult> CheckUserRole()
    {
        var userRoleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);

        if (userRoleClaim == null || !int.TryParse(userRoleClaim.Value, out int userRole))
        {
            return Task.FromResult<IActionResult>(Unauthorized("Invalid token: User Role is missing."));
        }

        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Task.FromResult<IActionResult>(Unauthorized("Invalid token: User ID is missing."));
        }

        return Task.FromResult<IActionResult>(Ok(new { isAuthenticated = true, id = userId, role = userRole }));

    }

    /// <summary>
    /// Получение информации о пользователе.
    /// </summary>
    [Authorize]
    [HttpGet("{userId}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> GetUserInfo(int userId)
    {
        var result = await _usersService.GetUserInfo(userId);
        if (result.Successfull)
        {
            return Ok(result.Data);
        }
        return NotFound(new { Message = result.ErrorMessage });
    }

    /// <summary>
    /// Получение полной информации о пользователе.
    /// </summary>
    [HttpGet("/full-info/{userId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndAdminPolicy)]
    public async Task<IActionResult> GetUserDetails(int userId)
    {
        var result = await _usersService.GetUserDetails(userId);
        if (result.Successfull)
        {
            return Ok(result.Data);
        }
        return NotFound(new { Message = result.ErrorMessage });
    }

    /// <summary>
    /// Получение информации о всех пользователях.
    /// </summary>
    [HttpGet("all-users")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _usersService.GetAllUsers();
        if (result.Successfull)
        {
            return Ok(result.Data);
        }
        return NotFound(new { Message = result.ErrorMessage });
    }

    /// <summary>
    /// Обновление данных пользователя.
    /// </summary>
    [HttpPut("{userId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndAdminPolicy)]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] User updatedUser)
    {
        if (updatedUser == null)
        {
            return BadRequest("Invalid user data.");
        }

        var result = await _usersService.UpdateUser(userId, updatedUser);
        if (result.Successfull)
        {
            return Ok(new { Message = "User updated successfully" });
        }
        return NotFound(new { Message = result.ErrorMessage });
    }

    /// <summary>
    /// Удаление пользователя.
    /// </summary>
    [HttpDelete("{userId}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> DeleteUserAsync(int userId)
    {
        var result = await _usersService.DeleteUserAsync(userId);
        if (result.Successfull)
        {
            return Ok(new { Message = "User deleted successfully" });
        }
        return NotFound(new { Message = result.ErrorMessage });
    }
}

