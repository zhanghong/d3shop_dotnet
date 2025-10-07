using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;

public class DepartmentQuery(ApplicationDbContext applicationDbContext) : IQuery
{
    private DbSet<Department> DepartmentSet { get; } = applicationDbContext.Departments;

    public async Task<bool> DoesDepartmentExist(string name, CancellationToken cancellationToken)
    {
        return await DepartmentSet.AsNoTracking()
            .AnyAsync(r => r.Name == name, cancellationToken: cancellationToken);
    }



    /// <summary>
    /// 获取所有部门
    /// </summary>
    /// <param name="queryRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<DepartmentResponse>> GetAllDepartmentsAsync(DepartmentQueryRequest queryRequest, CancellationToken cancellationToken)
    {
        // 查询并构建初始列表
        var departments = await DepartmentSet.AsNoTracking()
            .Where(dt => string.IsNullOrWhiteSpace(queryRequest.Name) || dt.Name.Contains(queryRequest.Name!))
            .Where(dt => !dt.IsDeleted)
            .OrderBy(dt => dt.Id)
            .Select(d => new DepartmentResponse(
                d.Id,
                d.Name,
                d.Description,
                d.Code,
                d.ParentId,
                d.Status,
                d.CreatedAt,
                new List<DepartmentResponse>()))
            .ToListAsync(cancellationToken);

        // 构建部门树
        var departmentMap = departments.ToDictionary(department => department.Id);
        var topLevelDepartments = new List<DepartmentResponse>();

        var rootParentId = new DepartmentId(Guid.Empty);
        foreach (var department in departments)
        {
            if (department.ParentId == rootParentId)
            {
                topLevelDepartments.Add(department);
            }
            else if (departmentMap.TryGetValue(department.ParentId, out var parent))
            {
                parent.Children.Add(department);
            }
        }
        return topLevelDepartments;
    }


    public async Task<DepartmentResponse?> GetDepartmentByIdAsync(DepartmentId id, CancellationToken cancellationToken)
    {
        return await DepartmentSet.AsNoTracking()
              .Select(d => new DepartmentResponse(
                d.Id,
                d.Name,
                d.Description,
                d.Code,
                d.ParentId,
                d.Status,
                d.CreatedAt,
                new List<DepartmentResponse>()))
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

}
