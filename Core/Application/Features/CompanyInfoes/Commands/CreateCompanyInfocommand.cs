
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CompanyInfoes.Commands;

public class CreateCompanyInfoCommand : IRequest<Result<byte>>
{
    public string CompanyName { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
    public string? Address { get; set; }
}
public class CreateCompanyInfoCommandHandler : IRequestHandler<CreateCompanyInfoCommand, Result<byte>>
{
    private readonly ApplicationDbContext _context;

    public CreateCompanyInfoCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<byte>> Handle(CreateCompanyInfoCommand request, CancellationToken cancellationToken)
    {
        var existed = await _context.CompanyInfoes.FirstOrDefaultAsync();

        if (existed is null)
        {
            var info = new CompanyInfo
            {
                CompanyName = request.CompanyName,
                LogoUrl = request.LogoUrl,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                Address = request.Address,
            };
            _context.CompanyInfoes.Add(info);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<byte>.Success(info.Id);
        }
        else
        {
            existed.CompanyName = request.CompanyName;
            existed.LogoUrl = request.LogoUrl;
            existed.Email = request.Email;
            existed.MobileNumber= request.MobileNumber;
            existed.Address = request.Address;

            _context.CompanyInfoes.Update(existed);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<byte>.Success(existed.Id);
        }
        
    }
}
