using Board.Domain.Contracts.Models.HRM;
using Refit;

namespace Board.Infrastructure.Clients.HRM;

public interface IEmployeeApiClient
{
    [Get("/api/employee-management/api/v2/employees/{id}?loadDependency=true")]
    Task<EmployeeModel> GetEmployeeAsync(int id);

    [Post("/api/employee-management/api/v2/employees/search/names")]
    Task<EmployeeSearchResult> GetEmployeesAsync(EmployeeSearchRequest searchRequest);
}
