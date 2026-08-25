using RMS.Domain.Entities;
using MassTransit;

namespace RMS.Application.Events
{
    public class OrderUpdatedEvent : IDomainEvent
    {
        public Order OldOrder { get; }
        public Order NewOrder { get; }

        public OrderUpdatedEvent(Order oldOrder, Order newOrder)
        {
            OldOrder = oldOrder;
            NewOrder = newOrder;
        }
    }
}
