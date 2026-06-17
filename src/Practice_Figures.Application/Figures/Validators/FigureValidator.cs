using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Figures.Validators;

public static class FigureValidator
{
    public static async Task<(List<string> Missing, List<Materials> Materials)> ValidateAsync(
        ILookupRepository lookupRepository,
        int typeId,
        int brandId,
        int themeId,
        int? seriesId,
        List<int> materialIds,
        CancellationToken cancellationToken)
    {
        var missing = new List<string>();

        if (!await lookupRepository.TypeExistsAsync(typeId, cancellationToken))
            missing.Add($"type_id={typeId}");

        if (!await lookupRepository.BrandExistsAsync(brandId, cancellationToken))
            missing.Add($"brand_id={brandId}");

        if (!await lookupRepository.ThemeExistsAsync(themeId, cancellationToken))
            missing.Add($"theme_id={themeId}");

        if (seriesId.HasValue &&
            !await lookupRepository.SeriesBelongsToThemeAsync(
                seriesId.Value,
                themeId,
                cancellationToken))
        {
            missing.Add($"series_id={seriesId} for theme_id={themeId}");
        }

        var materials = await lookupRepository.GetMaterialsByIdsAsync(materialIds, cancellationToken);

        var missingMaterialIds = materialIds
            .Except(materials.Select(m => m.Id))
            .ToList();

        if (missingMaterialIds.Count > 0)
            missing.Add($"materialIds={string.Join(", ", missingMaterialIds)}");

        return (missing, materials);
    }
}
