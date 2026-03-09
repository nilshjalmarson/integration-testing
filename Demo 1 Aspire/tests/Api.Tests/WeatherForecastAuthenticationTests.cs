namespace Api.Tests.Tests;

public class WeatherForecastAuthenticationTests(AspireFixture fixture) : IClassFixture<AspireFixture>
{
    [Fact]
    public async Task weatherforecast_request_without_a_bearer_token_returns_unauthorized()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        using var httpClient = fixture.App.CreateHttpClient("Api", "https");
        using var response = await httpClient.GetAsync("/weatherforecast", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task weatherforecast_request_with_a_valid_bearer_token_returns_ok()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        using var httpClient = fixture.App.CreateHttpClient("Api", "https");
        var token = JwtTokenFactory.CreateToken("testuser");
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        
        using var response = await httpClient.GetAsync("/weatherforecast", cancellationToken);

        // Assert
        if (response.StatusCode != HttpStatusCode.OK)
        {
            var wwwAuth = response.Headers.WwwAuthenticate.ToString();
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Xunit.Sdk.XunitException($"Expected 200 OK but got {response.StatusCode}. WWW-Authenticate: {wwwAuth}. Body: {body}");
        }
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task weatherforecast_request_with_an_expired_bearer_token_returns_unauthorized()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        using var httpClient = fixture.App.CreateHttpClient("Api", "https");
        var expiredToken = JwtTokenFactory.CreateExpiredToken("testuser");
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", expiredToken);
        
        using var response = await httpClient.GetAsync("/weatherforecast", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
