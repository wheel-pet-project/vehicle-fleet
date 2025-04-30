using Domain.ModelAggregate;
using Domain.SharedKernel.Exceptions.PublicExceptions;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.ModelAggregate;

[TestSubject(typeof(Model))]
public class ModelShould
{
    private readonly Model _model = Model.Create(Brand.Create("Kia"), CarModel.Create("Rio"),
        Category.Create(Category.BCategory), Tariff.Create(10.0M, 300.0M, 4000.0M));

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
        void Act()
        {
            Model.Create(null!, _carModel, _category, _tariff);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfCarModelIsNull()
    {
        // Arrange

        // Act
        void Act()
        {
            Model.Create(_brand, null!, _category, _tariff);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfCategoryIsNull()
    {
        // Arrange

        // Act
        void Act()
        {
            Model.Create(_brand, _carModel, null!, _tariff);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfTariffIsNull()
    {
        // Arrange

        // Act
        void Act()
        {
            Model.Create(_brand, _carModel, _category, null!);
        }

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

        // Act
        _model.UpdateCategory(category);

        // Assert
        Assert.Equal(_category, _model.Category);
    }

    [Fact]
    public void AddDomainEventWhenUpdatingCategory()
    {
        // Arrange
        var category = Category.Create(Category.BCategory);
        _model.ClearDomainEvents();

        // Act
        _model.UpdateCategory(category);

        // Assert
        Assert.NotEmpty(_model.DomainEvents);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfCategoryForUpdateCategoryIsNull()
    {
        // Arrange

        // Act
        void Act()
        {
            _model.UpdateCategory(null!);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void UpdateTariff()
    {
        // Arrange
        var tariff = Tariff.Create(20.0M, 600.0M, 8000.0M);

        // Act
        _model.UpdateTariff(tariff);

        // Assert
        Assert.Equal(tariff, _model.Tariff);
    }

    [Fact]
    public void AddDomainEventWhenUpdatingTariff()
    {
        // Arrange
        var tariff = Tariff.Create(20.0M, 600.0M, 8000.0M);
        _model.ClearDomainEvents();

        // Act
        _model.UpdateTariff(tariff);

        // Assert
        Assert.NotEmpty(_model.DomainEvents);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfTariffIsNullWhenUpdatingTariff()
    {
        // Arrange

        // Act
        void Act()
        {
            _model.UpdateTariff(null!);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }
}