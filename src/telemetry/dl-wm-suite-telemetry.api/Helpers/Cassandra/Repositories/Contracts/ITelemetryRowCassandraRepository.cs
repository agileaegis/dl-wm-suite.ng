using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Models;

namespace dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts
{
    public interface ITelemetryRowCassandraRepository

    {
        Task<TelemetryRow> AddTelemetryRow(TelemetryRow telemetryRow);
        Task<TelemetryRow> UpdateTelemetryRow(string key, TelemetryRow telemetryRow);
        Task<TelemetryRow> GetTelemetryRow(string key);

        void Save(string key, TelemetryRow row);
        TelemetryRow Get(string key);

        IEnumerable<TelemetryRow> GetAll();
    }
}