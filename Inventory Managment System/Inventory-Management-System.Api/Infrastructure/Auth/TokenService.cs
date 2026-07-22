using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Inventory_Management_System.Api.Infrastructure.Auth
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public TokenService(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            _key = configuration["Jwt:Key"];

            _issuer = configuration["Jwt:Issuer"];

            _audience = configuration["Jwt:Audience"];

            string expirationValue = configuration["Jwt:ExpirationMinutes"] ?? "60";

            if (!int.TryParse(expirationValue, out _expirationMinutes) || _expirationMinutes <= 0)
            {
                throw new InvalidOperationException("JWT expiration must be a positive integer.");
            }

            if (Encoding.UTF8.GetByteCount(_key) < 32)
            {
                throw new InvalidOperationException("JWT key must contain at least 32 bytes.");
            }
        }

        public string GenerateToken(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            DateTime issuedAt = DateTime.UtcNow;
            DateTime expiresAt = issuedAt.AddMinutes(_expirationMinutes);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(
                    JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64
                ),
            ];

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_key));

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: issuedAt,
                expires: expiresAt,
                signingCredentials: credentials
            );

            JwtSecurityTokenHandler handler = new();

            return handler.WriteToken(token);
        }
    }
}
