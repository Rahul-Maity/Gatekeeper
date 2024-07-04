using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Gatekeeper;



public class Database
{

    public static string userHash(string username) => Convert.ToBase64String(MD5.HashData(Encoding.UTF8.GetBytes(username)));

    public async Task<User?>GetUserAsync(string username)
    {
        var hash = userHash(username);
        if(!File.Exists(hash))
        {
            return null;
        }
        await using var reader = File.OpenRead(hash);
        return await JsonSerializer.DeserializeAsync<User?>(reader);
    }

    public async Task Putasync(User user)
    {
        var hash = userHash(user.Username);
        await using var writer = File.OpenWrite(hash);
        await JsonSerializer.SerializeAsync(writer, user);
    }
}

public class User
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public List<UserClaim>? Claims {  get; set; }
}

public class UserClaim
{
    public string Type { get; set; }
    public string Value { get; set; }
}