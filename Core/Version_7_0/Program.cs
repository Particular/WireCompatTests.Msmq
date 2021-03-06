using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var bus = await CreateBus()
            .ConfigureAwait(false);
        await TestRunner.RunTests(bus)
            .ConfigureAwait(false);
    }

    static Task<IEndpointInstance> CreateBus()
    {
        var endpointConfiguration = new EndpointConfiguration(EndpointNames.EndpointName);
        var conventions = endpointConfiguration.Conventions();
        conventions.ApplyMessageConventions();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        PubSubConfigOverride.RegisterPublishers(transport);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        endpointConfiguration.EnableInstallers();

        return Endpoint.Start(endpointConfiguration);
    }
}