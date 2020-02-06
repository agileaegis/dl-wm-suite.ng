using System;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Domain.Contracts;
using dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Repositories.Contracts;
using dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Repositories.Impls;
using Marten;

namespace dl.wm.suite.telemetry.api.Helpers.Marten.DataAccess
{
    public class MartenDataStore : IDataStore
    {
        private readonly IDocumentSession _session;

        public MartenDataStore(IDocumentStore documentStore)
        {
            _session = documentStore.LightweightSession();
            DeviceRepository = new MartenDeviceRepository(_session);
        }

        public IDeviceRepository DeviceRepository { get; }

        public async Task CommitChanges()
        {
            await _session.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _session.Dispose();
            }

        }
    }
}