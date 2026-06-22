using Moq;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.Commands;
using Practice_Figures.Domain.Entities;

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

        figureRepository.Verify(x => x.Remove(It.IsAny<Figure>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
