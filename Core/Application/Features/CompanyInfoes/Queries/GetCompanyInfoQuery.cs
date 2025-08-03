

using Microsoft.EntityFrameworkCore;

namespace Application.Features.CompanyInfoes.Queries;

public class CompanyInfoDto
{
    public byte Id { get; set; }
    public string CompanyName { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
    public string? Address { get; set; }
}

public record GetCompanyInfoQuery : IRequest<Result<CompanyInfoDto>>;

public class GetCompanyInfoQueryHandler : IRequestHandler<GetCompanyInfoQuery, Result<CompanyInfoDto>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetCompanyInfoQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Result<CompanyInfoDto>> Handle(GetCompanyInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();

            var company = await context.CompanyInfoes
                .Select(c => new CompanyInfoDto
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    LogoUrl = c.LogoUrl ?? "",
                    Email = c.Email ?? "",
                    MobileNumber = c.MobileNumber ?? "",
                    Address = c.Address ?? "",
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (company is null) 
            {
                return Result<CompanyInfoDto>.Failure("Category not found");
            }

            return Result<CompanyInfoDto>.Success(company);
        }
        catch (Exception ex)
        {
            return Result<CompanyInfoDto>.Failure("Category not found");
        }
    }
}
