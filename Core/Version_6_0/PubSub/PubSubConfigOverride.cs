﻿using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#pragma warning disable CS0618
public class PubSubConfigOverride : IProvideConfiguration<UnicastBusConfig>
{
    public UnicastBusConfig GetConfiguration()
    {
        var messageEndpointMappingCollection = new MessageEndpointMappingCollection();
        foreach (var endpointName in EndpointNames.All)
        {
            var assemblyName = endpointName.Replace("WireCompat","");
            var mapping = new MessageEndpointMapping
                {
                    Messages = $"{assemblyName}.Messages",
                    TypeFullName = $"{assemblyName}.Messages.MyEvent",
                    Endpoint = endpointName
                };
            messageEndpointMappingCollection.Add(mapping);
        }
        return new UnicastBusConfig
            {
                MessageEndpointMappings = messageEndpointMappingCollection
            };
    }
}
#pragma warning restore CS0618