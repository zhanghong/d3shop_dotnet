using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;

public class UserDepartmentQuery(ApplicationDbContext applicationDbContext) : IQuery
{
    private DbSet<UserDepartment> UserDepartmentSet { get; } = applicationDbContext.UserDepartments;


    /// <summary>
    ///  获取部门下的用户数量
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> GetUserCount(DepartmentId departmentId, CancellationToken cancellationToken)
    {
        // 查询并构建初始列表
        var userCount = await UserDepartmentSet.AsNoTracking()
            .Where(d => d.DepartmentId == departmentId)
            .CountAsync();
        return userCount;
    }
}
