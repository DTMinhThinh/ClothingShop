using ClothingStore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ClothingStore.Infrastructure.Repositories
{
	public class TokenService(IConfiguration config) : ITokenService
	{
		string ITokenService.CreateToken(User user)
		{
			var token = config["TokenKey"] ?? throw new Exception("TokenKey is not configured");
			if (token.Length < 64) throw new Exception("Token length is less than 64 characters");

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));

			var claims = new List<Claim>
			{
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			};

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds,
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(securityToken);
		}
	}
}
