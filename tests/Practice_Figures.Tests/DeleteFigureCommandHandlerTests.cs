using Moq;
using Practice_Figures.Application.Figures.Commands;
using Practice_Figures.Core.Entities;
using Practice_Figures.Core.Interfaces;

namespace Practice_Figures.Tests;

public class DeleteFigureCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenFigureDoesNotExist_CompletesWithoutSaving()
    {
        var figureRepository = new Mock<IFigureRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        figureRepository
            .Setup(x => x.GetByIdWithDetailsAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Figure?)null);

        var handler = new DeleteFigureCommandHandler(
            figureRepository.Object,
            unitOfWork.Object);

        await handler.Handle(new DeleteFigureCommand(10), CancellationToken.None);

        figureRepository.Verify(
            x => x.GetByIdWithDetailsAsync(10, It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenFigureExists_MarksFigureAsDeletedAndSavesChanges()
    {
        var figureRepository = new Mock<IFigureRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var figure = new Figure { Id = 10, Name = "Iron Man" };

        figureRepository
            .Setup(x => x.GetByIdWithDetailsAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(figure);

        unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new DeleteFigureCommandHandler(
            figureRepository.Object,
            unitOfWork.Object);

        await handler.Handle(new DeleteFigureCommand(10), CancellationToken.None);

        Assert.True(figure.IsDeleted);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
