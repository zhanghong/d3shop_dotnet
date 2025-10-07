using MediatR;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Const;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity.Admin;
public class UserDepartmentChangedDomainEventHandler(
    IMediator mediator,
    UserDepartmentQuery userDepartmentQuery) : IDomainEventHandler<UserDepartmentChangedDomainEvent>
{
    public async Task Handle(UserDepartmentChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var departmentId = notification.UserDepartment.DepartmentId;
        var userCount = await userDepartmentQuery.GetUserCount(departmentId, cancellationToken);
        await mediator.Send(new UpdateDepartmentUserCountCommand(departmentId, userCount), cancellationToken);
    }
}
