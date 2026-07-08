using ClinicFlow.Application.Features.Reports.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Reports.Queries.GetDailyReport;

public record GetDailyReportQuery(DateOnly Date) : IRequest<DailyReportDto>;
