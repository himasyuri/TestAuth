using AuthTest.Models;
using AuthTest.Models.Requests;

namespace AuthTest.Interfaces
{
    public interface IUserAuthService
    {
        Task<User> RegisterAsync(RegisterRequest request);

        Task<User> LoginAsync(LoginRequest request);
    }
}
