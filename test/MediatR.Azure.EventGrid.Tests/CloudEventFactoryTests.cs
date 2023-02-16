using MediatR.Azure.EventGrid.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Azure.EventGrid.Tests;

public class CloudEventFactoryTests
{
    [Fact]
    public void Create_ShouldSetTypeAndDataSchema_WhenTypeIsMapped()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddEventGridDataTypes(builder => builder.Add<string>("type", "version"))
            .BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<CloudEventFactory>();
        var eventData = string.Empty;

        // Act
        var cloudEvent = factory.Create("source", eventData);

        // Assert
        cloudEvent.Type.Should().Be("type");
        cloudEvent.DataSchema.Should().Be("version");
    }

    [Fact]
    public void Create_ShouldThrowEventGridDataTypeNotFoundException_WhenTypeIsNotMapped()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddEventGridDataTypes()
            .BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<CloudEventFactory>();
        var eventData = string.Empty;

        // Act
        var action = factory.Invoking(_ => factory.Create("source", eventData));

        // Assert
        action.Should().Throw<EventGridDataTypeNotFoundException>();
    }
}
