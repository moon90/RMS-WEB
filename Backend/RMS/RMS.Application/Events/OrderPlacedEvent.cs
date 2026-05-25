using RMS.Domain.Entities;

namespace RMS.Application.Events
{
    public class OrderPlacedEvent : IDomainEvent
    {
        public Order Order { get; }

        public OrderPlacedEvent(Order order)
        {
            Order = order;
        }
    }
}
