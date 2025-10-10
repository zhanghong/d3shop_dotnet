using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.UserAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(c => c.Name).HasColumnName("name").HasDefaultValue("").HasMaxLength(50).HasComment("登录名");
        builder.Property(c => c.Avatar).HasColumnName("avatar").HasDefaultValue("").HasMaxLength(150).HasComment("用户头像");
        builder.Property(c => c.Phone).HasColumnName("phone").HasDefaultValue("").HasMaxLength(11).HasComment("手机号码");
        builder.Property(c => c.Email).HasColumnName("email").HasDefaultValue("").HasMaxLength(50).HasComment("邮箱");
        builder.Property(c => c.PasswordHash).HasColumnName("password_hash").HasDefaultValue("").HasMaxLength(200).HasComment("密码");
        builder.Property(c => c.PasswordSalt).HasColumnName("password_salt").HasDefaultValue("").HasMaxLength(5).HasComment("密码Salt");
        builder.Property(c => c.PasswordFailedTimes).HasColumnName("password_failed_times").HasDefaultValue(0).HasComment("登录失败次数");
        builder.Property(c => c.IsDisabled).HasColumnName("is_disabled").HasDefaultValue(false).HasComment("是否禁用");
        builder.Property(c => c.DisabledAt).HasColumnName("disabled_at").HasComment("禁用时间");
        builder.Property(c => c.DisabledReason).HasDefaultValue("").HasMaxLength(500).HasColumnName("disabled_reason").HasComment("禁用原因");
        builder.Property(c => c.IsTwoFactorEnabled).HasColumnName("is_two_factor_enabled").HasDefaultValue(false).HasComment("是否双认证");
        builder.Property(c => c.LastLoginAt).HasColumnName("last_login_at").HasComment("最后登录时间");
        builder.Property(c => c.CreatedAt).HasColumnName("created_at").HasComment("创建时间");
        builder.Property(c => c.UpdatedAt).HasColumnName("updated_at").HasComment("更新时间");
        builder.Property(c => c.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(new Deleted(false)).HasComment("是否删除");
        builder.Property(c => c.DeletedAt).HasColumnName("deleted_at").HasComment("删除时间");
    }
}
