using ClinicFlow.Application.Features.Customers.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;
