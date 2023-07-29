using AuthTest.Models;
using AuthTest.Models.Requests;

namespace AuthTest.Interfaces
{
    public interface IUserService
    {
        Task<User> GetAsync(string id);

        Task<User> EditUserDataAsync(EditUserDataRequest request, string id);
    }
}
