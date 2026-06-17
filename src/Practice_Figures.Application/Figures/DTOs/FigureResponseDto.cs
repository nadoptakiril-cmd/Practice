namespace Practice_Figures.Application.Figures.DTOs;

public class FigureResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Height { get; set; }

    public int ReleaseYear { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int TypeId { get; set; }
    public LookupDto? Type { get; set; }

    public int BrandId { get; set; }
    public LookupDto? Brand { get; set; }

    public int ThemeId { get; set; }
    public LookupDto? Theme { get; set; }

    public int? SeriesId { get; set; }
    public SeriesResponseDto? Series { get; set; }

    public List<LookupDto> Materials { get; set; } = new();
    public List<ImageResponseDto> Images { get; set; } = new();
}

public class LookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SeriesResponseDto : LookupDto
{
    public int ThemeId { get; set; }
}

public class ImageResponseDto
{
    public int Id { get; set; }

    public int FigureId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}
