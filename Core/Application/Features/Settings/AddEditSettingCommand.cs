

using Domain.Entities.Site;

namespace Application.Features.Settings;

public class AddEditSettingCommand : IRequest<Result<short>>
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string TValue { get; set; }
    public bool Create { get; set; } = true;
}

public class AddEditSettingCommandHandler : IRequestHandler<AddEditSettingCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public AddEditSettingCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(AddEditSettingCommand request, CancellationToken cancellationToken)
    {
        if (request.Create)
        {
            var setting = new Setting
            {
                Key = request.Key,
                Name = request.Name,
                Value = request.Value,
                TValue = request.TValue,
            };

            _context.Settings.Add(setting);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<short>.Success(1);
        }
        else
        {
            var setting = _context.Settings.FirstOrDefault(setting => setting.Key == request.Key);
            if (setting != null)
            {
                setting.Key = request.Key;
                setting.Name = request.Name;
                setting.Value = request.Value;
                setting.TValue = request.TValue;
                _context.Settings.Update(setting);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<short>.Success(1);
            }
        }
        return Result<short>.Failure("Something went wrong");

    }
}