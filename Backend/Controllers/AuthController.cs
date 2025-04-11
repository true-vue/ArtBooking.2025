using Business.Application.UserIdentity;
using Business.Application.UserIdentity.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserIdentityService _userIdentityService;

    public AuthController(IConfiguration configuration, IUserIdentityService userIdentityService)
    {
        _configuration = configuration;
        _userIdentityService = userIdentityService;
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] LoginRequest request)
    {
        var loginResult = _userIdentityService.Login(request);
        // TODO: Add proper user authentication logic here
        // This is a simplified example - you should validate credentials against your database
        if (loginResult.Success)
        {
            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                return BadRequest("JWT key is not configured");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var expirationTime = DateTime.Now.AddHours(1);

            var claims = new[]
            {
                    new Claim(ClaimTypes.Name, request.Username),
                    // new Claim(ClaimTypes.Role, "Admin"),
                    // new Claim("CustomClaim", "UserValue")
                };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                ValidUntil = expirationTime
            });
        }

        return Unauthorized(new ProblemDetails
        {
            Title = "Login failed",
            Detail = loginResult.ErrorMessage,
            Status = StatusCodes.Status401Unauthorized
        });
    }
}
