
using Application.Auth.DTOs;
using Infrastructure.Auth;
using Infrastructure.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands;

public record LoginResponse(bool Success, string? ErrorMessage, UserDto? User);
public record LoginCommand : IRequest<LoginResponse>
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly AuthTokenService _tokenService;

    public LoginCommandHandler(ApplicationDbContext context, AuthTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _context.Users
            .Where(x => x.UserName == request.UserName || x.Email == request.UserName)
            .Include(x => x.Role)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new LoginResponse(false, "User not found", null);
        }

        if (!PasswordManager.CheckPassword(request.Password, user.PasswordHash))
        {
            return new LoginResponse(false, "Invalid password", null);
        }

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse(true, "", new UserDto
        {
            Id = user.Id,
            UserName = request.UserName,
            Role = user.Role.Name,
        });
    }
}