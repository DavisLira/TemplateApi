using TemplateApi.Domain.Enums;

namespace TemplateApi.Domain.Entities;

public class User : EntitieBase
{
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }
    public string Role { get; set; } = Roles.MEMBER;
}