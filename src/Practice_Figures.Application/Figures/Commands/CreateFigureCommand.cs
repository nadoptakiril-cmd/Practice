using MediatR;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Application.Figures.Validators;
using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Figures.Commands;

public class CreateFigureCommand : IRequest<FigureResponseDto>
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

public class CreateFigureCommandHandler : IRequestHandler<CreateFigureCommand, FigureResponseDto>
{
    private readonly IFigureRepository _figureRepository;
    private readonly ILookupRepository _lookupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFigureCommandHandler(
        IFigureRepository figureRepository,
        ILookupRepository lookupRepository,
        IUnitOfWork unitOfWork)
    {
        _figureRepository = figureRepository;
        _lookupRepository = lookupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FigureResponseDto> Handle(CreateFigureCommand request, CancellationToken cancellationToken)
    {
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

        var now = DateTime.UtcNow;
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
            CreatedAt = now,
            UpdatedAt = now,
            Materials = materials,
            Images = request.ImageUrls
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select(url => new Images { ImageUrl = url })
                .ToList()
        };

        _figureRepository.Add(figure);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdFigure = await _figureRepository.GetByIdWithDetailsAsync(figure.Id, cancellationToken)
            ?? throw new InvalidOperationException("Created figure was not found.");

        return createdFigure.ToResponseDto();
    }
}
