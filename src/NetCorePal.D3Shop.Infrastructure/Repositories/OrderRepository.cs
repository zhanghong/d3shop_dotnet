using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories
{

    public interface IOrderRepository : IRepository<Order, OrderId>
    {

    }


    public class OrderRepository(ApplicationDbContext context) : RepositoryBase<Order, OrderId, ApplicationDbContext>(context), IOrderRepository
    {
    }
}
