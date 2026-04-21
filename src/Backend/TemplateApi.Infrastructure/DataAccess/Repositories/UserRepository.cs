using Microsoft.EntityFrameworkCore;
using TemplateApi.Domain.Entities;
using TemplateApi.Domain.Repositories.User;

namespace TemplateApi.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly TemplateApiDbContext _dbContext;

    public UserRepository(TemplateApiDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
    public async Task<bool> ExistActiveUserWithEmail(string email) {
        return await _dbContext.Users.AnyAsync(
            user => user.Email.Equals(email)
            && user.Active);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user =>
                user.Active
                && user.Email.Equals(email));
    }
}