using System.Threading.Tasks;
using dl.wm.suite.common.messaging.Messaging.Telemetry;
using dl.wm.suite.telemetry.api.Proxies;
using MassTransit;

namespace dl.wm.suite.telemetry.api.Messaging.Consumers
{
    public class TelemetryDeliveredEventConsumer : IConsumer<TelemetryDeliveredEvent>
    {
        public async Task Consume(ConsumeContext<TelemetryDeliveredEvent> context)
        {
            await Task.Run(() => EventServer.GetServer.RaiseTelemetryRowDetection(context.CorrelationId, context.Message.Imei, context.Message.Payload));
        }
    }
}