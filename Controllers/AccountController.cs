using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web_Api.Account_Models;
using Web_Api.Jwt;

namespace Web_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IConfiguration _configuration;
		private readonly JwtModel _jwtModel;

		public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration,IOptions<JwtModel> Jwtoptions)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
			_jwtModel = Jwtoptions.Value;
		}



		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
		{
			var user = new User
			{
				UserName = registerModel.Email,
				Email = registerModel.Email
			};

			var result = await _userManager.CreateAsync(user, registerModel.Password);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			return Ok(".ثبت نام موفقیت آمیز بود");
		} 

	
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			var user = await _userManager.FindByEmailAsync(loginModel.Email);
			if (user == null)
			{
				return Unauthorized("!نام کاربری یا رمز عبور اشتباه است");
			}

			var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
			if (!result.Succeeded)
			{
				return Unauthorized("!نام کاربری یا رمز عبور اشتباه است");
			}

			var token = GenerateJwtToken(user);
			return Ok(new { token });
		}

		private string GenerateJwtToken(User user)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var claims = new[]
			{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtModel.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

			var token = new JwtSecurityToken
			(
				issuer: _jwtModel.Issuer,
				audience: _jwtModel.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtModel.ExpirationInMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);

		} 
	}
}
			
