
namespace Core.Common.Interfaces
{
    public interface IPublisherRabbitMQ
    {
        Task Publish(object message,string queue);
    }
}
