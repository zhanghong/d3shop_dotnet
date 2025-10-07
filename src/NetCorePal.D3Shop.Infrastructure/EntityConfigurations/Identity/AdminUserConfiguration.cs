using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Domain;
using System.Text.Json;

namespace NetCorePal.D3Shop.Infrastructure.EntityConfigurations.Identity;

internal class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
{
    public void Configure(EntityTypeBuilder<AdminUser> builder)
    {
        builder.ToTable("editors");
        builder.HasKey(au => au.Id);
        builder.Property(au => au.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(au => au.Types).HasColumnName("types").HasDefaultValue("").HasMaxLength(30).HasComment("身份类型");
        builder.Property(au => au.Name).HasColumnName("name").HasDefaultValue("").HasMaxLength(50).HasComment("登录名");
        builder.Property(au => au.Phone).HasColumnName("phone").HasDefaultValue("").HasMaxLength(11).HasComment("手机号码");
        builder.Property(au => au.Password).HasColumnName("password").HasDefaultValue("").HasMaxLength(200).HasComment("密码");
        builder.Property(au => au.RealName).HasColumnName("real_name").HasDefaultValue("").HasMaxLength(20).HasComment("真实姓名");
        builder.Property(au => au.Email).HasColumnName("email").HasDefaultValue("").HasMaxLength(50).HasComment("邮箱");
        builder.Property(au => au.Status).HasColumnName("status").HasDefaultValue(1).HasComment("状态");
        builder.Property(au => au.CreatedAt).HasColumnName("created_at").HasComment("创建时间");
        builder.Property(au => au.UpdatedAt).HasColumnName("updated_at").HasComment("更新时间");
        builder.Property(au => au.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(new Deleted(false)).HasComment("是否删除");
        builder.Property(au => au.DeletedAt).HasColumnName("deleted_at").HasComment("删除时间");

        // 配置 AdminUser 与 AdminUserRole 的一对多关系
        builder.HasMany(au => au.Roles)
            .WithOne()
            .HasForeignKey(aur => aur.AdminUserId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Navigation(au => au.Roles).AutoInclude();

        // 配置 AdminUser 与 AdminUserPermission 的一对多关系
        builder.HasMany(au => au.Permissions)
            .WithOne()
            .HasForeignKey(aup => aup.AdminUserId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Navigation(au => au.Permissions).AutoInclude();

        //配置 AdminUser 与 UserDept 的一对多关系
        builder.HasMany(au => au.UserDepts)
            .WithOne()
            .HasForeignKey(aup => aup.AdminUserId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Navigation(au => au.UserDepts).AutoInclude();

        builder.HasQueryFilter(au => !au.IsDeleted);
    }
}

internal class AdminUserRoleConfiguration : IEntityTypeConfiguration<AdminUserRole>
{
    public void Configure(EntityTypeBuilder<AdminUserRole> builder)
    {
        builder.ToTable("admin_user_roles");
        builder.HasKey(aur => new { aur.AdminUserId, aur.RoleId });
    }
}

internal class UserDeptConfiguration : IEntityTypeConfiguration<UserDept>
{
    public void Configure(EntityTypeBuilder<UserDept> builder)
    {
        builder.ToTable("user_departments");
        builder.HasKey(aur => new { aur.AdminUserId, aur.DeptId });
    }
}

internal class AdminUserPermissionConfiguration : IEntityTypeConfiguration<AdminUserPermission>
{
    public void Configure(EntityTypeBuilder<AdminUserPermission> builder)
    {
        builder.ToTable("admin_user_permissions");
        builder.HasKey(aup => new { aup.AdminUserId, aup.PermissionCode });
        builder.Property(p => p.SourceRoleIds).HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<RoleId>>(v, (JsonSerializerOptions?)null) ??
                 new List<RoleId>(),
            new ValueComparer<IList<RoleId>>(
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }
}
