using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using WireMock.Client;
using WireMock.Client.Extensions;
using WireMock.Net.Testcontainers;

namespace Demo2.Api.Tests;

public class WeatherApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private WireMockContainer? _wireMockContainer;

    public IWireMockAdminApi AdminClient => _wireMockContainer!.CreateWireMockAdminClient();

    public async ValueTask InitializeAsync()
    {
        _wireMockContainer = new WireMockContainerBuilder()
            .WithCleanUp(true)
            .Build();

        await _wireMockContainer.StartAsync();

        // Wait for WireMock to be ready
        await AdminClient.WaitForHealthAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("WeatherApi:BaseUrl", _wireMockContainer!.GetPublicUrl());
    }

    public new async ValueTask DisposeAsync()
    {
        if (_wireMockContainer is not null)
            await _wireMockContainer.DisposeAsync();

        await base.DisposeAsync();
    }
}
