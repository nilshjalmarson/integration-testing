using System.Text.RegularExpressions;

namespace Demo3.Api.Tests;

public sealed partial class LiftParityTests(AspireFixture fixture) : IClassFixture<AspireFixture>
{
    [Theory]
    [InlineData(1, "OpenLift")]
    [InlineData(4, "ClosedLift")]
    public async Task existing_lift_requests_return_matching_payloads_from_old_and_new_apis(int liftId, string snapshotName)
    {

    }

    [Fact]
    public async Task missing_lift_requests_return_matching_not_found_payloads_from_old_and_new_apis()
    {

    }

    private static async Task<string> VerifyResponse(HttpResponseMessage response, string snapshotName)
    {
        var settings = new VerifySettings();

        var verifyResult = await Verify(response, settings)
            .UseFileName(snapshotName);

        return verifyResult.Text;
    }
}