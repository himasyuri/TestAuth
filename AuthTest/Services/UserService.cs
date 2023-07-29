using AuthTest.Interfaces;
using AuthTest.Models;
using AuthTest.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace AuthTest.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;

        public UserService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> EditUserDataAsync(EditUserDataRequest request, string id)
        {
            User? user = await _userRepository.GetAll()
                               .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            if (user is null)
            {
                throw new Exception("User not found");
            }

            user.Name = request.Name;
            user.Surname = request.Surname;
            user.Email = request.Email;
            user.Phone = request.Phone;

            await _userRepository.UpdateAsync(user);
            
            return user;
        }

        public async Task<User> GetAsync(string id)
        {
            User? user = await _userRepository.GetAll()
                               .FirstOrDefaultAsync (p => p.Id == Guid.Parse(id));

            if (user is null)
            {
                throw new Exception("User not found");
            }

            return user;
        }
    }
}
