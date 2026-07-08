using FluentValidation;

namespace ClinicFlow.Application.Features.Reports.Queries.GetDailyReport;

public class GetDailyReportQueryValidator : AbstractValidator<GetDailyReportQuery>
{
    public GetDailyReportQueryValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");
    }
}
