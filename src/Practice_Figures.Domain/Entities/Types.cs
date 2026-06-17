namespace Practice_Figures.Domain.Entities;

public class Types
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}
