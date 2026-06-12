using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Figure
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Height { get; set; }

    [JsonPropertyName("release_year")]
    [Column("release_year")]
    public int ReleaseYear { get; set; }

    public decimal Price { get; set; }

    [JsonPropertyName("created_at")]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("type_id")]
    [Column("type_id")]
    public int TypeId { get; set; }
    public Types? Type { get; set; }

    [JsonPropertyName("brand_id")]
    [Column("brand_id")]
    public int BrandId { get; set; }
    public Brands? Brand { get; set; }

    [JsonPropertyName("theme_id")]
    [Column("theme_id")]
    public int ThemeId { get; set; }
    public Themes? Theme { get; set; }

    [JsonPropertyName("series_id")]
    [Column("series_id")]
    public int? SeriesId { get; set; }
    public Series? Series { get; set; }

    public ICollection<Materials> Materials { get; set; } = new List<Materials>();
    public ICollection<Images> Images { get; set; } = new List<Images>();
}
