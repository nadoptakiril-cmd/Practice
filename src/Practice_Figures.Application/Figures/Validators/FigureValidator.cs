using Practice_Figures.Application.Common.Interfaces;

namespace Practice_Figures.Application.Figures.Validators;

public class FigureValidator : IFigureValidator
{
    private readonly IFigureReferenceRepository _figureReferenceRepository;

    public FigureValidator(IFigureReferenceRepository figureReferenceRepository)
    {
        _figureReferenceRepository = figureReferenceRepository;
    }

    public async Task<List<string>> ValidateAsync(
        IFigureMutationCommand command,
        CancellationToken cancellationToken)
    {
        var missing = new List<string>();

        var type = await _figureReferenceRepository.GetTypeByIdAsync(command.TypeId, cancellationToken);
        if (type is null)
            missing.Add($"type_id={command.TypeId}");

        var brand = await _figureReferenceRepository.GetBrandByIdAsync(command.BrandId, cancellationToken);
        if (brand is null)
            missing.Add($"brand_id={command.BrandId}");

        var theme = await _figureReferenceRepository.GetThemeByIdAsync(command.ThemeId, cancellationToken);
        if (theme is null)
            missing.Add($"theme_id={command.ThemeId}");

        if (command.SeriesId.HasValue)
        {
            var series = await _figureReferenceRepository.GetSeriesByIdAndThemeIdAsync(
                command.SeriesId.Value,
                command.ThemeId,
                cancellationToken);

            if (series is null)
                missing.Add($"series_id={command.SeriesId} for theme_id={command.ThemeId}");
        }

        var existingMaterials = await _figureReferenceRepository.GetMaterialsByIdsAsync(
            command.MaterialIds,
            cancellationToken);

        var missingMaterialIds = command.MaterialIds
            .Except(existingMaterials.Select(material => material.Id))
            .ToList();

        if (missingMaterialIds.Count > 0)
            missing.Add($"materialIds={string.Join(", ", missingMaterialIds)}");

        return missing;
    }
}
