

namespace Domain.Common;

public interface IImageOwner
{
    int OwnerId { get; }
    byte OwnerType { get; }
}