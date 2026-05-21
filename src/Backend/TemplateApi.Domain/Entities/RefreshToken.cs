namespace TemplateApi.Domain.Entities;

public class RefreshToken : EntitieBase
{
    public long RefreshTokenId { get; set; } 
    public string Value { get; set; } = string.Empty;
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}