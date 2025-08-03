

using Application.Auth.DTOs;
using Domain.Entities.Auth;
using Infrastructure.Auth;
using Infrastructure.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands;
public record RegisterResponse(bool Success, string? ErrorMessage, UserDto? User);

public record RegisterCommand : IRequest<RegisterResponse>
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
    public string RoleName { get; set; } = "User";
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly AuthTokenService _tokenService;

    public RegisterCommandHandler(ApplicationDbContext context, AuthTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == request.Email, ct))
        {
            return new RegisterResponse(false, "Username already taken", null);
        }

        // Password confirmation check
        if (request.Password != request.ConfirmPassword)
        {
            return new RegisterResponse(false, "Passwords do not match", null);
        }

        // Find role
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName, ct);
        if (role == null)
        {
            return new RegisterResponse(false, "Invalid role specified", null);
        }

        // Create user
        var user = new User
        {
            FullName = request.FullName,
            UserName = "",
            Email = request.Email,
            PasswordHash = PasswordManager.HashPassword(request.Password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);

        var token = _tokenService.GenerateToken(user);

        return new RegisterResponse(true, null, new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = role.Name
        });
    }
}
