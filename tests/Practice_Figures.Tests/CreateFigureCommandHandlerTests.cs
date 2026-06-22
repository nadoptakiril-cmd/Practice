using Moq;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.Commands;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Tests;

public class CreateFigureCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenCommandIsValid_AddsFigureAndSavesChanges()
    {
        var figureRepository = new Mock<IFigureRepository>();
        var referenceRepository = new Mock<IFigureReferenceRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        referenceRepository
            .Setup(x => x.GetMaterialsByIdsAsync(
                It.IsAny<List<int>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Materials>
            {
                new() { Id = 1, Name = "ABS plastic" },
                new() { Id = 4, Name = "Metal" }
            });

        Figure? addedFigure = null;
        figureRepository
            .Setup(x => x.Add(It.IsAny<Figure>()))
            .Callback<Figure>(figure => addedFigure = figure);

        unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateFigureCommandHandler(
            figureRepository.Object,
            referenceRepository.Object,
            unitOfWork.Object);

        var command = new CreateFigureCommand
        {
            Name = "Iron Man",
            Height = 28,
            ReleaseYear = 2019,
            Price = 8000,
            TypeId = 1,
            BrandId = 1,
            ThemeId = 1,
            SeriesId = 2,
            MaterialIds = new List<int> { 1, 4 },
            ImageUrls = new List<string> { "/images/figures/1.jpg", "" }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(FigureCommandStatus.Success, result.Status);
        Assert.NotNull(addedFigure);
        Assert.Equal("Iron Man", addedFigure.Name);
        Assert.Equal(2, addedFigure.Materials.Count);
        Assert.Single(addedFigure.Images);
        Assert.Equal("/images/figures/1.jpg", addedFigure.Images.First().ImageUrl);

        figureRepository.Verify(x => x.Add(It.IsAny<Figure>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
