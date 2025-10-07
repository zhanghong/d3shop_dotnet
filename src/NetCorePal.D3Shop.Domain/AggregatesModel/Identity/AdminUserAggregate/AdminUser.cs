using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;

// ReSharper disable VirtualMemberCallInConstructor

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public partial record AdminUserId : IGuidStronglyTypedId;

    public class AdminUser : Entity<AdminUserId>, IAggregateRoot
    {
        public string Types { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string RealName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        /// <summary>
        /// 0:已禁用  1:已启用
        /// </summary>
        public int Status { get; private set; } = 1;
        public DateTimeOffset CreatedAt { get; init; }
        public UpdateTime UpdatedAt { get; private set; } = new UpdateTime(DateTimeOffset.UtcNow);
        public Deleted IsDeleted { get; private set; } = new Deleted(false);
        public DeletedTime? DeletedAt { get; private set; }

        public virtual ICollection<AdminUserRole> Roles { get; } = [];
        public virtual ICollection<UserDepartment> UserDepartments { get; } = [];
        public virtual ICollection<AdminUserPermission> Permissions { get; } = [];

        protected AdminUser()
        {
        }

        public AdminUser(string name, string phone, string password,
            IEnumerable<AdminUserRole> roles, IEnumerable<AdminUserPermission> permissions, string realName, int status, string email)
        {
            CreatedAt = DateTimeOffset.Now;
            Name = name;
            Phone = phone;
            Password = password;
            RealName = realName;
            Status = status;
            Email = email;
            foreach (var adminUserRole in roles)
            {
                Roles.Add(adminUserRole);
            }

            foreach (var adminUserPermission in permissions)
            {
                Permissions.Add(adminUserPermission);
            }
        }

        public void UpdateRoleInfo(RoleId roleId, string roleName)
        {
            var savedRole = Roles.FirstOrDefault(r => r.RoleId == roleId);
            savedRole?.UpdateRoleInfo(roleName);
        }

        public void SetUserDepartments(DepartmentId departmentId, string departmentName)
        {
            var savedDepartment = UserDepartments.FirstOrDefault(r => r.DepartmentId == departmentId);
            savedDepartment?.UpdateDepartmentInfo(departmentName);
        }

        /// <summary>
        /// 添加用户到部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="departmentName">部门名称</param>
        public void AddUserDepartment(DepartmentId departmentId, string departmentName)
        {
            // 检查用户是否已在该部门
            if (UserDepartments.Any(d => d.DepartmentId == departmentId))
            {
                return;
            }

            var userDepartment = new UserDepartment(departmentId, departmentName);
            UserDepartments.Add(userDepartment);

            // 添加领域事件，通知部门用户数量增加
            AddDomainEvent(new UserDepartmentChangedDomainEvent(userDepartment));
        }

        /// <summary>
        /// 从部门中移除用户
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        public void RemoveUserDepartment(DepartmentId departmentId)
        {
            var userDepartment = UserDepartments.FirstOrDefault(d => d.DepartmentId == departmentId);
            if (userDepartment == null)
            {
                return;
            }

            UserDepartments.Remove(userDepartment);
            // 添加领域事件，通知部门用户数量减少
            AddDomainEvent(new UserDepartmentChangedDomainEvent(userDepartment));
        }

        public void UpdateRoles(IEnumerable<AdminUserRole> rolesToBeAssigned,
            IEnumerable<AdminUserPermission> permissions)
        {
            var currentRoleMap = Roles.ToDictionary(r => r.RoleId);
            var targetRoleMap = rolesToBeAssigned.ToDictionary(r => r.RoleId);

            var roleIdsToRemove = currentRoleMap.Keys.Except(targetRoleMap.Keys);
            foreach (var roleId in roleIdsToRemove)
            {
                Roles.Remove(currentRoleMap[roleId]);
                RemoveRolePermissions(roleId);
            }

            var roleIdsToAdd = targetRoleMap.Keys.Except(currentRoleMap.Keys);
            foreach (var roleId in roleIdsToAdd)
            {
                var targetRole = targetRoleMap[roleId];
                Roles.Add(targetRole);
            }

            AddPermissions(permissions);
        }

        public void UpdateRolePermissions(RoleId roleId, IEnumerable<AdminUserPermission> newPermissions)
        {
            RemoveRolePermissions(roleId);
            AddPermissions(newPermissions);
        }

        private void AddPermissions(IEnumerable<AdminUserPermission> permissions)
        {
            foreach (var permission in permissions)
            {
                var existedPermission = Permissions.SingleOrDefault(p => p.PermissionCode == permission.PermissionCode);
                if (existedPermission is not null)
                {
                    foreach (var permissionSourceRoleId in permission.SourceRoleIds)
                        existedPermission.AddSourceRoleId(permissionSourceRoleId);
                }
                else
                {
                    Permissions.Add(permission);
                }
            }
        }

        private void RemoveRolePermissions(RoleId roleId)
        {
            foreach (var permission in Permissions.Where(
                             p => p.SourceRoleIds.Remove(roleId) &&
                                  p.SourceRoleIds.Count == 0)
                         .ToArray())
            {
                Permissions.Remove(permission);
            }
        }

        public void SetSpecificPermissions(IEnumerable<AdminUserPermission> permissionsToBeAssigned)
        {
            var currentSpecificPermissionMap =
                Permissions.Where(p => p.SourceRoleIds.Count == 0).ToDictionary(p => p.PermissionCode);
            var newSpecificPermissionMap = permissionsToBeAssigned.ToDictionary(p => p.PermissionCode);

            var permissionCodesToRemove = currentSpecificPermissionMap.Keys.Except(newSpecificPermissionMap.Keys);
            foreach (var permissionCode in permissionCodesToRemove)
            {
                var permission = currentSpecificPermissionMap[permissionCode];
                Permissions.Remove(permission);
            }

            var permissionCodesToAdd = newSpecificPermissionMap.Keys.Except(currentSpecificPermissionMap.Keys);
            foreach (var permissionCode in permissionCodesToAdd)
            {
                if (Permissions.Any(p => p.PermissionCode == permissionCode))
                    throw new KnownException("权限重复！");
                Permissions.Add(newSpecificPermissionMap[permissionCode]);
            }
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }

        public void Delete()
        {
            SoftDelete();
        }

        public bool IsInRole(string roleName)
        {
            return Roles.Any(r => r.RoleName == roleName);
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetPhone(string phone)
        {
            Phone = phone;
        }
    }
}
