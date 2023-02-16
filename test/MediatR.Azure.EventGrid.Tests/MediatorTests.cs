using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using MediatR.Azure.EventGrid.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Azure.EventGrid.Tests;

public class MediatorTests
{
    [Fact]
    public async Task PublishAsync_ShouldThrowEventDataTypeNotFoundException_WhenEventDataTypeIsNotRegistered()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR()
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var eventGridEvent = new EventGridEvent("subject", "eventType", "dataVersion", "data");

        // Act
        var action = mediator.Invoking(_ => _.PublishAsync(eventGridEvent));

        // Assert
        await action.Should().ThrowAsync<EventGridDataTypeNotFoundException>();
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveNotification_WhenEventGridEventIsPublishedAndEventDataIsNull()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<string?>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<EventGridEvent, string?>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("eventType", "dataVersion"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var eventGridEvent = new EventGridEvent("subject", "eventType", "dataVersion", BinaryData.FromObjectAsJson<string?>(null));

        // Act
        await mediator.PublishAsync(eventGridEvent);

        // Assert
        await handler.Received()
            .HandleAsync(eventGridEvent, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveNotification_WhenCloudEventIsPublishedAndEventDataIsNull()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<string?>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<CloudEvent, string?>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("type"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var cloudEvent = new CloudEvent("source", "type", null);

        // Act
        await mediator.PublishAsync(cloudEvent);

        // Assert
        await handler.Received()
            .HandleAsync(cloudEvent, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveNotification_WhenEventGridEventIsPublished()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<string>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<EventGridEvent, string>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("eventType", "dataVersion"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var eventGridEvent = new EventGridEvent("subject", "eventType", "dataVersion", "data");

        // Act
        await mediator.PublishAsync(eventGridEvent);

        // Assert
        await handler.Received()
            .HandleAsync(eventGridEvent, "data", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveNotification_WhenCloudEventIsPublished()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<string>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<CloudEvent, string>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("type"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var cloudEvent = new CloudEvent("source", "type", "data");

        // Act
        await mediator.PublishAsync(cloudEvent);

        // Assert
        await handler.Received()
            .HandleAsync(cloudEvent, "data", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveMultipleNotifications_WhenMultipleEventGridEventsArePublished()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<string>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<EventGridEvent, string>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("eventType", "dataVersion"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var eventGridEvent = new EventGridEvent("subject", "eventType", "dataVersion", "data");

        // Act
        await mediator.PublishAsync(new[] { eventGridEvent, eventGridEvent });

        // Assert
        await handler.Received(2)
            .HandleAsync(Arg.Any<EventGridEvent>(), "data", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveMultipleNotifications_WhenMultipleCloudEventsArePublished()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<string>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<CloudEvent, string>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("type"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var cloudEvent = new CloudEvent("source", "type", "data");

        // Act
        await mediator.PublishAsync(new[] { cloudEvent, cloudEvent });

        // Assert
        await handler.Received(2)
            .HandleAsync(Arg.Any<CloudEvent>(), "data", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveNotification_WhenSystemEventAsEventGridEvent()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<StorageBlobCreatedEventData>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<EventGridEvent, StorageBlobCreatedEventData>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("eventType"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var eventGridEvent = EventGridEvent.ParseMany(BinaryData.FromString(@"
            [
                {
                    ""id"": ""f94a40f0-2d7b-4c23-9e23-8b8f5dcbbe5b"",
                    ""eventType"": ""Microsoft.Storage.BlobCreated"",
                    ""subject"": ""/blobServices/default/containers/mycontainer/blobs/myblob"",
                    ""eventTime"": ""2022-02-14T15:25:00.0000000Z"",
                    ""data"": {
                        ""api"": ""PutBlockList"",
                        ""clientRequestId"": ""6d79dbfb-0e37-4fc4-981f-442c9ca65760"",
                        ""requestId"": ""831e1650-001e-001b-66ab-eeb76e000000"",
                        ""eTag"": ""0x8D1C5A9E8D4C3AE"",
                        ""contentType"": ""text/plain"",
                        ""contentLength"": 524288,
                        ""blobType"": ""BlockBlob"",
                        ""url"": ""https://myaccount.blob.core.windows.net/mycontainer/myblob"",
                        ""sequencer"": ""00000000000004420000000000028963"",
                        ""storageDiagnostics"": {
                            ""batchId"": ""b68529f3-68cd-4744-baa4-3c0498ec19f0""
                        }
                    },
                    ""dataVersion"": ""1"",
                    ""metadataVersion"": ""1""
                }
            ]")).First();

        // Act
        await mediator.PublishAsync(eventGridEvent);

        // Assert
        await handler.Received()
            .HandleAsync(eventGridEvent, Arg.Any<StorageBlobCreatedEventData>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PublishAsync_ShouldReceiveNotification_WhenSystemEventAsCloudEvent()
    {
        // Arrange
        var handler = Substitute.For<MockEventHandler<StorageBlobCreatedEventData>>();
        using var serviceProvider = new ServiceCollection()
            .AddTransient<INotificationHandler<EventGridNotification<CloudEvent, StorageBlobCreatedEventData>>>(_ => handler)
            .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(MediatorTests).Assembly))
            .AddEventGridMediatR(builder => builder.DataTypes.Add<string>("eventType"))
            .BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<EventGridMediator>();
        var cloudEvent = CloudEvent.ParseMany(BinaryData.FromString(@"
            [
                {
                    ""specversion"": ""1.0"",
                    ""type"": ""Microsoft.Storage.BlobCreated"",
                    ""source"": ""/blobServices/default/containers/mycontainer/blobs"",
                    ""id"": ""f94a40f0-2d7b-4c23-9e23-8b8f5dcbbe5b"",
                    ""time"": ""2022-02-14T15:25:00.0000000Z"",
                    ""datacontenttype"": ""application/json"",
                    ""data"": {
                        ""api"": ""PutBlockList"",
                        ""clientRequestId"": ""6d79dbfb-0e37-4fc4-981f-442c9ca65760"",
                        ""requestId"": ""831e1650-001e-001b-66ab-eeb76e000000"",
                        ""eTag"": ""0x8D1C5A9E8D4C3AE"",
                        ""contentType"": ""text/plain"",
                        ""contentLength"": 524288,
                        ""blobType"": ""BlockBlob"",
                        ""url"": ""https://myaccount.blob.core.windows.net/mycontainer/myblob"",
                        ""sequencer"": ""00000000000004420000000000028963"",
                        ""storageDiagnostics"": {
                        ""batchId"": ""b68529f3-68cd-4744-baa4-3c0498ec19f0""
                        }
                    }
                }
            ]")).First();

        // Act
        await mediator.PublishAsync(cloudEvent);

        // Assert
        await handler.Received()
            .HandleAsync(cloudEvent, Arg.Any<StorageBlobCreatedEventData>(), Arg.Any<CancellationToken>());
    }

    public abstract class MockEventHandler<T> : IEventGridEventHandler<T>, ICloudEventHandler<T>
    {
        public abstract Task HandleAsync(EventGridEvent eventGridEvent, T data, CancellationToken cancellationToken);
        public abstract Task HandleAsync(CloudEvent cloudEvent, T data, CancellationToken cancellationToken);
    }

}
