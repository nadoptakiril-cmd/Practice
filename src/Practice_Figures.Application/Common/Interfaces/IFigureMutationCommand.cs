namespace Practice_Figures.Application.Common.Interfaces;

public interface IFigureMutationCommand
{
    int TypeId { get; }
    int BrandId { get; }
    int ThemeId { get; }
    int? SeriesId { get; }
    List<int> MaterialIds { get; }
}
