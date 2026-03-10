using System.Runtime.CompilerServices;
using DiffEngine;

namespace Demo3.Api.Tests;

public static class VerifyGlobalSettings
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        VerifierSettings.ScrubEmptyLines();
        VerifierSettings.IgnoreMember("Content-Length");
        VerifierSettings.IgnoreMember("Date");
        VerifierSettings.IgnoreMember("Server");
        VerifyHttp.Initialize();        
        DiffTools.UseOrder(DiffTool.VisualStudioCode);
    }
}