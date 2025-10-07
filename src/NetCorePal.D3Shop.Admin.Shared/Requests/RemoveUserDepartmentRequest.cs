using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

/// <summary>
/// 从部门中移除用户请求
/// </summary>
public class RemoveUserDepartmentRequest
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public AdminUserId AdminUserId { get; set; } = default!;

    /// <summary>
    /// 部门ID
    /// </summary>
    public DepartmentId DepartmentId { get; set; } = default!;
}
