using TemplateApi.Domain.Repositories;

namespace TemplateApi.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly TemplateApiDbContext _dbContext;

    public UnitOfWork(TemplateApiDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}