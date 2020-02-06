using System;
using System.Linq;
using Marten;
using Marten.Services;
using Npgsql;

namespace dl.wm.suite.telemetry.api.Helpers.Marten.Logging
{
    class DeviceMartenLogger : IMartenLogger, IMartenSessionLogger
    {
        public IMartenSessionLogger StartSession(IQuerySession session)
        {
            return this;
        }

        public void SchemaChange(string sql)
        {
            Console.WriteLine("Executing schema change with the following DDL:");
            Console.WriteLine(sql);
            Console.WriteLine();
        }

        public void LogSuccess(NpgsqlCommand command)
        {
            Console.WriteLine($"CommandText={command.CommandText}");

            Console.WriteLine("Parameters");
            foreach (NpgsqlParameter parameter in command.Parameters)
            {
                Console.WriteLine($"Parameter '{parameter.ParameterName}' =  '{parameter.Value}'");
            }

            Console.WriteLine("SQL Statements");
            foreach (NpgsqlStatement statement in command.Statements)
            {
                Console.WriteLine(statement.SQL);
            }
        }

        public void LogFailure(NpgsqlCommand command, Exception ex)
        {
            Console.WriteLine("Postgresql command failed");
            Console.WriteLine(command.CommandText);
            Console.WriteLine(ex);
        }

        public void RecordSavedChanges(IDocumentSession session, IChangeSet commit)
        {
            var lastCommit = commit;
            Console.WriteLine(
                $"Persisted {lastCommit.Updated.Count()} updates, {lastCommit.Inserted.Count()} inserts, and {lastCommit.Deleted.Count()} deletions");
        }
    }
}