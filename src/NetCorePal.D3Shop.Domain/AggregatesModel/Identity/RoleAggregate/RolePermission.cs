using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate
{
    public class RolePermission
    {
        protected RolePermission()
        {
        }

        public RoleId RoleId { get; internal set; } = default!;
        public string PermissionCode { get; private set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public UpdateTime UpdatedAt { get; private set; } = new UpdateTime(DateTimeOffset.UtcNow);
        public Deleted IsDeleted { get; private set; } = new Deleted(false);
        public DeletedTime? DeletedAt { get; private set; }


        //public MenuId MenuId { get; internal set; } = default!;
        public MenuId MenuId { get; internal set; } = default!;

        public RolePermission(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        public RolePermission(string permissionCode, MenuId menuId)
        {
            PermissionCode = permissionCode;
            MenuId = menuId;
        }
    }
}
