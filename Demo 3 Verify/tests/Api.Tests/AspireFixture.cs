using Aspire.Hosting;
using Aspire.Hosting.Testing;

namespace Demo3.Api.Tests;

public sealed class AspireFixture : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    private DistributedApplication? _app;

    public HttpClient OldApiClient => _app?.CreateHttpClient("old", "https")
        ?? throw new InvalidOperationException("App has not been initialized.");

    public HttpClient NewApiClient => _app?.CreateHttpClient("new", "https")
        ?? throw new InvalidOperationException("App has not been initialized.");

    public async ValueTask InitializeAsync()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(cancellationToken);

        _app = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await _app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("old", cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("new", cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }
}