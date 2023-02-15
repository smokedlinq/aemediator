# Azure Event Grid Mediator

---

![CI](https://github.com/smokedlinq/aemediator/workflows/CI/badge.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=smokedlinq_aemediator&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=smokedlinq_aemediator)
[![NuGet](https://img.shields.io/nuget/dt/MediatR.Azure.EventGrid.svg)](https://www.nuget.org/packages/MediatR.Azure.EventGrid)
[![NuGet](https://img.shields.io/nuget/vpre/MediatR.Azure.EventGrid.svg)](https://www.nuget.org/packages/MediatR.Azure.EventGrid)

A [MediatR](/jbogard/MediatR) wrapper for Azure Event Grid.

## Installing MediatR.Azure.EventGrid

You can install the latest version of `MediatR.Azure.EventGrid` from NuGet by running the following command in PowerShell:

```powershell
Install-Package MediatR.Azure.EventGrid
```

Alternatively, you can use the .NET command line:

```dotnetcli
dotnet add package MediatR.Azure.EventGrid
```

## Getting Started

To use `EventGridMediator`, you need to register it with the `IServiceCollection` along with MediatR:

```csharp
services
    .AddMediatR(mediator => mediator.RegisterServicesFromAssembly(typeof(Program).Assembly))
    .AddEventGridMediator();
```

This will automatically deserialize the [Event Grid system topics](https://learn.microsoft.com/azure/event-grid/system-topics) data.

## Custom Events

For custom events, you need to register each data type:

- For `EventGridEvent` objects, the `EventType` and `DataVersion` properties are used to resolve the .NET type.
- For `CloudEvent` objects, the `Type` and `DataSchema` properties are used to resolve the .NET type.

To register a custom data type, use the `AddDataType` method on the `EventGridMediatorBuilder`:

```csharp
services.AddEventGridMediator(builder =>
{
    builder
        .AddDataType<MyCustomEventData>(nameof(MyCustomEventData))
        .AddDataType<MyCustomEventDataV2>(nameof(MyCustomEventData), "2.0");
});
```

Alternatively, you can use the `EventGridDataTypeAttribute` attribute on classes to register them through assembly discovery of public types:

```csharp
services.AddEventGridMediator(builder => builder.RegisterDataTypesFromAssembly(typeof(Program).Assembly));

[EventGridDataType(nameof(MyCustomEventData))]
public class MyCustomEventData
{
}
```

## Publishing Events to MediatR

To publish `EventGridEvent` objects using the `EventGridMediator`, use the following code (the example is an ASP.NET Core Minimal API):

```csharp
app.MapPost("/api/events", async (HttpContext context, CancellationToken cancellationToken) =>
{
    var json = await BinaryData.FromStreamAsync(context.Request.Body, cancellationToken).ConfigureAwait(false);
    var events = EventGridEvent.ParseMany(json);
    var mediator = context.RequestServices.GetRequiredService<EventGridMediator>();

    await mediator.PublishAsync(events, cancellationToken);
});
```

To publish `CloudEvent` objects, use the following code (the example is an ASP.NET Core Minimal API):

```csharp
app.MapPost("/api/events", async (HttpContext context, CancellationToken cancellationToken) =>
{
    var json = await BinaryData.FromStreamAsync(context.Request.Body, cancellationToken).ConfigureAwait(false);
    var events = CloudEvent.ParseMany(json);
    var mediator = context.RequestServices.GetRequiredService<EventGridMediator>();

    await mediator.PublishAsync(events, cancellationToken);
});
```

## Handling Events

By default, MediatR will invoke each `INotificationHandler<T>` in a sequential manner. However, as of MediatR 12.0.0, you can now parallelize the notification with the `TaskWhenAllPublisher` strategy. For more information, see [pull request 838](https://github.com/jbogard/MediatR/pull/838).

Regardless of the notification strategy you choose, keep in mind that handlers should be idempotent because any failures may require reprocessing of the event.

To handle `EventGridEvent` objects, implement `IEventGridEventHandler<T>`:

```csharp
public record MyCustomEventData { }

public class MyCustomEventHandler : IEventGridEventHandler<MyCustomEventData>
{
    public Task HandleAsync(EventGridEvent eventGridEvent, MyCustomEventData data, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Received event {eventGridEvent.Id} of type {eventGridEvent.EventType} with data {data}.");
        return Task.CompletedTask;
    }
}
```

To handle `CloudEvent` objects, implement `ICloudEventHandler<T>`:

```csharp
public record MyCustomEventData { }

public class MyCustomEventHandler : ICloudEventHandler<MyCustomEventData>
{
    public Task HandleAsync(CloudEvent cloudEvent, MyCustomEventData data, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Received event {cloudEvent.Id} of type {cloudEvent.Type} with data {data}.");
        return Task.CompletedTask;
    }
}
```
