using Domain.ModelAggregate;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.ModelAggregate;

[TestSubject(typeof(Model))]
public class ModelShould
{
    private readonly Brand _brand = Brand.Create("Kia");
    private readonly CarModel _carModel = CarModel.Create("Rio");
    private readonly Category _category = Category.Create(Category.BCategory);
    private readonly Tariff _tariff = Tariff.Create(10.0M, 300.0M, 4000.0M);
    
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange

        // Act
        var actual = Model.Create(_brand, _carModel, _category, _tariff);

        // Assert
        Assert.NotNull(actual);
        Assert.NotEqual(Guid.Empty, actual.Id);
        Assert.Equal(_brand, actual.Brand);
        Assert.Equal(_carModel, actual.CarModel);
        Assert.Equal(_category, actual.Category);
        Assert.Equal(_tariff, actual.Tariff);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfSBrandIsNull()
    {
        // Arrange

        // Act
        void Act() => Model.Create(null!, _carModel, _category, _tariff);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfCarModelIsNull()
    {
        // Arrange

        // Act
        void Act() => Model.Create(_brand, null!, _category, _tariff);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfCategoryIsNull()
    {
        // Arrange

        // Act
        void Act() => Model.Create(_brand, _carModel, null!, _tariff);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfTariffIsNull()
    {
        // Arrange
        
        // Act
        void Act() => Model.Create(_brand, _carModel, _category, null!);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void AddDomainEventWhenCreatingNewInstance()
    {
        // Arrange

        // Act
        var actual = Model.Create(_brand, _carModel, _category, _tariff);

        // Assert
        Assert.NotEmpty(actual.DomainEvents);
    }

    [Fact]
    public void UpdateCategory()
    {
        // Arrange
        var category = Category.Create(Category.BCategory);
        var model = Model.Create(_brand, _carModel, _category, _tariff);

        // Act
        model.UpdateCategory(category);

        // Assert
        Assert.Equal(_category, model.Category);
    }

    [Fact]
    public void AddDomainEventWhenUpdatingCategory()
    {
        // Arrange
        var category = Category.Create(Category.BCategory);
        var model = Model.Create(_brand, _carModel, _category, _tariff);

        // Act
        model.UpdateCategory(category);

        // Assert
        Assert.NotEmpty(model.DomainEvents);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfCategoryForUpdateCategoryIsNull()
    {
        // Arrange
        var model = Model.Create(_brand, _carModel, _category, _tariff);

        // Act
        void Act() => model.UpdateCategory(null!);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void UpdateTariff()
    {
        // Arrange
        var tariff = Tariff.Create(20.0M, 600.0M, 8000.0M);
        var model = Model.Create(_brand, _carModel, _category, tariff);
        
        // Act
        model.UpdateTariff(30.0M, 700.0M, 9000.0M);

        // Assert
        Assert.Equal(tariff, model.Tariff);
    }
    
    [Fact]
    public void AddDomainEventWhenUpdatingTariff()
    {
        // Arrange
        var tariff = Tariff.Create(20.0M, 600.0M, 8000.0M);
        var model = Model.Create(_brand, _carModel, _category, tariff);

        // Act
        model.UpdateTariff(30.0M, 700.0M, 9000.0M);

        // Assert
        Assert.NotEmpty(model.DomainEvents);
    }
}