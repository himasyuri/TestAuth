using AuthTest.Interfaces;
using AuthTest.Models;
using AuthTest.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthTest.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IBaseRepository<User> _userRepository;

        public UserAuthService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> LoginAsync(LoginRequest request)
        {
            User? user = await _userRepository.GetAll()
                        .FirstOrDefaultAsync(p => p.Email == request.Login);
            if (user is null)
            {
                throw new Exception("User with this email is not registered");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Invalid password");
            }


            return user;
        }

        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.GetAll().AnyAsync(p => p.Email == request.Email))
            {
                throw new Exception("This email is already registered");
            }

            if (await _userRepository.GetAll().AnyAsync(p => p.Phone == request.Phone))
            {
                throw new Exception("This phone is already registered");
            }

            CreatePasswordHash(request.Password,
                              out byte[] passwordHash,
                              out byte[] passwordSalt);

            User user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.CreateAsync(user);
            
            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
