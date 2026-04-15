using Microsoft.EntityFrameworkCore;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Infrastructure.DataAccess;

public class TemplateApiDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateApiDbContext).Assembly);
    }
}