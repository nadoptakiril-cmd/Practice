using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Application.Figures.DTOs;

public static class FigureMapper
{
    public static FigureResponseDto ToResponseDto(this Figure figure)
    {
        return new FigureResponseDto
        {
            Id = figure.Id,
            Name = figure.Name,
            Height = figure.Height,
            ReleaseYear = figure.ReleaseYear,
            Price = figure.Price,
            CreatedAt = figure.CreatedAt,
            UpdatedAt = figure.UpdatedAt,
            TypeId = figure.TypeId,
            Type = figure.Type is null ? null : ToLookupDto(figure.Type.Id, figure.Type.Name),
            BrandId = figure.BrandId,
            Brand = figure.Brand is null ? null : ToLookupDto(figure.Brand.Id, figure.Brand.Name),
            ThemeId = figure.ThemeId,
            Theme = figure.Theme is null ? null : ToLookupDto(figure.Theme.Id, figure.Theme.Name),
            SeriesId = figure.SeriesId,
            Series = figure.Series is null
                ? null
                : new SeriesResponseDto
                {
                    Id = figure.Series.Id,
                    Name = figure.Series.Name,
                    ThemeId = figure.Series.ThemeId
                },
            Materials = figure.Materials
                .Select(material => ToLookupDto(material.Id, material.Name))
                .ToList(),
            Images = figure.Images
                .Select(image => new ImageResponseDto
                {
                    Id = image.Id,
                    FigureId = image.FigureId,
                    ImageUrl = image.ImageUrl
                })
                .ToList()
        };
    }

    private static LookupDto ToLookupDto(int id, string name) =>
        new()
        {
            Id = id,
            Name = name
        };
}
