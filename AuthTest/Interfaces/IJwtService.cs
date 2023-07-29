using AuthTest.Models;
using AuthTest.Models.Responses;

namespace AuthTest.Interfaces
{
    public interface IJwtService
    {
        Task<TokensResponse> CreateTokensAsync(User user);

        Task<TokensResponse> RefreshAsync(string token);

        Task RevokeAsync(string token);
    }
}
