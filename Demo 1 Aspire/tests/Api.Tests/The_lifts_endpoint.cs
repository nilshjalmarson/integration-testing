using System.Net;
using System.Text.Json;

namespace Api.Tests.Tests;

public class The_lifts_endpoint(AspireFixture fixture) : IClassFixture<AspireFixture>
{
    [Fact]
    public async Task requests_return_ok()
    {
        // Act
        var cancellationToken = TestContext.Current.CancellationToken;
        var httpClient = fixture.App.CreateHttpClient("Api", "https");
        var token = JwtTokenFactory.CreateToken("testuser");
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        var response = await httpClient.GetAsync("/lifts", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(content);
        var lifts = doc.RootElement.EnumerateArray().ToList();
        Assert.NotEmpty(lifts);
    }

    [Fact]
    public async Task requests_without_token_return_unauthorized()
    {
        // Act
        var cancellationToken = TestContext.Current.CancellationToken;
        var httpClient = fixture.App.CreateHttpClient("Api", "https");
        var response = await httpClient.GetAsync("/lifts", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
