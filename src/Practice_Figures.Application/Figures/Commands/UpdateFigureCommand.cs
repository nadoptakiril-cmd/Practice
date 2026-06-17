using MediatR;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Application.Figures.Validators;
using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Figures.Commands;

public class UpdateFigureCommand : IRequest<bool>
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

public class UpdateFigureCommandHandler : IRequestHandler<UpdateFigureCommand, bool>
{
    private readonly IFigureRepository _figureRepository;
    private readonly ILookupRepository _lookupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFigureCommandHandler(
        IFigureRepository figureRepository,
        ILookupRepository lookupRepository,
        IUnitOfWork unitOfWork)
    {
        _figureRepository = figureRepository;
        _lookupRepository = lookupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateFigureCommand request, CancellationToken cancellationToken)
    {
        var existingFigure = await _figureRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (existingFigure is null)
            return false;

        var (missing, materials) = await FigureValidator.ValidateAsync(
            _lookupRepository,
            request.TypeId,
            request.BrandId,
            request.ThemeId,
            request.SeriesId,
            request.MaterialIds,
            cancellationToken);

        if (missing.Count > 0)
            throw new ArgumentException($"Not found: {string.Join("; ", missing)}");

        existingFigure.Name = request.Name;
        existingFigure.Height = request.Height;
        existingFigure.ReleaseYear = request.ReleaseYear;
        existingFigure.Price = request.Price;
        existingFigure.TypeId = request.TypeId;
        existingFigure.BrandId = request.BrandId;
        existingFigure.ThemeId = request.ThemeId;
        existingFigure.SeriesId = request.SeriesId;
        existingFigure.UpdatedAt = DateTime.UtcNow;

        existingFigure.Materials.Clear();
        foreach (var material in materials)
            existingFigure.Materials.Add(material);

        _figureRepository.RemoveImages(existingFigure.Images);
        existingFigure.Images = request.ImageUrls
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select(url => new Images { ImageUrl = url })
            .ToList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
