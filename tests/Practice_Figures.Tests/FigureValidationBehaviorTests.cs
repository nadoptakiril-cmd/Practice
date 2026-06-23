using MediatR;
using Moq;
using Practice_Figures.Application.Common.Behaviors;
using Practice_Figures.Application.Figures.Commands;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Application.Figures.Validators;
using Practice_Figures.Core.Entities;
using Practice_Figures.Core.Interfaces;

namespace Practice_Figures.Tests;

public class FigureValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WhenValidationFails_ReturnsValidationFailedAndDoesNotCallHandler()
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

        var behavior = new FigureValidationBehavior<CreateFigureCommand, FigureCommandResult>(
            validator);

        var handlerWasCalled = false;
        RequestHandlerDelegate<FigureCommandResult> next = _ =>
        {
            handlerWasCalled = true;
            return Task.FromResult(FigureCommandResult.Success(new FigureResponseDto()));
        };

        var result = await behavior.Handle(
            new CreateFigureCommand
            {
                TypeId = 1,
                BrandId = 99,
                ThemeId = 1,
                MaterialIds = new List<int> { 1 }
            },
            next,
            CancellationToken.None);

        Assert.Equal(FigureCommandStatus.ValidationFailed, result.Status);
        Assert.Contains("brand_id=99", result.Errors);
        Assert.False(handlerWasCalled);
    }
}
