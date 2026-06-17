namespace Practice_Figures.Domain.Entities;

public class Images
{
    public int Id { get; set; }
    public int FigureId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Figure? Figure { get; set; }
}
