
namespace Application.Features.Settings;

public record UpdateSettingsValueCommand(string Key, string Value) : IRequest<Result<short>>;

public class UpdateSettingsValueCommandHandler : IRequestHandler<UpdateSettingsValueCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public UpdateSettingsValueCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(UpdateSettingsValueCommand request, CancellationToken cancellationToken)
    {
        var setting = _context.Settings.FirstOrDefault(setting => setting.Key == request.Key);
        if (setting != null)
        {
            setting.Value = request.Value;
            _context.Settings.Update(setting);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<short>.Success(0);
        }
        return Result<short>.Failure("Something went wrong");
    }
}