using Application.UseCases.Queries.Model.GetAllModels;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace IntegrationTests.Queries.Model;

[TestSubject(typeof(GetAllModelsQueryHandler))]
public class GetAllModelsQueryHandlerShould : IntegrationTestBase
{
    [Fact]
    public async Task ReturnModelWithCorrectValues()
    {
        // Arrange
        var expectedModels = await AddModels(1);
        var queryHandler = new GetAllModelsQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(new GetAllModelsQuery(1, 10),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedModels.Count, actual.Value.Models.Count);
        Assert.Equal(expectedModels[0].Id, actual.Value.Models[0].Id);
        Assert.Equal(expectedModels[0].Brand.Name, actual.Value.Models[0].Brand);
        Assert.Equal(expectedModels[0].CarModel.Name, actual.Value.Models[0].CarModel);
    }

    [Fact]
    public async Task ReturnAllModels()
    {
        // Arrange
        var expectedModels = await AddModels(5);
        var queryHandler = new GetAllModelsQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(new GetAllModelsQuery(1, 10),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedModels.Count, actual.Value.Models.Count);
    }

    [Fact]
    public async Task ReturnEmptyListWhenNoVehicles()
    {
        // Arrange
        var queryHandler = new GetAllModelsQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(new GetAllModelsQuery(1, 3),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Empty(actual.Value.Models);
    }

    private async Task<List<Domain.ModelAggregate.Model>> AddModels(int count)
    {
        var models = Enumerable.Range(1, count)
            .Select(i => Domain.ModelAggregate.Model.Create(Brand.Create($"Kia{i}"), CarModel.Create($"Rio{i}"),
                Category.Create('B'), Tariff.Create(1, 2, 3)))
            .ToList();

        await Context.Models.AddRangeAsync(models);
        await Context.SaveChangesAsync();

        return models;
    }
}