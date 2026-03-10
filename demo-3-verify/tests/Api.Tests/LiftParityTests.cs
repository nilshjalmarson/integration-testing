using System.Text.RegularExpressions;

namespace Demo3.Api.Tests;

public sealed partial class LiftParityTests(AspireFixture fixture) : IClassFixture<AspireFixture>
{
    [Theory]
    [InlineData(1, "OpenLift")]
    [InlineData(4, "ClosedLift")]
    public async Task existing_lift_requests_return_matching_payloads_from_old_and_new_apis(int liftId, string snapshotName)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        using var oldResponse = await fixture.OldApiClient.GetAsync($"/lifts/{liftId}", cancellationToken);
        using var newResponse = await fixture.NewApiClient.GetAsync($"/lifts/{liftId}", cancellationToken);

        var oldText = await VerifyResponse(oldResponse, $"{snapshotName}.OldResponse");
        var newText = await VerifyResponse(newResponse, $"{snapshotName}.NewResponse");

        Assert.Equal(oldText, newText);
    }

    [Fact]
    public async Task missing_lift_requests_return_matching_not_found_payloads_from_old_and_new_apis()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        using var oldResponse = await fixture.OldApiClient.GetAsync("/lifts/404", cancellationToken);
        using var newResponse = await fixture.NewApiClient.GetAsync("/lifts/404", cancellationToken);

        var oldText = await VerifyResponse(oldResponse, "MissingLift.OldResponse");
        var newText = await VerifyResponse(newResponse, "MissingLift.NewResponse");

        Assert.Equal(oldText, newText);
    }

    private static async Task<string> VerifyResponse(HttpResponseMessage response, string snapshotName)
    {
        var settings = new VerifySettings();
        settings.UseDirectory("Snapshots");
        settings.AutoVerify();
        settings.DisableRequireUniquePrefix();
        settings.ScrubGuids();
        settings.ScrubDateTimes();

        var verifyResult = await Verify(response, settings)
            .UseFileName(snapshotName);

        return verifyResult.Text;
    }
}