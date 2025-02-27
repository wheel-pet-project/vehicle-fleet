using Application.Ports.Postgres;
using Application.UseCases.Commands.Model.AddModel;
using FluentResults;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Commands.Model;

[TestSubject(typeof(AddModelHandler))]
public class AddModelHandlerShould
{
    private readonly AddModelRequest _request = new("Kia", "Rio", 'B', 1, 2, 3);

    private readonly Mock<IModelRepository> _modelRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly AddModelHandler _handler;

    public AddModelHandlerShould()
    {
        _unitOfWorkMock.Setup(x => x.Commit()).ReturnsAsync(Result.Ok);
        _handler = new AddModelHandler(_modelRepositoryMock.Object, _unitOfWorkMock.Object);
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
    public async Task ReturnModelId()
    {
        // Arrange

        // Act
        var actual = await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEqual(Guid.Empty, actual.Value.ModelId);
        Assert.NotNull(actual.Value);
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