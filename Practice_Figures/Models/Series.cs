using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Series
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("theme_id")]
    [Column("theme_id")]
    public int ThemeId { get; set; }
    public Themes? Theme { get; set; }

    [JsonIgnore]
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}
