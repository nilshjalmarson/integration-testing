using System.Text.Json;
using WireMock.Admin.Mappings;
using WireMock.Client.Extensions;

namespace Demo2.Api.Tests;

public class LiftsTests(WeatherApiFactory factory) : IClassFixture<WeatherApiFactory>
{
    private const string CurrentWeatherPath = "/v1/forecast";

    [Fact]
    public async Task all_lifts_are_closed_when_vemdalsskalet_temperature_is_below_minus_nineteen_degrees()
    {
        // Arrange
        var adminClient = factory.AdminClient;
        await adminClient.DeleteMappingsAsync(TestContext.Current.CancellationToken);

        var mappingBuilder = adminClient.GetMappingBuilder();
        mappingBuilder.Given(m => m
            .WithRequest(req => req
                .WithMethods("GET")
                .WithPath(CurrentWeatherPath))
            .WithResponse(res => res
                .WithStatusCode(200)
                .WithBodyAsJson(new
                {
                    current = new
                    {
                        temperature_2m = -20.4
                    }
                })));

        await mappingBuilder.BuildAndPostAsync(TestContext.Current.CancellationToken);

        // Act
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/lifts", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        using var doc = JsonDocument.Parse(body);
        var statuses = doc.RootElement
            .EnumerateArray()
            .Select(lift => lift.GetProperty("status").GetString())
            .ToArray();

        Assert.All(statuses, status => Assert.Equal("closed", status, true));
    }

    [Fact]
    public async Task lift_statuses_are_unchanged_when_vemdalsskalet_temperature_is_minus_nineteen_or_warmer()
    {

    }
}
