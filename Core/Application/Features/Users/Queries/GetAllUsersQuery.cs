

using Application.Auth.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries;

public record GetAllUsersQuery() : IRequest<Result<List<UserDto>>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetAllUsersQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            var categories = await context.Users
                .Include(r=> r.Role)
                .Select(c => new UserDto
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    FullName = c.FullName,
                    Email = c.Email ?? "",
                    Role = c.Role.Name ?? ""
                })
                .ToListAsync(cancellationToken);

            return Result<List<UserDto>>.Success(categories);
        }
        catch (Exception ex)
        {
            return Result<List<UserDto>>.Failure("Something went wrong");
        }
    }
}
