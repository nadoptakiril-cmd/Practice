namespace Practice_Figures.Core.Interfaces;

public interface IFigureMutationCommand
{
    int TypeId { get; }
    int BrandId { get; }
    int ThemeId { get; }
    int? SeriesId { get; }
    List<int> MaterialIds { get; }
}
