using Application.Features.CompanyInfoes.Queries;
using MediatR;

namespace BongoEcom.Services;

public class CompanyInfoService(IMediator mediator)
{
    private IMediator _mediator { get; } = mediator;
    private CompanyInfoDto? _companyInfo;
    public CompanyInfoDto? CompanyInfo => _companyInfo;

    public async Task Load()
    {
        if (_companyInfo == null) 
        {
            var query = new GetCompanyInfoQuery();
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
            {
                _companyInfo = response.Data ?? new();
            }
        }    
    }
}
