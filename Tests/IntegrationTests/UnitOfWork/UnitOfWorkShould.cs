using Domain.ModelAggregate;
using Domain.SharedKernel;
using Domain.SharedKernel.ValueObjects;
using Infrastructure.Adapters.Postgres;
using JetBrains.Annotations;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests.UnitOfWork;

[TestSubject(typeof(Infrastructure.Adapters.Postgres.UnitOfWork))]
public class UnitOfWorkShould : IntegrationTestBase
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        ContractResolver = new PrivateSetterContractResolver(),
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly Model _model = Model.Create(Brand.Create("Kia"), CarModel.Create("Rio"),
        Category.Create(Category.BCategory),
        Tariff.Create(10.0M, 300.0M, 4000.0M)); // Создается доменный ивент о создании модели

    [Fact]
    public async Task SaveDomainEventToOutbox()
    {
        // Arrange
        var expectedDomainEvent = _model.DomainEvents[0];
        var uowBuilder = new UnitOfWorkBuilder();
        var uow = uowBuilder.Build(Context);

        // Act
        await Context.Models.AddAsync(_model, TestContext.Current.CancellationToken);
        await uow.Commit();

        // Assert
        var outboxEvent = Context.Outbox.FirstOrDefault();
        var eventParsedContent =
            JsonConvert.DeserializeObject<DomainEvent>(outboxEvent!.Content, _jsonSerializerSettings);
        Assert.NotNull(eventParsedContent);
        Assert.Equivalent(expectedDomainEvent, eventParsedContent);
    }

    private class UnitOfWorkBuilder
    {
        public Infrastructure.Adapters.Postgres.UnitOfWork Build(DataContext context)
        {
            return new Infrastructure.Adapters.Postgres.UnitOfWork(context);
        }
    }
}