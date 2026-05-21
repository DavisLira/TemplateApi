using Microsoft.EntityFrameworkCore;
using TemplateApi.Domain.Entities;
using TemplateApi.Domain.Repositories.Token;

namespace TemplateApi.Infrastructure.DataAccess.Repositories;
public class TokenRepository(
    TemplateApiDbContext dbContext
) : ITokenRepository
{
    private readonly TemplateApiDbContext _dbContext = dbContext;

    public async Task<RefreshToken?> Get(string refreshToken)
    {
        return await _dbContext
            .RefreshTokens
            .AsNoTracking()
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    }

    public async Task SaveNewRefreshToken(RefreshToken refreshToken)
    {
        var tokens = _dbContext.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);

        // Mudar no caso de ter vários refresh tokens para diferentes aplicações
        _dbContext.RefreshTokens.RemoveRange(tokens);

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }
}