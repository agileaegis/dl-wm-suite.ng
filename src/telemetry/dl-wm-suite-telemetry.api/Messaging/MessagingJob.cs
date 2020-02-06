using System;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Models;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts;
using dl.wm.suite.telemetry.api.Messaging.Events.EventArgs;
using dl.wm.suite.telemetry.api.Messaging.Events.Listeners;
using dl.wm.suite.telemetry.api.Proxies;
using Serilog;

namespace dl.wm.suite.telemetry.api.Messaging
{
    public interface IMessagingJob
    {
        void RegisterToConsumers();
    }

    public class MessagingJob : IMessagingJob, ITelemetryRowDetectionActionListener
    {
        public IJSONSerializer Serializer { get; }
        private readonly ITelemetryRowCassandraRepository _telemetryRowCassandraRepository;

        public MessagingJob(ITelemetryRowCassandraRepository telemetryRowCassandraRepository, IJSONSerializer serializer)
        {
            Serializer = serializer;
            _telemetryRowCassandraRepository = telemetryRowCassandraRepository;
        }

        public void Update(object sender, TelemetryRowEventArgs e)
        {
            var payload = Serializer.DeserializeObject<TelemetryRow>(e.Payload);

            payload.Id = Guid.NewGuid().ToString();
            payload.Imei = e.Imei;
            payload.CorrelationId = e.CorrelationId.ToString();
            payload.CreatedDate = DateTime.Now;
            payload.CreatedDateUtc = DateTime.UtcNow;

            _telemetryRowCassandraRepository.Save(payload.Id, payload);
        }

        public void RegisterToConsumers()
        {
            EventServer.GetServer.Attach((ITelemetryRowDetectionActionListener)this);
        }
    }
}
