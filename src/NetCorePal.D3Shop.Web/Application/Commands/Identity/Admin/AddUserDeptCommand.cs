using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

/// <summary>
/// 添加用户到部门命令
/// </summary>
public record AddUserDepartmentCommand(AdminUserId AdminUserId, DepartmentId DepartmentId, string DepartmentName) : ICommand;

/// <summary>
/// 添加用户到部门命令处理程序
/// </summary>
public class AddUserDepartmentCommandHandler(AdminUserRepository adminUserRepository)
    : ICommandHandler<AddUserDepartmentCommand>
{
    public async Task Handle(AddUserDepartmentCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.AddUserDepartment(request.DepartmentId, request.DepartmentName);
    }
}
