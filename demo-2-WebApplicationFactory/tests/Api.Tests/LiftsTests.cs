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
       
    }

    [Fact]
    public async Task lift_statuses_are_unchanged_when_vemdalsskalet_temperature_is_minus_nineteen_or_warmer()
    {
       
    }
}
