using System.Threading.Tasks;
using dl.wm.suite.common.messaging.Messaging;
using MassTransit;

namespace dl.wm.suite.telemetry.api.Messaging.Consumers
{
  public class RegistrationCompletedEventConsumer : IConsumer<RegistrationCompletedEvent>
  {
    public Task Consume(ConsumeContext<RegistrationCompletedEvent> context)
    {
      return null;
    }
    }
}
