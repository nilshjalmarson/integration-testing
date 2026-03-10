using System.Net;
using System.Text.Json;

namespace Demo1.Api.Tests;

public class The_lifts_endpoint()
{
    [Fact]
    public async Task returns_ok_for_authorized_requests()
    {
        var apphost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(TestContext.Current.CancellationToken);
        var app = await apphost.BuildAsync(TestContext.Current.CancellationToken);

        await app.StartAsync(TestContext.Current.CancellationToken);
        var client = app.CreateHttpClient("Api");

        var token =  JwtTokenFactory.CreateToken("testuser");
        client.DefaultRequestHeaders.Authorization = new ("Bearer", token);

        var response = await client.GetAsync("/lifts", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task returns_unauthenticated_for_unauthorized_requests()
    {
        var apphost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(TestContext.Current.CancellationToken);
        var app = await apphost.BuildAsync(TestContext.Current.CancellationToken);

        await app.StartAsync(TestContext.Current.CancellationToken);
        var client = app.CreateHttpClient("Api");
        var response = await client.GetAsync("/lifts", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }    
}
