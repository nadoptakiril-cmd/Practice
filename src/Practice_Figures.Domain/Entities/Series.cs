namespace Practice_Figures.Domain.Entities;

public class Series
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int ThemeId { get; set; }
    public Themes? Theme { get; set; }

    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}
