using System.Threading.Tasks;

namespace RMS.Application.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
    }
}
