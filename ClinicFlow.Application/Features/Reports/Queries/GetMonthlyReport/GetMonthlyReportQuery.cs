using ClinicFlow.Application.Features.Reports.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Reports.Queries.GetMonthlyReport;

public record GetMonthlyReportQuery(int Year, int Month) : IRequest<MonthlyReportDto>;
