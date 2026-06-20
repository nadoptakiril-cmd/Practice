using Microsoft.EntityFrameworkCore;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Domain.Entities;
using Practice_Figures.Infrastructure.Data;

namespace Practice_Figures.Infrastructure.Repositories;

public class FigureReferenceRepository : IFigureReferenceRepository
{
    private readonly AppDbContext _context;

    public FigureReferenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Types?> GetTypeByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Types.FirstOrDefaultAsync(type => type.Id == id, cancellationToken);
    }

    public Task<Brands?> GetBrandByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Brands.FirstOrDefaultAsync(brand => brand.Id == id, cancellationToken);
    }

    public Task<Themes?> GetThemeByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Themes.FirstOrDefaultAsync(theme => theme.Id == id, cancellationToken);
    }

    public Task<Series?> GetSeriesByIdAndThemeIdAsync(
        int seriesId,
        int themeId,
        CancellationToken cancellationToken)
    {
        return _context.Series.FirstOrDefaultAsync(
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
