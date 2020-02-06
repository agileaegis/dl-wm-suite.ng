using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Models;
using dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Repositories.Contracts;
using Marten;

namespace dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Repositories.Impls
{
    public class MartenDeviceRepository : IDeviceRepository
    {
        private readonly IDocumentSession session;

        public MartenDeviceRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public void Add(Device device)
        {
            session.Insert(device);
        }

        public async Task<bool> Exists(string imei)
        {
            return await session.Query<Device>().AnyAsync(t => t.Imei == imei);
        }

        public async Task<Device> WithImei(string imei)
        {
            return await session.Query<Device>().FirstOrDefaultAsync(t => t.Imei == imei);
        }

        Task<Device> IDeviceRepository.this[string imei] => throw new System.NotImplementedException();

        public Task<Device> this[string imei] => WithImei(imei);
    }
}