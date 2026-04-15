namespace TemplateApi.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit();
}