using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Figure
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Height { get; set; }

    [JsonPropertyName("production_Year")]
    [Column("production_year")]
    public int ProductionYear { get; set; }

    public decimal Price { get; set; }

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

    public ICollection<Materials> Materials { get; set; } = new List<Materials>();
}

public class Materials
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}

public class Types
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}

public class Brands
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}

public class Themes
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Figure> Figures { get; set; } = new List<Figure>();
}
