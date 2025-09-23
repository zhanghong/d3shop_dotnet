using NetCorePal.D3Shop.Domain.AggregatesModel.DeliverAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories
{
    public interface IDeliverRecordRepository : IRepository<DeliverRecord, DeliverRecordId>
    {

    }

    public class DeliverRecordRepository(ApplicationDbContext context) : RepositoryBase<DeliverRecord, DeliverRecordId, ApplicationDbContext>(context), IDeliverRecordRepository
    {
    }
}
