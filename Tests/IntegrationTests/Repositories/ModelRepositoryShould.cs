using Application.Ports.Postgres;
using Domain.ModelAggregate;
using Domain.SharedKernel.ValueObjects;
using Infrastructure.Adapters.Postgres;
using Infrastructure.Adapters.Postgres.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace IntegrationTests.Repositories;

[TestSubject(typeof(ModelRepository))]
public class ModelRepositoryShould : IntegrationTestBase
{
    private readonly Model _model = Model.Create(Brand.Create("Kia"), CarModel.Create("Rio"),
        Category.Create(Category.BCategory),
        Tariff.Create(10.0M, 300.0M, 4000.0M));
    
    [Fact]
    public async Task Add()
    {
        // Arrange
        var repositoryAndUowAndUnitOfWorkBuilderBuilder = new RepositoryAndUnitOfWorkBuilder();
        var (repository, uow) = repositoryAndUowAndUnitOfWorkBuilderBuilder.Build(Context);

        // Act
        await repository.Add(_model);
        await uow.Commit();
        
        // Assert
        _model.ClearDomainEvents();
        var modelFromDb = await repository.GetById(_model.Id);
        Assert.NotNull(modelFromDb);
        Assert.Equivalent(_model, modelFromDb);
    }

    [Fact]
    public async Task Update()
    {
        // Arrange
        var repositoryAndUowAndUnitOfWorkBuilderBuilder = new RepositoryAndUnitOfWorkBuilder();
        var (repositoryForArrange, uowForArrange) = repositoryAndUowAndUnitOfWorkBuilderBuilder.Build(Context);
        
        await repositoryForArrange.Add(_model);
        await uowForArrange.Commit();
        
        var (repository, uow) = repositoryAndUowAndUnitOfWorkBuilderBuilder.Build(Context);
        _model.UpdateTariff(pricePerMinute: 30.0M);

        // Act
        repository.Update(_model);
        await uow.Commit();
        
        // Assert
        _model.ClearDomainEvents();
        var modelFromDb = await repository.GetById(_model.Id);
        Assert.NotNull(modelFromDb);
        Assert.Equivalent(_model, modelFromDb);
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var repositoryAndUowAndUnitOfWorkBuilderBuilder = new RepositoryAndUnitOfWorkBuilder();
        var (repository, uow) = repositoryAndUowAndUnitOfWorkBuilderBuilder.Build(Context);

        await repository.Add(_model);
        await uow.Commit();
        
        // Act
        var actual = await repository.GetById(_model.Id);
        
        // Assert
        _model.ClearDomainEvents();
        Assert.NotNull(actual);
        Assert.Equivalent(_model, actual);
    }
    
    private class RepositoryAndUnitOfWorkBuilder
    {
        public (ModelRepository, Infrastructure.Adapters.Postgres.UnitOfWork) Build(DataContext context) =>
            (new ModelRepository(context), new Infrastructure.Adapters.Postgres.UnitOfWork(context));
    }
}