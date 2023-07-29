using AuthCommon;
using AuthTest.Interfaces;
using AuthTest.Models;
using AuthTest.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthTest.Services
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;

        public JwtService(IOptions<AuthOptions> authOptions, 
            IBaseRepository<User> userRepository,
            IBaseRepository<RefreshToken> refreshTokenRepository)
        {
            _authOptions = authOptions;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;

        }

        public async Task<TokensResponse> CreateTokensAsync(User user)
        {
            user = await _userRepository.GetAll()
                                        .Include(x => x.RefreshTokens)
                                        .FirstOrDefaultAsync(p => p.Id == user.Id);
            if (user.RefreshTokens.Count > 200)
            {
                user.RefreshTokens.RemoveRange(0, 100);
            }

            var accessToken = createAccessToken(user);
            var refreshToken = createRefreshToken();
            user.RefreshTokens.Add(refreshToken);

            await _userRepository.UpdateAsync(user);

            return new TokensResponse 
                    {
                        RefreshToken = refreshToken.Token, 
                        AccessToken  = accessToken 
                    };
        }

        public async Task<TokensResponse> RefreshAsync(string token)
        {
            RefreshToken? checkToken = await _refreshTokenRepository.GetAll()
                                      .FirstOrDefaultAsync(p => p.Token == token);
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(p => p.Id == checkToken.UserId);

            if (checkToken.IsUsed)
            {
                user.RefreshTokens.Clear();
                await _userRepository.UpdateAsync(user);

                throw new Exception("Invalid token");
            }

            if (checkToken.IsRevoked)
            {
                user.RefreshTokens.Clear();
                await _userRepository.UpdateAsync(user);

                throw new Exception("Invalid token");
            }

            checkToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(checkToken);
            var accessToken = createAccessToken(user);
            var refreshToken = createRefreshToken();

            return new TokensResponse
            {
                RefreshToken = refreshToken.Token,
                AccessToken = accessToken
            };
        }

        public async Task RevokeAsync(string token)
        {
            var checkToken = await _refreshTokenRepository.GetAll()
                             .FirstOrDefaultAsync(p => p.Token == token);
            checkToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(checkToken);

            return;
        }

        //create refresh token
        private RefreshToken createRefreshToken()
        {

            var refreshToken = new RefreshToken
            {

            };

            refreshToken.Token = createUniqueToken(refreshToken);

            return refreshToken;
        }

        //create access tokens
        private string createAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString())
            };

            var authParams = _authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var creditionals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(authParams.AccessTokenLifeTime),
                signingCredentials: creditionals);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string createUniqueToken(RefreshToken token)
        {
            Random rnd = new Random();
            var seed = token.Id + DateTime.UtcNow.Ticks + rnd.NextInt64();

            byte[] bytes = Encoding.UTF8.GetBytes(seed.ToString());

            return Convert.ToHexString(RandomNumberGenerator.GetBytes(bytes.Length));
        }
    }
}
