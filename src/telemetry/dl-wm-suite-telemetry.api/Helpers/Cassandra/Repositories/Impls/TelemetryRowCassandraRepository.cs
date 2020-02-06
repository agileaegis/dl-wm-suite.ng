using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Configurations.Installers;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Models;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts;
using Cassandra;
using Cassandra.Mapping;
using Serilog;

namespace dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Impls
{
    public class TelemetryRowCassandraRepository : ITelemetryRowCassandraRepository 
    {
        public IJSONSerializer Serializer { get; }
        private readonly IMapper _mapper;

        public TelemetryRowCassandraRepository (IJSONSerializer serializer)
        {
            Serializer = serializer;
            _mapper = new Mapper(CassandraInitializer.session);
        }

        public async Task<TelemetryRow> AddTelemetryRow(TelemetryRow telemetryRow)
        {
            string json = this.Serializer.SerializeObject(telemetryRow);

            string key = Guid.NewGuid().ToString();

            telemetryRow.Id = key;

            try
            {
                var preparedStatement = CassandraInitializer.session.Prepare("INSERT INTO aegiswmtelemetry.telemetryrow" +  " JSON = ?;");
                var boundStatement = preparedStatement.Bind(json);
                CassandraInitializer.session.Execute(boundStatement);
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Create New TelemetryRow Entry Cassandra: Details: {e.Message}" +
                    "--AddTelemetryRow--  @NotComplete@ [TelemetryRowCassandraRepository]. " +
                    "Message: Error Creating New TelemetryRow Entry");
            }

            return await GetTelemetryRow(key);
        }
        public async Task<TelemetryRow> UpdateTelemetryRow(string key, TelemetryRow telemetryRow)
        {
            string json = this.Serializer.SerializeObject(telemetryRow);

            var preparedStatement = CassandraInitializer.session.Prepare("UPDATE aegiswmtelemetry.telemetryrow" +  " SET JSON = ? WHERE id = ?;");

            var boundStatement = preparedStatement.Bind(json, key);

            CassandraInitializer.session.Execute(boundStatement);
            return await GetTelemetryRow(key);
        }

        public async Task<TelemetryRow> GetTelemetryRow(string key)
        {
            var preparedStatement = CassandraInitializer.session.Prepare("SELECT JSON FROM aegiswmtelemetry.telemetryrow" + " WHERE id = ?;");

            var boundStatement = preparedStatement.Bind(key);

            RowSet results = CassandraInitializer.session.Execute(boundStatement);

            if (!results.IsExhausted())
            {
                return Serializer.DeserializeObject<TelemetryRow>(results.GetRows().First().GetValue<string>("row"));
            } 
            else
            {
                return await default(Task<TelemetryRow>);
            }
        }

        public void Save(string key, TelemetryRow row)
        {
            string json = this.Serializer.SerializeObject(row);

            var preparedStatement = CassandraInitializer.session.Prepare("UPDATE aegiswmtelemetry.telemetryrow SET json = ? WHERE id = ?;");

            var boundStatement = preparedStatement.Bind(json, key);

            CassandraInitializer.session.Execute(boundStatement);
        }
        public void Save(string key, string rowJson)
        {
            string json = rowJson;

            var preparedStatement = CassandraInitializer.session.Prepare("UPDATE aegiswmtelemetry.telemetryrow SET json = ? WHERE id = ?;");

            var boundStatement = preparedStatement.Bind(json, key);

            CassandraInitializer.session.Execute(boundStatement);
        }

        public TelemetryRow Get(string key)
        {
            var preparedStatement = CassandraInitializer.session.Prepare("SELECT json FROM aegiswmtelemetry.telemetryrow WHERE id = ?;");

            var boundStatement = preparedStatement.Bind(key);

            RowSet results = CassandraInitializer.session.Execute(boundStatement);

            if (!results.IsExhausted())
            {
                return this.Serializer.DeserializeObject<TelemetryRow>(results.GetRows().First().GetValue<string>("json"));
            }
            else
            {
                return default(TelemetryRow);
            }
        }

        public IEnumerable<TelemetryRow> GetAll()
        {
            var preparedStatement = CassandraInitializer.session.Prepare("SELECT json FROM aegiswmtelemetry.telemetryrow;");

            var boundStatement = preparedStatement.Bind();

            RowSet results = CassandraInitializer.session.Execute(boundStatement);

            if (!results.IsExhausted())
            {
                foreach (var row in results.GetRows())
                {
                    yield return this.Serializer.DeserializeObject<TelemetryRow>(row.GetValue<string>("json"));
                }
            }
            else
            {
                yield return default(TelemetryRow);
            }
        }
    }
}
