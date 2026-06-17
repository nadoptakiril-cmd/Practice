namespace Practice_Figures.Domain.Entities;

public class Figure
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Height { get; set; }
    public int ReleaseYear { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int TypeId { get; set; }
    public Types? Type { get; set; }

    public int BrandId { get; set; }
    public Brands? Brand { get; set; }

    public int ThemeId { get; set; }
    public Themes? Theme { get; set; }

    public int? SeriesId { get; set; }
    public Series? Series { get; set; }

    public ICollection<Materials> Materials { get; set; } = new List<Materials>();
    public ICollection<Images> Images { get; set; } = new List<Images>();
}
