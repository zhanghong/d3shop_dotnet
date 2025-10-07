using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record DeleteDepartmentCommand(DepartmentId DepartmentId) : ICommand;

public class DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    : ICommandHandler<DeleteDepartmentCommand>
{
    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var depart = await departmentRepository.GetAsync(request.DepartmentId, cancellationToken) ??
                     throw new KnownException($"部门不存在，DepartmentId={request.DepartmentId}");


        depart.Delete();
    }
}
