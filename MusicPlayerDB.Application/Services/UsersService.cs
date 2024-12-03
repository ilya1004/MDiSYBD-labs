using MusicPlayerDB.Application.Interfaces;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class UsersService
{
    private readonly UsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UsersService(UsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    public async Task<ResponseData<int>> RegisterUserAsync(UserCredentialsDTO userCredentialsDTO)
    {
        var hashedPassword = _passwordHasher.GenerateHash(userCredentialsDTO.Password);

        var existingUserResponse = await _usersRepository.GetUserByLogin(userCredentialsDTO.Login);
        
        if (existingUserResponse.Successfull)
        {
            return ResponseData<int>.Error("User with this login already exists.");
        }

        var user = new User
        {
            Id = 0,
            Login = userCredentialsDTO.Login,
            PasswordHash = hashedPassword,
            RoleId = 1,
            IsBlocked = false,
        };

        var response = await _usersRepository.CreateUserAsync(user);

        return response;
    }

    /// <summary>
    /// Аутентификация пользователя по логину и паролю.
    /// </summary>
    public async Task<ResponseData<(string, int)>> AuthenticateUserAsync(UserCredentialsDTO userCredentialsDTO)
    {
        var existingUserResponse = await _usersRepository.GetUserByLogin(userCredentialsDTO.Login);

        if (!existingUserResponse.Successfull) 
        {
            return ResponseData<(string, int)>.Error("Invalid credentials");
        }

        var result = _passwordHasher.VerifyPassword(userCredentialsDTO.Password, existingUserResponse.Data!.PasswordHash);

        if (result == false)
        {
            return ResponseData<(string, int)>.Error("Invalid credentials");
        }

        var token = _jwtProvider.GenerateToken(existingUserResponse.Data.Id, 1);

        return ResponseData<(string, int)>.Success(new (token, existingUserResponse.Data.RoleId));
    }

    /// <summary>
    /// Получение информации о пользователе.
    /// </summary>
    public async Task<ResponseData<User>> GetUserInfo(int userId)
    {
        return await _usersRepository.GetUserById(userId);
    }

    /// <summary>
    /// Получение информации о пользователе.
    /// </summary>
    public async Task<ResponseData<UserDetailDTO>> GetUserDetails(int userId)
    {
        return await _usersRepository.GetUserDetailAsync(userId);
    }

    /// <summary>
    /// Обновление данных о всех пользователях.
    /// </summary>
    public async Task<ResponseData<List<User>>> GetAllUsers()
    {
        return await _usersRepository.GetAllUsers();
    }

    /// <summary>
    /// Обновление данных пользователя.
    /// </summary>
    public async Task<ResponseData<int>> UpdateUser(int userId, User updatedUser)
    {
        var existingUserResponse = await _usersRepository.GetUserById(userId);
        if (!existingUserResponse.Successfull)
        {
            return ResponseData<int>.Error("User not found.");
        }

        return await _usersRepository.UpdateUserAsync(userId, updatedUser);
    }

    /// <summary>
    /// Удаление пользователя.
    /// </summary>
    public async Task<ResponseData<int>> DeleteUserAsync(int userId)
    {
        var existingUserResponse = await _usersRepository.GetUserById(userId);
        if (!existingUserResponse.Successfull)
        {
            return ResponseData<int>.Error("User not found.");
        }

        return await _usersRepository.DeleteUserAsync(userId);
    }
}

