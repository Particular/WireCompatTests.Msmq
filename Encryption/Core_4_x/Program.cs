using System.Threading;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;
using NServiceBus.Unicast;

public class Program
{

    public static void Main()
    {
        //HACK: for trial dialog issue https://github.com/Particular/NServiceBus/issues/2001
        var synchronizationContext = SynchronizationContext.Current;
        var bus = CreateBus();
        SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        TestRunner.RunTests(bus);
    }

    static UnicastBus CreateBus()
    {
        Configure.GetEndpointNameAction = () => EndpointNames.EndpointName;

        Logging.ConfigureLogging();
        Asserter.LogError = log4net.LogManager.GetLogger("Asserter").Error;
        Configure.Features.Disable<TimeoutManager>();
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.DefiningMessagesAs(MessageConventions.IsMessage);
        configure.DefiningEncryptedPropertiesAs(MessageConventions.IsEncryptedProperty);
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.RijndaelEncryptionService();
        configure.Configurer.ConfigureComponent<MutatorVerifier>(DependencyLifecycle.SingleInstance);

        return (UnicastBus) configure.UnicastBus()
            .CreateBus().Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
    }
}