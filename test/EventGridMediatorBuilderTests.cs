using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Azure.EventGrid.Tests;

public class EventGridMediatorBuilderTests
{
    [Fact]
    public void RegisterDataTypesFromAssembly_ShouldFindPublicClassesWithAttribute_WhenAssemblyIsProvided()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new EventGridMediatorBuilder(services);

        // Act
        builder.RegisterDataTypesFromAssembly(typeof(EventGridMediatorBuilderTests).Assembly);

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var registrations = serviceProvider.GetServices<EventGridDataTypeRegistration>();
        var dataType = new EventGridDataType(nameof(EventGridMediatorBuilderTestsEvent), null);

        registrations.Should().Contain(x => x.DataType == dataType);
    }

    [EventGridDataType(nameof(EventGridMediatorBuilderTestsEvent))]
    public sealed class EventGridMediatorBuilderTestsEvent
    {
    }
}
