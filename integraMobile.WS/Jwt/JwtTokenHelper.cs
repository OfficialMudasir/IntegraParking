using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace integraMobile.WS.Jwt
{
    public static class JwtTokenHelper
    {
		public static string GenerateToken(string user)
		{
			var mySecret = ConfigurationManager.AppSettings["JwtSecretKey"];
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			var myIssuer = ConfigurationManager.AppSettings["JwtMyIssuer"];
			var myAudience = ConfigurationManager.AppSettings["JwtMyAudience"];

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim("u", user),
				}),
				Expires = DateTime.UtcNow.AddMinutes(20),
				Issuer = myIssuer,
				Audience = myAudience,
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public static bool ValidateToken(string authToken, string u)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var result = false;
			SecurityToken validatedToken;
			try 
			{
				ClaimsPrincipal claims = tokenHandler.ValidateToken(authToken, new TokenValidationParameters()
				{
					ValidateLifetime = true, // Because there is no expiration in the generated token
					ValidateAudience = true, // Because there is no audiance in the generated token
					ValidateIssuer = true,   // Because there is no issuer in the generated token
					ValidIssuer = ConfigurationManager.AppSettings["JwtMyIssuer"],
					ValidAudience = ConfigurationManager.AppSettings["JwtMyAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSecretKey"])) // The same key as the one that generate the token
				}, out validatedToken);

				result = claims.Claims.Any(claim => claim.Value.Equals(u, StringComparison.OrdinalIgnoreCase));

			} catch// (Exception ex) 
			{
				return false;
			}
			
			return result;
		}
	}
}