using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MediatR.Azure.EventGrid;

internal static class EventNotificationFactory
{
    private delegate INotification EventNotificationFactoryDelegate(object @event, object? data);
    private static readonly ConcurrentDictionary<Type, EventNotificationFactoryDelegate> Delegates = new();

    public static object Create<T>(T @event, Type dataType, object? data)
        where T : class
    {
        var notificationType = typeof(EventNotification<,>).MakeGenericType(typeof(T), dataType);
        var factory = Delegates.GetOrAdd(notificationType, _ => CreateDelegate(notificationType, typeof(T), dataType));
        return factory(@event, data);
    }

    private static EventNotificationFactoryDelegate CreateDelegate(Type notificationType, Type eventType, Type dataType)
    {
        var parameterTypes = new[] { eventType, dataType };
        var constructor = notificationType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, parameterTypes)
            ?? throw new InvalidOperationException($"Could not find constructor for type '{notificationType}'.");
        var @event = Expression.Parameter(typeof(object), "event");
        var data = Expression.Parameter(typeof(object), "data");
        var castEventParameter = Expression.Convert(@event, eventType);
        var castDataParameter = Expression.Convert(data, dataType);
        var @new = Expression.New(constructor, castEventParameter, castDataParameter);
        var lambda = Expression.Lambda<EventNotificationFactoryDelegate>(@new, @event, data);
        return lambda.Compile();
    }
}
