using MediatR;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Figures.Commands;

public class CreateFigureCommand : IRequest<FigureCommandResult>, IFigureMutationCommand
{
    public string Name { get; set; } = string.Empty;
    public double Height { get; set; }
    public int ReleaseYear { get; set; }
    public decimal Price { get; set; }
    public int TypeId { get; set; }
    public int BrandId { get; set; }
    public int ThemeId { get; set; }
    public int? SeriesId { get; set; }
    public List<int> MaterialIds { get; set; } = new();
    public List<string> ImageUrls { get; set; } = new();
}

public class CreateFigureCommandHandler : IRequestHandler<CreateFigureCommand, FigureCommandResult>
{
    private readonly IFigureRepository _figureRepository;
    private readonly IFigureReferenceRepository _figureReferenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFigureCommandHandler(
        IFigureRepository figureRepository,
        IFigureReferenceRepository figureReferenceRepository,
        IUnitOfWork unitOfWork)
    {
        _figureRepository = figureRepository;
        _figureReferenceRepository = figureReferenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FigureCommandResult> Handle(CreateFigureCommand request, CancellationToken cancellationToken)
    {
        var materials = await _figureReferenceRepository.GetMaterialsByIdsAsync(
            request.MaterialIds,
            cancellationToken);

        var figure = new Figure
        {
            Name = request.Name,
            Height = request.Height,
            ReleaseYear = request.ReleaseYear,
            Price = request.Price,
            TypeId = request.TypeId,
            BrandId = request.BrandId,
            ThemeId = request.ThemeId,
            SeriesId = request.SeriesId,
            Materials = materials,
            Images = request.ImageUrls
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select(url => new Images { ImageUrl = url })
                .ToList()
        };

        _figureRepository.Add(figure);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return FigureCommandResult.Success(figure.ToResponseDto());
    }
}
