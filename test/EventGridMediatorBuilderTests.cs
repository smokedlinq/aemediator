using MediatR.Azure.EventGrid.Serialization;
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

    [Fact]
    public void ConfigureJsonSerializerOptions_ShouldConfigureEventGridDataDeserializerOptions_WhenActionIsProvided()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new EventGridMediatorBuilder(services);

        // Act
        builder.ConfigureJsonSerializerOptions(options => options.PropertyNameCaseInsensitive = true);

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<EventGridDataDeserializerOptions>();

        options.JsonSerializerOptions.PropertyNameCaseInsensitive.Should().BeTrue();
    }

    [Fact]
    public void UseDataDeserializer_ShouldReplaceEventGridDataDeserializer_WhenInstanceIsProvided()
    {
        // Arrange
        var deserializer = Substitute.For<EventGridDataDeserializer>();
        var services = new ServiceCollection();
        var builder = new EventGridMediatorBuilder(services);

        // Act
        builder.UseDataDeserializer<EventGridDataDeserializer>(_ => deserializer);

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var dataDeserializer = serviceProvider.GetRequiredService<EventGridDataDeserializer>();

        dataDeserializer.Should().Be(deserializer);
    }

    [Fact]
    public void UseDataTypeResolver_ShouldReplaceEventGridDataTypeResolver_WhenInstanceIsProvided()
    {
        // Arrange
        var resolver = Substitute.For<EventGridDataTypeResolver>();
        var services = new ServiceCollection();
        var builder = new EventGridMediatorBuilder(services);

        // Act
        builder.UseDataTypeResolver<EventGridDataTypeResolver>(_ => resolver);

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var dataTypeResolver = serviceProvider.GetRequiredService<EventGridDataTypeResolver>();

        dataTypeResolver.Should().Be(resolver);
    }

    [Fact]
    public void UseEventGridMediator_ShouldReplaceEventGridMediator_WhenInstanceIsProvided()
    {
        // Arrange
        var mediator = Substitute.For<EventGridMediator>();
        var services = new ServiceCollection();
        var builder = new EventGridMediatorBuilder(services);

        // Act
        builder.UseMediator<EventGridMediator>(_ => mediator);

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var eventGridMediator = serviceProvider.GetRequiredService<EventGridMediator>();

        eventGridMediator.Should().Be(mediator);
    }

    [EventGridDataType(nameof(EventGridMediatorBuilderTestsEvent))]
    public sealed class EventGridMediatorBuilderTestsEvent
    {
    }
}
