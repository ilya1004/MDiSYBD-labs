using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class UsersRepository
{
    private readonly MusicPlayerDbContext _context;

    public UsersRepository(MusicPlayerDbContext context)
    {
        _context = context;
    }

    private async Task<ResponseData<T>> ExecuteWithErrorHandling<T>(Func<Task<T>> func)
    {
        try
        {
            return ResponseData<T>.Success(await func());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
            return ResponseData<T>.Error(ex.Message);
        }
    }

    /// <summary>
    /// Создание нового пользователя.
    /// </summary>
    public Task<ResponseData<int>> CreateUserAsync(User user)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                INSERT INTO users (login, password_hash, role_id, is_blocked)
                VALUES ({0}, {1}, {2}, {3})",
                user.Login, user.PasswordHash, user.RoleId, user.IsBlocked);

            var rowsAffected = await _context.Database.ExecuteSqlAsync(query);
            return rowsAffected;
        });
    }

    /// <summary>
    /// Получение пользователя по ID.
    /// </summary>
    public Task<ResponseData<User>> GetUserById(int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT id, login, password_hash, is_blocked, role_id, user_info_id, artist_info_id
                FROM users
                WHERE id = {0}", userId);

            var data = await _context.Database.SqlQuery<User>(query).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"User with ID {userId} not found");
        });
    }

    /// <summary>
    /// Получение пользователя по логину.
    /// </summary>
    public Task<ResponseData<User>> GetUserByLogin(string login)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT id, login, password_hash, is_blocked, role_id, user_info_id, artist_info_id
                FROM users
                WHERE login = {0}", login);

            var data = await _context.Database.SqlQuery<User>(query).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"User with login {login} not found");
        });
    }

    /// <summary>
    /// Получение всех пользователей.
    /// </summary>
    public Task<ResponseData<List<User>>> GetAllUsers()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT id, login, password_hash, is_blocked, role_id, user_info_id, artist_info_id
                FROM users
                ORDER BY id");

            var data = await _context.Database.SqlQuery<User>(query).AsNoTracking().ToListAsync();

            return data;
        });
    }

    /// <summary>
    /// Обновление информации о пользователе.
    /// </summary>
    public Task<ResponseData<int>> UpdateUserAsync(int userId, User updatedUser)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                UPDATE users
                SET login = {0}, password_hash = {1}, role_id = {2}, user_info_id = {3}, artist_info_id = {4}, is_blocked = {5}
                WHERE id = {6}",
                updatedUser.Login, updatedUser.PasswordHash, updatedUser.RoleId, updatedUser.UserInfoId, updatedUser.ArtistInfoId, updatedUser.IsBlocked, userId);

            var rowsAffected = await _context.Database.ExecuteSqlAsync(query);
            return rowsAffected;
        });
    }

    /// <summary>
    /// Удаление пользователя по ID.
    /// </summary>
    public Task<ResponseData<int>> DeleteUserAsync(int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                DELETE FROM users WHERE id = {0}", userId);

            var rowsAffected = await _context.Database.ExecuteSqlAsync(query);
            if (rowsAffected == 0)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            return rowsAffected;
        });
    }

    /// <summary>
    /// Получение информации о пользователе (включая UserInfo).
    /// </summary>
    public Task<ResponseData<UserDetailDTO>> GetUserDetailAsync(int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT u.id, u.login, u.password_hash, u.is_blocked, u.role_id, u.user_info_id, u.artist_info_id, ui.nickname, ui.about
                FROM users AS u
                INNER JOIN user_info AS ui ON u.user_info_id = ui.id
                WHERE u.id = {0}", userId);

            var userDetail = await _context.Database.SqlQuery<UserDetailDTO>(query)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return userDetail ?? throw new KeyNotFoundException($"User with ID {userId} not found");
        });
    }

    ///// <summary>
    ///// Аутентификация пользователя по логину и паролю.
    ///// </summary>
    //public Task<ResponseData<User>> AuthenticateUserAsync(string login, string passwordHash)
    //{
    //    return ExecuteWithErrorHandling(async () =>
    //    {
    //        var query = FormattableStringFactory.Create(@"
    //            SELECT u.id, u.login, u.password_hash, u.is_blocked, r.id role_id, u.user_info_id, u.artist_info_id
    //            FROM users u
    //            INNER JOIN roles r ON u.role_id = r.id
    //            WHERE u.login = {0} AND u.password_hash = {1}", login, passwordHash);

    //        var user = await _context.Database.SqlQuery<User>(query)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync();

    //        if (user == null)
    //            throw new UnauthorizedAccessException("Invalid login or password");

    //        if (user.IsBlocked)
    //            throw new UnauthorizedAccessException("User is blocked");

    //        return user;
    //    });
    //}
}

