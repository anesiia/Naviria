using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.Auth;
using NaviriaAPI.IServices.IAuthServices;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for authentication and authorization.
    /// Provides endpoints for user login and Google OAuth authentication.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">Service for basic authentication operations.</param>
        /// <param name="googleAuthService">Service for Google OAuth authentication.</param>
        /// <param name="logger">Logger instance.</param>
        public AuthController(
            IAuthService authService,
            IGoogleAuthService googleAuthService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _googleAuthService = googleAuthService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="dto">The user's login credentials.</param>
        /// <returns>
        /// 200: Returns a JWT token for the authenticated user.<br/>
        /// 400: If the input model is invalid.<br/>
        /// 401: If authentication fails (invalid email or password).<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.AuthenticateAsync(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                return Unauthorized(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates a user via Google OAuth and returns a JWT token.
        /// </summary>
        /// <param name="dto">The Google authentication DTO containing the Google token.</param>
        /// <returns>
        /// 200: Returns a JWT token for the authenticated user.<br/>
        /// 400: If the input model is invalid.<br/>
        /// 401: If Google authentication fails.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _googleAuthService.AuthenticateAsync(dto.Token);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google login failed");
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
