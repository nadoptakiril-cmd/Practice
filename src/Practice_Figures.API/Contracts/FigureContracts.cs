using System.Text.Json.Serialization;
using Practice_Figures.Application.Figures.Commands;

namespace Practice_Figures.API.Contracts;

public class FigureRequest
{
    public string Name { get; set; } = string.Empty;
    public double Height { get; set; }

    [JsonPropertyName("release_year")]
    public int ReleaseYear { get; set; }

    public decimal Price { get; set; }

    [JsonPropertyName("type_id")]
    public int TypeId { get; set; }

    [JsonPropertyName("brand_id")]
    public int BrandId { get; set; }

    [JsonPropertyName("theme_id")]
    public int ThemeId { get; set; }

    [JsonPropertyName("series_id")]
    public int? SeriesId { get; set; }
}

public class FigureMutationRequest
{
    public FigureRequest Figure { get; set; } = new();

    [JsonPropertyName("materialIds")]
    public List<int> MaterialIds { get; set; } = new();

    [JsonPropertyName("imageUrls")]
    public List<string> ImageUrls { get; set; } = new();
}

public static class FigureContractMapper
{
    public static CreateFigureCommand ToCreateCommand(this FigureMutationRequest request)
    {
        return new CreateFigureCommand
        {
            Name = request.Figure.Name,
            Height = request.Figure.Height,
            ReleaseYear = request.Figure.ReleaseYear,
            Price = request.Figure.Price,
            TypeId = request.Figure.TypeId,
            BrandId = request.Figure.BrandId,
            ThemeId = request.Figure.ThemeId,
            SeriesId = request.Figure.SeriesId,
            MaterialIds = request.MaterialIds,
            ImageUrls = request.ImageUrls
        };
    }

    public static UpdateFigureCommand ToUpdateCommand(this FigureMutationRequest request, int id)
    {
        return new UpdateFigureCommand
        {
            Id = id,
            Name = request.Figure.Name,
            Height = request.Figure.Height,
            ReleaseYear = request.Figure.ReleaseYear,
            Price = request.Figure.Price,
            TypeId = request.Figure.TypeId,
            BrandId = request.Figure.BrandId,
            ThemeId = request.Figure.ThemeId,
            SeriesId = request.Figure.SeriesId,
            MaterialIds = request.MaterialIds,
            ImageUrls = request.ImageUrls
        };
    }

}
