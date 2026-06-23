namespace Practice_Figures.Core.Entities;

public class Brands
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}
