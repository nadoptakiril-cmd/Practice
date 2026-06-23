using Microsoft.EntityFrameworkCore;
using Practice_Figures.Core.Entities;
using Practice_Figures.Core.Interfaces;
using Practice_Figures.Infrastructure.Data;

namespace Practice_Figures.Infrastructure.Repositories;

public class FigureRepository : IFigureRepository
{
    private readonly AppDbContext _context;

    public FigureRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Figure>> GetAllWithDetailsAsync(CancellationToken cancellationToken)
    {
        return FiguresWithDetails().ToListAsync(cancellationToken);
    }

    public Task<Figure?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken)
    {
        return FiguresWithDetails()
            .FirstOrDefaultAsync(figure => figure.Id == id, cancellationToken);
    }

    public void Add(Figure figure)
    {
        _context.Figures.Add(figure);
    }

    public void RemoveImages(IEnumerable<Images> images)
    {
        _context.Images.RemoveRange(images);
    }

    private IQueryable<Figure> FiguresWithDetails()
    {
        return _context.Figures
            .Where(f => !f.IsDeleted)
            .Include(figure => figure.Type)
            .Include(figure => figure.Brand)
            .Include(figure => figure.Theme)
            .Include(figure => figure.Series)
            .Include(figure => figure.Images)
            .Include(figure => figure.Materials)
            .AsSplitQuery();
    }
}
