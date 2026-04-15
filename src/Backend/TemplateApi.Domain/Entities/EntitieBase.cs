namespace TemplateApi.Domain.Entities;

public class EntitieBase
{
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}