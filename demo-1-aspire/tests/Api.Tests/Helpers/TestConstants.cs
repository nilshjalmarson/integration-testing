namespace Demo1.Api.Tests;

/// <summary>
/// Shared constants used across test infrastructure and the API.
/// </summary>
public static class TestConstants
{
    /// <summary>
    /// The JWT signing key used in tests, base64-encoded.
    /// This is the same key as the one managed by `dotnet user-jwts` for the Api project.
    /// Retrieve with: dotnet user-jwts key (run from the Api project directory)
    /// </summary>
    public const string JwtSigningKeyBase64 = "WUmVkr/IvY9nxLatUUh6e3\u002BMLdvNUOJQeYZlEItu9ao=";

    /// <summary>
    /// The JWT issuer used in tests. Must match the API configuration.
    /// </summary>
    public const string JwtIssuer = "dotnet-user-jwts";

    /// <summary>
    /// The JWT audience used in tests. Must match a valid audience in the API.
    /// </summary>
    public const string JwtAudience = "https://localhost:7220";
}
