namespace Practice_Figures.Core.Entities;

public class Themes
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
    public ICollection<Series> Series { get; set; } = new List<Series>();
}
