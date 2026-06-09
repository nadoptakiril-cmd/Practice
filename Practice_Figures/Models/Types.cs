using System.Text.Json.Serialization;

public class Types
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}
