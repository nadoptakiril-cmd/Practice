using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class FiguresController : ControllerBase
{
    private readonly AppDbContext _context;

    public FiguresController(AppDbContext context) => _context = context;

    private IQueryable<Figure> FiguresWithRelations => _context.Figures
        .Include(f => f.Type)
        .Include(f => f.Brand)
        .Include(f => f.Theme)
        .Include(f => f.Series)
        .Include(f => f.Images)
        .Include(f => f.Materials);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Figure>>> GetAll() =>
        await FiguresWithRelations.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Figure>> Create(FigureCreateDto dto)
    {
        var (missing, materials) = await ValidateDto(dto);

        if (missing.Count > 0)
            return BadRequest($"Not found: {string.Join("; ", missing)}");

        var figure = dto.Figure;
        var now = DateTime.UtcNow;
        figure.CreatedAt = now;
        figure.UpdatedAt = now;
        figure.Materials = materials;
        figure.Images = dto.ImageUrls
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select(url => new Images { ImageUrl = url })
            .ToList();

        _context.Figures.Add(figure);
        await _context.SaveChangesAsync();

        return Ok(figure);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FigureCreateDto dto)
    {
        var existingFigure = await _context.Figures
            .Include(f => f.Materials)
            .Include(f => f.Images)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (existingFigure is null) return NotFound();

        var (missing, materials) = await ValidateDto(dto);
        if (missing.Count > 0)
            return BadRequest($"Not found: {string.Join("; ", missing)}");

        existingFigure.Name = dto.Figure.Name;
        existingFigure.Height = dto.Figure.Height;
        existingFigure.ReleaseYear = dto.Figure.ReleaseYear;
        existingFigure.Price = dto.Figure.Price;
        existingFigure.TypeId = dto.Figure.TypeId;
        existingFigure.BrandId = dto.Figure.BrandId;
        existingFigure.ThemeId = dto.Figure.ThemeId;
        existingFigure.SeriesId = dto.Figure.SeriesId;
        existingFigure.UpdatedAt = DateTime.UtcNow;

        existingFigure.Materials.Clear();
        foreach (var material in materials)
            existingFigure.Materials.Add(material);

        _context.Images.RemoveRange(existingFigure.Images);
        existingFigure.Images = dto.ImageUrls
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select(url => new Images { ImageUrl = url })
            .ToList();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var figure = await _context.Figures.FindAsync(id);
        if (figure is null) return NotFound();

        _context.Figures.Remove(figure);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<(List<string> Missing, List<Materials> Materials)> ValidateDto(FigureCreateDto dto)
    {
        var missing = new List<string>();

        if (!await _context.Types.AnyAsync(t => t.Id == dto.Figure.TypeId))
            missing.Add($"type_id={dto.Figure.TypeId}");

        if (!await _context.Brands.AnyAsync(b => b.Id == dto.Figure.BrandId))
            missing.Add($"brand_id={dto.Figure.BrandId}");

        if (!await _context.Themes.AnyAsync(t => t.Id == dto.Figure.ThemeId))
            missing.Add($"theme_id={dto.Figure.ThemeId}");

        if (dto.Figure.SeriesId.HasValue &&
            !await _context.Series.AnyAsync(s =>
                s.Id == dto.Figure.SeriesId.Value &&
                s.ThemeId == dto.Figure.ThemeId))
        {
            missing.Add($"series_id={dto.Figure.SeriesId} for theme_id={dto.Figure.ThemeId}");
        }

        var materials = await _context.Materials
            .Where(m => dto.MaterialIds.Contains(m.Id))
            .ToListAsync();

        var missingMaterialIds = dto.MaterialIds.Except(materials.Select(m => m.Id)).ToList();
        if (missingMaterialIds.Count > 0)
            missing.Add($"materialIds={string.Join(", ", missingMaterialIds)}");

        return (missing, materials);
    }
}
