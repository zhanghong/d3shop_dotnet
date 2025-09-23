using NetCorePal.D3Shop.Admin.Shared.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Const;
using System.Security.Claims;

namespace NetCorePal.D3Shop.Web.Admin.Client.Auth;

public class ClientPermissionChecker : IPermissionChecker
{
    public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permissionCode)
    {
        var permissions = user.Claims
            .Where(claim => claim.Type == AppClaim.AdminPermission
                            && claim.Value == permissionCode);
        return Task.FromResult(permissions.Any());
    }
}
