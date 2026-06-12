using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Images
{
    public int Id { get; set; }

    [JsonPropertyName("figure_id")]
    [Column("figure_id")]
    public int FigureId { get; set; }

    [JsonPropertyName("image_url")]
    [Column("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonIgnore]
    public Figure? Figure { get; set; }
}
