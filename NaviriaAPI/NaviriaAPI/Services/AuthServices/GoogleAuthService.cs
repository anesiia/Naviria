using Google.Apis.Auth;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IAuthServices;
using NaviriaAPI.IServices.IJwtService;
using Microsoft.AspNetCore.Identity;

namespace NaviriaAPI.Services.AuthServices
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly string _googleClientId;

        public GoogleAuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _googleClientId = configuration["Authentication:Google:WebClientId"];
        }

        public async Task<string> AuthenticateAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleClientId }
            });

            var email = payload.Email;
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new ArgumentException("User with such email does not exist");
            }

            return _jwtService.GenerateUserToken(user);
        }
    }
}
