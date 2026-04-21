using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Domain.Entities;
using TemplateApi.Domain.Security.Tokens;
using TemplateApi.Domain.Services.LoggedUser;
using TemplateApi.Infrastructure.DataAccess;

namespace TemplateApi.Infrastructure.Services;

public class LoggedUser(
    TemplateApiDbContext dbContext,
    ITokenProvider tokenProvider
) : ILoggedUser
{
    private readonly TemplateApiDbContext _dbContext = dbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    public async Task<User> User()
    {
        var token = _tokenProvider.Value();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        var userIdentifier = Guid.Parse(identifier);
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync( user =>
                user.Active
                && user.UserIdentifier == userIdentifier
            );

    }
}