namespace TemplateApi.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
    public Task<bool> ExistActiveUserWithEmail(string email);
    public Task<Entities.User?> GetByEmail(string email);
    public Task<Entities.User?> GetByIdentifier(Guid userIdentifier);
}