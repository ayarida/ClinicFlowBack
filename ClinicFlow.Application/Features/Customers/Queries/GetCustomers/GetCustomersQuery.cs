using ClinicFlow.Application.Features.Customers.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Queries.GetCustomers;

public record GetCustomersQuery(string? SearchTerm) : IRequest<IReadOnlyList<CustomerSummaryDto>>;
