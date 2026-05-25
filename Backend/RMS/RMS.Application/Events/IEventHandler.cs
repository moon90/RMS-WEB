using System.Threading.Tasks;

namespace RMS.Application.Events
{
    public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
