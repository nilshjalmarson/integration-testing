using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Api.Weather;

public class WeatherClient(HttpClient httpClient)
{
    private const string VemdalsskaletCurrentWeatherPath = "/v1/forecast?latitude=62.4083&longitude=13.8612&current=temperature_2m&timezone=Europe%2FStockholm";

    public async Task<double?> GetCurrentTemperatureCelsius(CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.GetAsync(VemdalsskaletCurrentWeatherPath, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            var payload = await response.Content.ReadFromJsonAsync<CurrentWeatherResponse>(cancellationToken);
            return payload?.Current?.TemperatureCelsius;
        }
        catch
        {
            return null;
        }
    }

    private sealed class CurrentWeatherResponse
    {
        [JsonPropertyName("current")]
        public CurrentWeather? Current { get; init; }
    }

    private sealed class CurrentWeather
    {
        [JsonPropertyName("temperature_2m")]
        public double TemperatureCelsius { get; init; }
    }
}
