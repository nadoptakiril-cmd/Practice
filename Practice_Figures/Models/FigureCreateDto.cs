public class FigureCreateDto
{
    public Figure Figure { get; set; } = new();
    public List<int> MaterialIds { get; set; } = new();
}
