using MediatR.Azure.EventGrid.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Azure.EventGrid.Tests;

public class EventGridEventFactoryTests
{
    [Fact]
    public void Create_ShouldSetTypeAndDataSchema_WhenTypeIsMapped()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddEventGridDataTypes(builder => builder.Add<string>("type", "version"))
            .BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<EventGridEventFactory>();
        var eventData = string.Empty;

        // Act
        var eventGridEvent = factory.Create("subject", eventData);

        // Assert
        eventGridEvent.EventType.Should().Be("type");
        eventGridEvent.DataVersion.Should().Be("version");
    }

    [Fact]
    public void Create_ShouldThrowEventGridDataTypeNotFoundException_WhenTypeIsNotMapped()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddEventGridDataTypes()
            .BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<EventGridEventFactory>();
        var eventData = string.Empty;

        // Act
        var action = factory.Invoking(_ => factory.Create("subject", eventData));

        // Assert
        action.Should().Throw<EventGridDataTypeNotFoundException>();
    }
}
