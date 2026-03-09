using System.Text.Json;
using WireMock.Admin.Mappings;
using WireMock.Client.Extensions;

namespace Api.Tests;

public class FruitsTests(FruitApiFactory factory) : IClassFixture<FruitApiFactory>
{
    private static readonly object[] FruitsJson =
    [
        new { name = "Apple", family = "Rosaceae", genus = "Malus", order = "Rosales", nutritions = new { calories = 52 } },
        new { name = "Banana", family = "Musaceae", genus = "Musa", order = "Zingiberales", nutritions = new { calories = 96 } }
    ];

    [Fact]
    public async Task GetFruits_WhenDownstreamReturnsOk_ReturnsOk()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var adminClient = factory.AdminClient;

        await adminClient.DeleteMappingsAsync(cancellationToken);

        var mappingBuilder = adminClient.GetMappingBuilder();
        mappingBuilder.Given(m => m
            .WithRequest(req => req
                .WithMethods("GET")
                .WithPath("/api/fruit/all"))
            .WithResponse(res => res
                .WithStatusCode(200)
                .WithBodyAsJson(FruitsJson)));

        await mappingBuilder.BuildAndPostAsync(cancellationToken);

        // Act
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/fruits", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(body);
        Assert.Equal(2, doc.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task GetFruits_WhenDownstreamReturnsError_ReturnsError()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var adminClient = factory.AdminClient;

        await adminClient.DeleteMappingsAsync(cancellationToken);

        var mappingBuilder = adminClient.GetMappingBuilder();
        mappingBuilder.Given(m => m
            .WithRequest(req => req
                .WithMethods("GET")
                .WithPath("/api/fruit/all"))
            .WithResponse(res => res
                .WithStatusCode(500)));

        await mappingBuilder.BuildAndPostAsync(cancellationToken);

        // Act
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/fruits", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}
