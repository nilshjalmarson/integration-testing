namespace Demo1.Api.Tests;

public class Backup_The_lifts_endpoint(AspireFixture fixture) : IClassFixture<AspireFixture>
{
    [Fact]
    public async Task returns_unauthorized_if_no_bearer_token_is_provided()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        using var httpClient = fixture.App.CreateHttpClient("Api", "https");
        using var response = await httpClient.GetAsync("/lifts", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task returns_ok_if_a_valid_bearer_token_is_provided()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        using var httpClient = fixture.App.CreateHttpClient("Api", "https");
        var token = JwtTokenFactory.CreateToken("testuser");
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        
        using var response = await httpClient.GetAsync("/lifts", cancellationToken);

        // Assert        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task returns_unauthorized_if_an_expired_bearer_token_is_provided()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        using var httpClient = fixture.App.CreateHttpClient("Api", "https");
        var expiredToken = JwtTokenFactory.CreateExpiredToken("testuser");
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", expiredToken);
        
        using var response = await httpClient.GetAsync("/lifts", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
