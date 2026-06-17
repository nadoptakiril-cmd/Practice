using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Common.Interfaces;

public interface ILookupRepository
{
    Task<bool> TypeExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> BrandExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> ThemeExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> SeriesBelongsToThemeAsync(int seriesId, int themeId, CancellationToken cancellationToken);
    Task<List<Materials>> GetMaterialsByIdsAsync(List<int> ids, CancellationToken cancellationToken);
}
