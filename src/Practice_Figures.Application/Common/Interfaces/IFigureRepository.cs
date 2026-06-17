using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Common.Interfaces;

public interface IFigureRepository
{
    Task<List<Figure>> GetAllWithDetailsAsync(CancellationToken cancellationToken);
    Task<Figure?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken);
    void Add(Figure figure);
    void Remove(Figure figure);
    void RemoveImages(IEnumerable<Images> images);
}
