using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Demo.Models;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo
{
    public class EventHubReceiverService : IHostedService
    {
        private readonly EventProcessorClient _processor;
        private readonly BlobContainerClient _storageClient;
        private readonly ITopicEventSender _eventSender;

        public EventHubReceiverService(
            IConfiguration configuration,
            ITopicEventSender eventSender)
        {
            // Read from the default consumer group: $Default
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // Create a blob container client that the event processor will use 
            _storageClient = new BlobContainerClient(
                configuration["Tracing:BlobStorage:ConnectionString"], 
                "myBlobForEvents");

            // Create an event processor client to process events in the event hub
            _processor = new EventProcessorClient(
                _storageClient, 
                consumerGroup, 
                connectionString: configuration["Tracing:EventHub:ConnectionString"]);

            // Register handlers for processing events and handling errors
            _processor.ProcessEventAsync += ProcessEventHandler;
            _processor.ProcessErrorAsync += ProcessErrorHandler;
            _eventSender = eventSender;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            /* create the blob */
            await _storageClient.CreateIfNotExistsAsync();

            /* Start the processing */
            await _processor.StartProcessingAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync();
        }

        async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            string eventMessageJson = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            EventHubTrace message = JsonConvert.DeserializeObject<EventHubTrace>(eventMessageJson);

            await _eventSender.SendAsync("trace", message);

            /* Update checkpoint in the blob storage so that the app 
             * receives only new events the next time it's run */
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': " +
                $"an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
