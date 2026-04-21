using TemplateApi.Domain.Repositories;

namespace TemplateApi.Infrastructure.DataAccess;

public class UnitOfWork(TemplateApiDbContext dbContext) : IUnitOfWork
{
    private readonly TemplateApiDbContext _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}