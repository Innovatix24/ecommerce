

using Microsoft.EntityFrameworkCore;

namespace Application.Features.Settings;

public class SettingDto
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string TValue { get; set; }
}


public record GetSettingByKeyQuery(string Key) : IRequest<Result<SettingDto>>;

public class GetSettingByKeyQueryHandler : IRequestHandler<GetSettingByKeyQuery, Result<SettingDto>>
{
    private readonly ApplicationDbContext _context;

    public GetSettingByKeyQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SettingDto>> Handle(GetSettingByKeyQuery request, CancellationToken cancellationToken)
    {
        var setting = await _context.Settings.FirstOrDefaultAsync(setting => setting.Key == request.Key);
        if (setting != null)
        {
            var dto = new SettingDto
            {
                Key = setting.Key,
                Name = setting.Name,
                Value = setting.Value,
                TValue = setting.Value,
            };
            return Result<SettingDto>.Success(dto);
        }
        return Result<SettingDto>.Failure("Settings not found");
    }
}