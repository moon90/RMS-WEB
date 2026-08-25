using RMS.Domain.Entities;
using MassTransit;

namespace RMS.Application.Events
{
    public class OrderDeletedEvent : IDomainEvent
    {
        public Order Order { get; }

        public OrderDeletedEvent(Order order)
        {
            Order = order;
        }
    }
}
