using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Demo1.Api.Tests;

/// <summary>
/// Factory for creating JWT tokens in integration tests.
/// Uses the same signing key as `dotnet user-jwts` to enable realistic test token generation.
/// </summary>
public static class JwtTokenFactory
{
    /// <summary>
    /// The test signing key bytes, decoded from the base64 key managed by dotnet user-jwts.
    /// </summary>
    private static readonly byte[] TestSigningKeyBytes = Convert.FromBase64String(TestConstants.JwtSigningKeyBase64);

    /// <summary>
    /// Creates a JWT token with the specified claims.
    /// </summary>
    /// <param name="subject">The subject (username) claim.</param>
    /// <param name="roles">Optional roles to add as role claims.</param>
    /// <param name="scopes">Optional scopes to add as scope claims.</param>
    /// <param name="expiresIn">Optional token expiration duration. Defaults to 1 hour.</param>
    /// <returns>A bearer token string ready for use in Authorization header.</returns>
    public static string CreateToken(
        string subject,
        string[]? roles = null,
        string[]? scopes = null,
        TimeSpan? expiresIn = null)
    {
        var signingKey = new SymmetricSecurityKey(TestSigningKeyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var expires = now.Add(expiresIn ?? TimeSpan.FromHours(1));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, subject),
            new(ClaimTypes.Name, subject)
        };

        if (roles != null)
        {
            foreach (var role in roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }
        }

        if (scopes != null)
        {
            foreach (var scope in scopes)
            {
                claims.Add(new("scope", scope));
            }
        }

        var token = new JwtSecurityToken(
            issuer: TestConstants.JwtIssuer,
            audience: TestConstants.JwtAudience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: credentials
        );

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }

    /// <summary>
    /// Creates an expired JWT token for testing unauthorized scenarios.
    /// </summary>
    public static string CreateExpiredToken(string subject)
    {
        var signingKey = new SymmetricSecurityKey(TestSigningKeyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        // Set NotBefore to past and expires further in the past
        var notBefore = now.AddHours(-2);
        var expires = now.AddHours(-1);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, subject),
            new(ClaimTypes.Name, subject)
        };

        var token = new JwtSecurityToken(
            issuer: TestConstants.JwtIssuer,
            audience: TestConstants.JwtAudience,
            claims: claims,
            notBefore: notBefore,
            expires: expires,
            signingCredentials: credentials
        );

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}

