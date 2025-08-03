
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Commands;

public class CreateCustomerCommand : IRequest<Result<short>>
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Result<short>>
{
    private readonly ApplicationDbContext _context;

    public CreateCustomerHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<short>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var duplicate = await _context.Customers
            .AnyAsync(c => c.PhoneNumber == request.PhoneNumber || c.Email == request.Email, cancellationToken);

        if (duplicate)
            return Result<short>.Failure("A customer with this email or phone number already exists.");

        var customer = new Customer
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<short>.Success(customer.Id);
    }
}
