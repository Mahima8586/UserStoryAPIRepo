using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("Login failed: Missing username or password.");
                return BadRequest(new
                {
                    Message = "Username and password are required."
                });
            }

            if (request.Username == "admin" && request.Password == "password")
            {
                var token = GenerateJwtToken();
                _logger.LogInformation("User '{Username}' logged in successfully.", request.Username);

                return Ok(new { token });
            }

            _logger.LogWarning("Unauthorized login attempt for user '{Username}'", request.Username);
            return Unauthorized(new
            {
                Message = "Invalid username or password. Please try again or contact support if the issue persists."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for user '{Username}'", request?.Username);

            return StatusCode(500, new
            {
                Message = "An unexpected error occurred while processing your login. Please try again later.",
                Help = "If this issue persists, contact support with the timestamp and your username."
            });
        }
    }

    private string GenerateJwtToken()
    {
        try
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "admin"),
            };

            var key = Encoding.UTF8.GetBytes("SuperSecretKeyThatIsAtLeast32BytesLongToMeetTheRequirement!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "BookSearchAPI",
                Audience = "BookSearchClient",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating JWT token.");
            throw new InvalidOperationException("An internal error occurred while creating your session token. Please try again.");
        }
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
