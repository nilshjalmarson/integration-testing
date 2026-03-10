using System.Text.RegularExpressions;

namespace Demo3.Api.Tests;

public sealed partial class LiftParityTests(AspireFixture fixture) : IClassFixture<AspireFixture>
{
    private static readonly Regex GuidRegex = GuidPattern();
    private static readonly Regex TimestampRegex = TimestampPattern();

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
        settings.ScrubHttpTextResponse(ScrubDynamicValues);

        var verifyResult = await Verify(response, settings)
            .DontScrubDateTimes()
            .UseFileName(snapshotName);

        return verifyResult.Text;
    }

    private static string ScrubDynamicValues(string text)
    {
        text = GuidRegex.Replace(text, "Guid_1");
        text = TimestampRegex.Replace(text, "DateTimeOffset_1");
        return text;
    }

    [GeneratedRegex(@"\b[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}\b", RegexOptions.CultureInvariant)]
    private static partial Regex GuidPattern();

    [GeneratedRegex(@"\b\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|[+\-]\d{2}:\d{2})\b", RegexOptions.CultureInvariant)]
    private static partial Regex TimestampPattern();
}