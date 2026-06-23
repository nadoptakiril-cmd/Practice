using MediatR;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Core.Entities;
using Practice_Figures.Core.Interfaces;

namespace Practice_Figures.Application.Figures.Commands;

public class UpdateFigureCommand : IRequest<FigureCommandResult>, IFigureMutationCommand
{
    public int Id { get; set; }
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

public class UpdateFigureCommandHandler : IRequestHandler<UpdateFigureCommand, FigureCommandResult>
{
    private readonly IFigureRepository _figureRepository;
    private readonly IFigureReferenceRepository _figureReferenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFigureCommandHandler(
        IFigureRepository figureRepository,
        IFigureReferenceRepository figureReferenceRepository,
        IUnitOfWork unitOfWork)
    {
        _figureRepository = figureRepository;
        _figureReferenceRepository = figureReferenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FigureCommandResult> Handle(UpdateFigureCommand request, CancellationToken cancellationToken)
    {
        var existingFigure = await _figureRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (existingFigure is null)
            return FigureCommandResult.NotFound();

        var materials = await _figureReferenceRepository.GetMaterialsByIdsAsync(
            request.MaterialIds,
            cancellationToken);

        existingFigure.Name = request.Name;
        existingFigure.Height = request.Height;
        existingFigure.ReleaseYear = request.ReleaseYear;
        existingFigure.Price = request.Price;
        existingFigure.TypeId = request.TypeId;
        existingFigure.BrandId = request.BrandId;
        existingFigure.ThemeId = request.ThemeId;
        existingFigure.SeriesId = request.SeriesId;

        existingFigure.Materials.Clear();
        foreach (var material in materials)
            existingFigure.Materials.Add(material);

        _figureRepository.RemoveImages(existingFigure.Images);
        existingFigure.Images = request.ImageUrls
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select(url => new Images { ImageUrl = url })
            .ToList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return FigureCommandResult.Success(existingFigure.ToResponseDto());
    }
}
