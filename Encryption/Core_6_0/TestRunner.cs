using System;
using System.Threading.Tasks;
using NServiceBus;

public static class TestRunner
{
    public static string EndpointName { get; set; }

    public static async Task RunTests(IEndpointInstance bus)
    {
        await Task.Delay(TimeSpan.FromSeconds(25)).ConfigureAwait(false);
        await bus.InitiateSendReply().ConfigureAwait(false);

        await Task.Delay(TimeSpan.FromMinutes(1)).ConfigureAwait(false);
        await bus.Stop().ConfigureAwait(false);
        SendReplyVerifier.AssertExpectations();
    }
}