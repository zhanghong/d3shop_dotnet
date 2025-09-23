using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.DomainEvents
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
