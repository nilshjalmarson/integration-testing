using Aspire.Hosting.Testing;

namespace Api.Tests;

public class AspireFixture : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    private DistributedApplication? _app;

    public DistributedApplication App => _app ?? throw new InvalidOperationException("App has not been initialized.");

    public async ValueTask InitializeAsync()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(cancellationToken);

        _app = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await _app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("Api", cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }
}
