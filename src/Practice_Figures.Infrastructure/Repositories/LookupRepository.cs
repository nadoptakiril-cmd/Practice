using Microsoft.EntityFrameworkCore;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Domain.Entities;
using Practice_Figures.Infrastructure.Data;

namespace Practice_Figures.Infrastructure.Repositories;

public class LookupRepository : ILookupRepository
{
    private readonly AppDbContext _context;

    public LookupRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> TypeExistsAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Types.AnyAsync(type => type.Id == id, cancellationToken);
    }

    public Task<bool> BrandExistsAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Brands.AnyAsync(brand => brand.Id == id, cancellationToken);
    }

    public Task<bool> ThemeExistsAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Themes.AnyAsync(theme => theme.Id == id, cancellationToken);
    }

    public Task<bool> SeriesBelongsToThemeAsync(
        int seriesId,
        int themeId,
        CancellationToken cancellationToken)
    {
        return _context.Series.AnyAsync(
            series => series.Id == seriesId && series.ThemeId == themeId,
            cancellationToken);
    }

    public Task<List<Materials>> GetMaterialsByIdsAsync(
        List<int> ids,
        CancellationToken cancellationToken)
    {
        return _context.Materials
            .Where(material => ids.Contains(material.Id))
            .ToListAsync(cancellationToken);
    }
}
