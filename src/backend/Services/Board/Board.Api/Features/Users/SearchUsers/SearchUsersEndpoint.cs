using Board.Domain.Contracts.Models.HRM;
using Board.Domain.Contracts.Users;
using Board.Infrastructure.Clients.HRM;
using FastEndpoints;

namespace Board.Api.Features.Users.SearchUsers;

public class SearchUsersEndpoint : Endpoint<SearchUserRequest>
{
    private readonly IEmployeeApiClient employeeApiClient;
    public SearchUsersEndpoint(IEmployeeApiClient employeeApiClient)
    {
        this.employeeApiClient = employeeApiClient;
    }
    public override void Configure()
    {
        Get("/api/users");
    }

    public override async Task HandleAsync(SearchUserRequest request, CancellationToken cancellationToken)
    {
        EmployeeSearchResult data = await employeeApiClient.GetEmployeesAsync(new EmployeeSearchRequest() { NameOrSurname = request.SearchTerm });

        IEnumerable<FoundUserDTO> response = data.Content.Select(MapToDTO);

        await Send.OkAsync(response, cancellationToken);
    }

    public FoundUserDTO MapToDTO(EmployeeSearchModel employee)
    {
        return new FoundUserDTO()
        {
            Id = employee.Id,
            FirstNameEn = employee.FirstNameEn,
            LastNameEn = employee.LastNameEn,
            FirstNameRu = employee.FirstNameRu,
            LastNameRu = employee.LastNameRu,
            LinkProfilePictureMini = employee.LinkProfilePictureMini
        };
    }
}
