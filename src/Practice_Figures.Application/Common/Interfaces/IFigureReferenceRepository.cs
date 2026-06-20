using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Common.Interfaces;

public interface IFigureReferenceRepository
{
    Task<Types?> GetTypeByIdAsync(int id, CancellationToken cancellationToken);
    Task<Brands?> GetBrandByIdAsync(int id, CancellationToken cancellationToken);
    Task<Themes?> GetThemeByIdAsync(int id, CancellationToken cancellationToken);
    Task<Series?> GetSeriesByIdAndThemeIdAsync(
        int seriesId,
        int themeId,
        CancellationToken cancellationToken);
    Task<List<Materials>> GetMaterialsByIdsAsync(List<int> ids, CancellationToken cancellationToken);
}
