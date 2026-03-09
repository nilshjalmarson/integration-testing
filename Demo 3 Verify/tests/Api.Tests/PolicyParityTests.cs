using System.Text.RegularExpressions;

namespace Api.Tests;

public sealed partial class PolicyParityTests(AspireFixture fixture) : IClassFixture<AspireFixture>
{
    private static readonly Regex GuidRegex = GuidPattern();
    private static readonly Regex TimestampRegex = TimestampPattern();

    [Theory]
    [InlineData("P-100", "StandardPolicy")]
    [InlineData("P-200", "PendingPolicy")]
    public async Task existing_policy_requests_return_matching_payloads_from_old_and_new_apis(string policyNumber, string snapshotName)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        using var oldResponse = await fixture.OldApiClient.GetAsync($"/policies/{policyNumber}", cancellationToken);
        using var newResponse = await fixture.NewApiClient.GetAsync($"/policies/{policyNumber}", cancellationToken);

        var oldText = await VerifyResponse(oldResponse, $"{snapshotName}.OldResponse");
        var newText = await VerifyResponse(newResponse, $"{snapshotName}.NewResponse");

        Assert.Equal(oldText, newText);
    }

    [Fact]
    public async Task missing_policy_requests_return_matching_not_found_payloads_from_old_and_new_apis()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        using var oldResponse = await fixture.OldApiClient.GetAsync("/policies/P-404", cancellationToken);
        using var newResponse = await fixture.NewApiClient.GetAsync("/policies/P-404", cancellationToken);

        var oldText = await VerifyResponse(oldResponse, "MissingPolicy.OldResponse");
        var newText = await VerifyResponse(newResponse, "MissingPolicy.NewResponse");

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