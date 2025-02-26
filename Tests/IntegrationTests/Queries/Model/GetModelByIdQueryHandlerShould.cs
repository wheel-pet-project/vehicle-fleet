using Application.UseCases.Queries.Model.GetModelById;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace IntegrationTests.Queries.Model;

[TestSubject(typeof(GetModelByIdQueryHandler))]
public class GetModelByIdQueryHandlerShould : IntegrationTestBase
{
    [Fact]
    public async Task ReturnModelById()
    {
        // Arrange
        var expectedModel = await AddModel();
        var queryHandler = new GetModelByIdQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(new GetModelByIdQuery(expectedModel.Id),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedModel.Id, actual.Value.Id);
        Assert.Equal(expectedModel.Brand.Name, actual.Value.Brand);
        Assert.Equal(expectedModel.CarModel.Name, actual.Value.CarModel);
        Assert.Equal((double)expectedModel.Tariff.PricePerMinute, actual.Value.PricePerMinute);
        Assert.Equal((double)expectedModel.Tariff.PricePerHour, actual.Value.PricePerHour);
        Assert.Equal((double)expectedModel.Tariff.PricePerDay, actual.Value.PricePerDay);
    }

    [Fact]
    public async Task ReturnFailIfNotFound()
    {
        // Arrange
        var queryHandler = new GetModelByIdQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(new GetModelByIdQuery(Guid.NewGuid()),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.IsType<NotFound>(actual.Errors[0]);
    }
    
    private async Task<Domain.ModelAggregate.Model> AddModel()
    {
        var model = Domain.ModelAggregate.Model.Create(Brand.Create("Kia"), CarModel.Create("Rio"),
            Category.Create('B'), Tariff.Create(1, 2, 3));
        
        await Context.Models.AddAsync(model);
        await Context.SaveChangesAsync();
        
        return model;
    }
}