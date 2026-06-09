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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Figure>()
            .HasMany(f => f.Materials)
            .WithMany(m => m.Figures)
            .UsingEntity<Dictionary<string, object>>(
                "FigureMaterials",
                j => j.HasOne<Materials>().WithMany().HasForeignKey("material_id"),
                j => j.HasOne<Figure>().WithMany().HasForeignKey("figure_id"));
    }
}
