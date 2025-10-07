using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record UpdateUserDepartmentInfoCommand(AdminUserId AdminUserId, DepartmentId DepartmentId, string DepartmentName) : ICommand;

public class UpdateUserDepartmentInfoCommandHandler(AdminUserRepository adminUserRepository)
    : ICommandHandler<UpdateUserDepartmentInfoCommand>
{
    public async Task Handle(UpdateUserDepartmentInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                   throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        user.SetUserDepartments(request.DepartmentId, request.DepartmentName);
    }
}
