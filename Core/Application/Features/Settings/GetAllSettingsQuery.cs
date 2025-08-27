
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Settings;

public record GetAllSettingsQuery() : IRequest<Result<List<SettingDto>>>;

public class GetAllSettingsQueryHandler : IRequestHandler<GetAllSettingsQuery, Result<List<SettingDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetAllSettingsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<SettingDto>>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _context.Settings.ToListAsync();
        var results = new List<SettingDto>();
        foreach (var setting in settings)
        {
            var settingDto = new SettingDto();
            settingDto.Key = setting.Key;
            settingDto.Name = setting.Name;
            settingDto.Value = setting.Value;
            settingDto.TValue = setting.TValue;

            results.Add(settingDto);
        }
        return Result<List<SettingDto>>.Success(results);
    }
}