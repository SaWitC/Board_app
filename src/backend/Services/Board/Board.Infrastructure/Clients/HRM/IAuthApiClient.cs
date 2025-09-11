using Board.Domain.Contracts.Models.HRM;
using Refit;

namespace Board.Infrastructure.Clients.HRM;

public interface IAuthApiClient
{
    [Post("/auth/realms/innowise-group/protocol/openid-connect/token")]
    Task<KeyCloakTokenModel> GetTokenAsync([Body(BodySerializationMethod.UrlEncoded)] KeyCloakTokenRequestParameters request);
}
