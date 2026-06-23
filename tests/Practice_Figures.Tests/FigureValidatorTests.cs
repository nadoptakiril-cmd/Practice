using Moq;
using Practice_Figures.Application.Figures.Commands;
using Practice_Figures.Application.Figures.Validators;
using Practice_Figures.Core.Entities;
using Practice_Figures.Core.Interfaces;

namespace Practice_Figures.Tests;

public class FigureValidatorTests
{
    [Fact]
    public async Task ValidateAsync_WhenReferencesExist_ReturnsNoErrors()
    {
        var repository = new Mock<IFigureReferenceRepository>();

        repository
            .Setup(x => x.GetTypeByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Types { Id = 1, Name = "Figure" });

        repository
            .Setup(x => x.GetBrandByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Brands { Id = 1, Name = "Hot Toys" });

        repository
            .Setup(x => x.GetThemeByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Themes { Id = 1, Name = "Marvel" });

        repository
            .Setup(x => x.GetSeriesByIdAndThemeIdAsync(2, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Series { Id = 2, ThemeId = 1, Name = "Endgame" });

        repository
            .Setup(x => x.GetMaterialsByIdsAsync(
                It.IsAny<List<int>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Materials>
            {
                new() { Id = 1, Name = "ABS plastic" },
                new() { Id = 4, Name = "Metal" }
            });

        var validator = new FigureValidator(repository.Object);

        var command = new CreateFigureCommand
        {
            TypeId = 1,
            BrandId = 1,
            ThemeId = 1,
            SeriesId = 2,
            MaterialIds = new List<int> { 1, 4 }
        };

        var result = await validator.ValidateAsync(command, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task ValidateAsync_WhenBrandDoesNotExist_ReturnsBrandError()
    {
        var repository = new Mock<IFigureReferenceRepository>();

        repository
            .Setup(x => x.GetTypeByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Types { Id = 1, Name = "Figure" });

        repository
            .Setup(x => x.GetBrandByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Brands?)null);

        repository
            .Setup(x => x.GetThemeByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Themes { Id = 1, Name = "Marvel" });

        repository
            .Setup(x => x.GetMaterialsByIdsAsync(
                It.IsAny<List<int>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Materials>
            {
                new() { Id = 1, Name = "ABS plastic" }
            });

        var validator = new FigureValidator(repository.Object);

        var command = new CreateFigureCommand
        {
            TypeId = 1,
            BrandId = 99,
            ThemeId = 1,
            MaterialIds = new List<int> { 1 }
        };

        var result = await validator.ValidateAsync(command, CancellationToken.None);

        Assert.Contains("brand_id=99", result);
    }
}
