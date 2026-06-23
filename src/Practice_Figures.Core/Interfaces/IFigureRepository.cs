using Practice_Figures.Core.Entities;

namespace Practice_Figures.Core.Interfaces;

public interface IFigureRepository
{
    Task<List<Figure>> GetAllWithDetailsAsync(CancellationToken cancellationToken);
    Task<Figure?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken);
    void Add(Figure figure);
    void RemoveImages(IEnumerable<Images> images);
}
