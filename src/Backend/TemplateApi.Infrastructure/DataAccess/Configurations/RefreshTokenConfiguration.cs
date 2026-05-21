using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Infrastructure.DataAccess.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => e.RefreshTokenId);

        builder.Property(e => e.Value)
               .IsRequired();
               
        builder.Property(e => e.CreatedAt)
               .IsRequired()
               .HasColumnName("CreatedOn");

        builder.Property(e => e.Active)
               .IsRequired()
               .HasDefaultValue(true);

        builder.HasOne(e => e.User)
               .WithMany() 
               .HasForeignKey(e => e.UserId)
               .HasConstraintName("FK_RefreshTokens_User_Id")
               .IsRequired();
    }
}