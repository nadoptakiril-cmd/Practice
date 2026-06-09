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
        .Include(f => f.Materials);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Figure>>> GetAll() =>
        await FiguresWithRelations.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Figure>> Create(FigureCreateDto dto)
    {
        var missing = new List<string>();

        if (!await _context.Types.AnyAsync(t => t.Id == dto.Figure.TypeId))
            missing.Add($"type_id={dto.Figure.TypeId}");

        if (!await _context.Brands.AnyAsync(b => b.Id == dto.Figure.BrandId))
            missing.Add($"brand_id={dto.Figure.BrandId}");

        if (!await _context.Themes.AnyAsync(t => t.Id == dto.Figure.ThemeId))
            missing.Add($"theme_id={dto.Figure.ThemeId}");

        var materials = await _context.Materials
            .Where(m => dto.MaterialIds.Contains(m.Id))
            .ToListAsync();

        var missingMaterialIds = dto.MaterialIds.Except(materials.Select(m => m.Id)).ToList();
        if (missingMaterialIds.Count > 0)
            missing.Add($"materialIds={string.Join(", ", missingMaterialIds)}");

        if (missing.Count > 0)
            return BadRequest($"Not found: {string.Join("; ", missing)}");

        var figure = dto.Figure;
        figure.Materials = materials;

        _context.Figures.Add(figure);
        await _context.SaveChangesAsync();

        return Ok(figure);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Figure figure)
    {
        if (id != figure.Id) return BadRequest();

        var existingFigure = await _context.Figures.FindAsync(id);
        if (existingFigure is null) return NotFound();

        _context.Entry(existingFigure).CurrentValues.SetValues(figure);
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
}
