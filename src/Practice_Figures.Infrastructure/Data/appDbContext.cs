using Microsoft.EntityFrameworkCore;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Domain.Entities;

namespace Practice_Figures.Infrastructure.Data;

public class AppDbContext : DbContext, IUnitOfWork
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
        modelBuilder.Entity<Figure>(entity =>
        {
            entity.ToTable(table => table.HasTrigger("TR_Figures_SetUpdatedAt"));

            entity.Property(f => f.ReleaseYear).HasColumnName("release_year");
            entity.Property(f => f.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .ValueGeneratedOnAdd();
            entity.Property(f => f.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .ValueGeneratedOnAddOrUpdate();
            entity.Property(f => f.TypeId).HasColumnName("type_id");
            entity.Property(f => f.BrandId).HasColumnName("brand_id");
            entity.Property(f => f.ThemeId).HasColumnName("theme_id");
            entity.Property(f => f.SeriesId).HasColumnName("series_id");
        });

        modelBuilder.Entity<Series>()
            .Property(s => s.ThemeId)
            .HasColumnName("theme_id");

        modelBuilder.Entity<Images>(entity =>
        {
            entity.Property(i => i.FigureId).HasColumnName("figure_id");
            entity.Property(i => i.ImageUrl).HasColumnName("image_url");
        });

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
