using Application.Ports.Postgres;
using Application.UseCases.Commands.Model.UpdateModelCategory;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Commands.Model;

[TestSubject(typeof(UpdateModelCategoryHandler))]
public class UpdateModelCategoryHandlerShould
{
    private readonly UpdateModelCategoryRequest _request = new(Guid.NewGuid(), 'B');

    private readonly global::Domain.ModelAggregate.Model _modelFromDb = global::Domain.ModelAggregate.Model.Create(
        Brand.Create("Kia"), CarModel.Create("Rio"),
        Category.Create(Category.BCategory), Tariff.Create(10.0M, 300.0M, 4000.0M));
    
    private readonly Mock<IModelRepository> _modelRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly UpdateModelCategoryHandler _handler;

    public UpdateModelCategoryHandlerShould()
    {
        _modelRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(_modelFromDb);
        _unitOfWorkMock.Setup(x => x.Commit()).ReturnsAsync(Result.Ok);
        _handler = new UpdateModelCategoryHandler(_modelRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ReturnSuccess()
    {
        // Arrange

        // Act
        var actual = await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsSuccess);
    }

    [Fact]
    public async Task ReturnFailIfModelNotFound()
    {
        // Arrange
        _modelRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(null as global::Domain.ModelAggregate.Model);

        // Act
        var actual = await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.IsType<NotFound>(actual.Errors[0]);
    }

    [Fact]
    public async Task ReturnFailIfCommitFailed()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.Commit()).ReturnsAsync(Result.Fail("error"));

        // Act
        var actual = await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsFailed);
    }
    
    [Fact]
    public async Task VerifyCommitCall()
    {
        // Arrange

        // Act
        await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }
}