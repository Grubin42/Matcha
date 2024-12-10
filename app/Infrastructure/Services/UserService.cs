using System.Data;
using MySql.Data.MySqlClient;
using MyApp.Models;
using Microsoft.Extensions.Configuration;

namespace MyApp.Services;

public class UserService
{
    private readonly string _connectionString;

    public UserService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = new List<User>();

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new MySqlCommand("SELECT id, username, email, created_at FROM users", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32("id"),
                Username = reader.GetString("username"),
                Email = reader.GetString("email"),
                CreatedAt = reader.GetDateTime("created_at")
            });
        }

        return users;
    }
}