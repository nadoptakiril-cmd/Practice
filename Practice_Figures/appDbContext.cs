using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Figure> Figures { get; set; } = null!;
    public DbSet<Materials> Materials { get; set; } = null!;
    public DbSet<Types> Types { get; set; } = null!;
    public DbSet<Brands> Brands { get; set; } = null!;
    public DbSet<Themes> Themes { get; set; } = null!;
    public DbSet<Series> Series { get; set; } = null!;
    public DbSet<Images> Images { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Images>()
            .HasOne(i => i.Figure)
            .WithMany(f => f.Images)
            .HasForeignKey(i => i.FigureId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Figure>()
            .HasMany(f => f.Materials)
            .WithMany(m => m.Figures)
            .UsingEntity<Dictionary<string, object>>("FigureMaterials",
                j => j.HasOne<Materials>().WithMany().HasForeignKey("material_id").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Figure>().WithMany().HasForeignKey("figure_id").OnDelete(DeleteBehavior.Cascade));
    }
}
