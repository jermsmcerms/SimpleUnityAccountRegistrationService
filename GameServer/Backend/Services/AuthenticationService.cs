using Backend.Context;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Services;
public class AuthenticationService : IAuthenticationService {
    private readonly Settings _settings;
    private readonly GameDbContext _context;

    public AuthenticationService(Settings settings, GameDbContext context) {
        _settings = settings;
        _context = context;
    }
    public (bool success, string token) Login(string username, string password) {
        var user = _context.Users.SingleOrDefault(user => user.UserName == username);
        if(user == null) {
            return (false, "Invalid username");
        }

        if(user.PasswordHash != AuthenticationHelpers.ComputeHash(password, user.Salt)) {
            return (false, "Invalid password");
        }

        return (true, GenerateJwtToken(AssembleClaimsIdentity(user)));
    }

    public (bool success, string content) Register(string username, string password) {
        if(_context.Users.Any(user => user.UserName == username)) {
            return (false, "username is already taken");
        }

        var user = new User() {
            UserName = username,
            PasswordHash = password
        };
        user.ProvideSaltAndHash();

        _context.Add(user);
        _context.SaveChanges();

        return (true, "Success");
    }

    private ClaimsIdentity AssembleClaimsIdentity(User user) {
        var subject = new ClaimsIdentity(new[] {
            new Claim("id", user.Id.ToString())
        });

        return subject;
    }

    private string GenerateJwtToken(ClaimsIdentity subject) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.BearerKey);
        var tokenDescriptor = new SecurityTokenDescriptor() {
            Subject = subject,
            Expires = DateTime.Now.AddYears(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public static class AuthenticationHelpers {
    public static void ProvideSaltAndHash(this User user) {
        var salt = GenerateSalt();
        user.Salt = Convert.ToBase64String(salt);
        user.PasswordHash = ComputeHash(user.PasswordHash, user.Salt);
    }
    public static string ComputeHash(string password, string saltString) {
        var salt = Convert.FromBase64String(saltString);
        using var hashGenerator = new Rfc2898DeriveBytes(password, salt);
        hashGenerator.IterationCount = 10101;
        var bytes = hashGenerator.GetBytes(24);
        return Convert.ToBase64String(bytes);
    }

    private static byte[] GenerateSalt() {
        var rng = RandomNumberGenerator.Create();
        var salt = new byte[24];
        rng.GetBytes(salt);
        return salt;
    }
}
