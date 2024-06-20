using AuthExample.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IUserService
{
    Task<bool> CreateRegister(RegisterDTO register);
    Task<bool> LoginUser(LoginDTO login);
    Task<RefreshTokenResponseDTO> RefreshTokenAsync(string refreshToken);
}

public class UserService : IUserService
{
    private readonly AddDbContext _context;
    private readonly ITokenService _tokenService;

    public UserService(ITokenService tokenService, AddDbContext context)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<bool> CreateRegister(RegisterDTO register)
    {
        var emailExists = await _context.Users.AnyAsync(e => e.Email == register.Email);
        if (emailExists)
        {
            return false;
        }

        var user = new User
        {
            Email = register.Email,
            Login = register.Login,
        };
        user.Password = new PasswordHasher<User>().HashPassword(user, register.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LoginUser(LoginDTO login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == login.Login);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);

        if (verificationResult == PasswordVerificationResult.Success)
        {
            var (token, expiration) = _tokenService.GenerateRefreshToken();
            user.RefreshToken = token;
            user.RefreshTokenExpiration = expiration;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true; 
        }

        return false; 
    }
    public async Task<RefreshTokenResponseDTO> RefreshTokenAsync(string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiration > DateTime.UtcNow);

        if (user==null)
        {
            throw new Exception("error");
        }
        var newAccToken = _tokenService.GenerateAccessToken(user);
        var (newRefToken, newRefreshTokenExpiration) = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefToken;
        user.RefreshTokenExpiration = newRefreshTokenExpiration;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new RefreshTokenResponseDTO
        {
            AccessToken = newAccToken,
            RefreshToken = newRefToken
        };
    }
}
