using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Configurations.Installers;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Models;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts;
using Cassandra;
using Cassandra.Mapping;
using Serilog;

namespace dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Impls
{
    public class DeviceCassandraRepository : IDeviceCassandraRepository
    {
        private readonly IMapper _mapper;
        private readonly int replicationFactor = 2;
        private readonly string tableName;
        private readonly string keyspaceName;

        public DeviceCassandraRepository()
        {
            _mapper = new Mapper(CassandraInitializer.session);
        }

        public async Task CreateKeySpaceDevice()
        {
            try
            {
                Log.Debug(
                    $"Create Keyspace Cassandra" +
                    "--CreateKeySpaceDevice--  @Complete@ [DeviceCassandraRepository]. " +
                    "Message: Just Before CreateKeyspace");

                CassandraInitializer.session.Execute("DROP KEYSPACE if exists dlwmtelemetry;");
                CassandraInitializer.session.Execute("CREATE KEYSPACE dlwmtelemetry WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };");
               
                Log.Debug(
                    $"Create Keyspace Cassandra" +
                    "--CreateKeySpaceDevice--  @Complete@ [DeviceCassandraRepository]. " +
                    "Message: Just After CreateKeyspace");
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Create Keyspace dlwmtelemetry Cassandra: Details: {e.Message}" +
                    "--CreateKeySpaceDevice--  @NotComplete@ [DeviceCassandraRepository]. " +
                    "Message: Error Creating KeySpace Device");
            }
        }

        public async Task CreateTableDevice()
        {
            try
            {
                Log.Debug(
                    $"Create Table Cassandra" +
                    "--CreateTableDevice--  @Complete@ [DeviceCassandraRepository]. " +
                    "Message: Just Before CreateTable");

                CassandraInitializer.session.Execute("CREATE TABLE IF NOT EXISTS dlwmtelemetry.devices (id text PRIMARY KEY, imei varchar, provisioning_at timestamp, activation_at timestamp, " +
                                                     "is_enabled boolean, is_activated boolean, firmware_version varchar)");
                CassandraInitializer.session.Execute("CREATE TABLE IF NOT EXISTS dlwmtelemetry.telemetryrow (id text PRIMARY KEY, json text)");
                Log.Debug(
                    $"Create Table Cassandra" +
                    "--CreateTableDevice--  @Complete@ [DeviceCassandraRepository]. " +
                    "Message: Just After CreateTable");
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Create Table Device Cassandra: Details: {e.Message}" +
                    "--CreateTableDevice--  @NotComplete@ [DeviceCassandraRepository]. " +
                    "Message: Error Creating Table Device");
            }
        }

        public async Task<Device> AddDevice(Device device)
        {
            device.Id = Guid.NewGuid().ToString();

            try
            {
                Log.Debug(
                    $"Create Device Cassandra: Id: {device.Id} - Imei: {device.Imei}" +
                    "--AddDevice--  @Complete@ [DeviceCassandraRepository]. " +
                    "Message: Just Before MakeItPersistence");                
                
                await _mapper.InsertAsync(device);
                
                Log.Debug(
                    $"Create Device Cassandra: Id: {device.Id} - Imei: {device.Imei}" +
                    "--AddDevice--  @Complete@ [DeviceCassandraRepository]. " +
                    "Message: Just After MakeItPersistence");
                return await GetSingleDevice(device.Id);
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Create Device Cassandra: Details: {e.Message}" +
                    "--AddDevice--  @NotComplete@ [DeviceCassandraRepository]. " +
                    "Message: Error Creating Device");
            }

            return null;
        }

        public async Task<Device> UpdateDevice(Device device)
        {
            var updateDevice = new SimpleStatement("UPDATE devices SET " 
                                                   +  " imei = ? " 
                                                   +  ",is_active  = ? " 
                                                   +  ",is_enabled    = ? " 
                                                   +  ",firmware_version   = ? " 
                                                   +  " WHERE id  = ? ",
                device.Imei, device.IsActivated, device.IsEnabled, device.FirmwareVer, device.Id);

            try
            {
                Log.Debug(
                    $"Update Device Cassandra: Id: {device.Id} - Imei: {device.Imei}" +
                    "--UpdateDevice--  @Complete@ [DeviceCassandraRepository]. " +
                    "Message: Just Before MakeItPersistence");

                await CassandraInitializer.session.ExecuteAsync(updateDevice);

                Log.Debug(
                    $"Update Device Cassandra: Id: {device.Id} - Imei: {device.Imei}" +
                    "--UpdateDevice--  @NotComplete@ [DeviceCassandraRepository]. " +
                    "Message: Just After MakeItPersistence");
                return await GetSingleDevice(device.Id);
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Update Device Cassandra: Details: {e.Message}" +
                    "--UpdateDevice--  @NotComplete@ [DeviceCassandraRepository]. " +
                    "Message: Error Updating Device");
            }
            return null;
        }

        public async Task DeleteDevice(string id)
        {
            var deleteDevice = new SimpleStatement("DELETE FROM devices WHERE id = ? ", id);
            await CassandraInitializer.session.ExecuteAsync(deleteDevice);
        }

        public async Task<Device> GetSingleDevice(string id)
        {
            Device device = await _mapper.SingleOrDefaultAsync<Device>("SELECT * FROM devices WHERE id = ?", id);
            return device;
        }

        public async Task<Device> GetSingleDeviceByImei(string imei)
        {
            Device device = await _mapper.SingleOrDefaultAsync<Device>("SELECT * FROM devices WHERE imei = ?", imei);
            return device;
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            IEnumerable<Device> devices = await _mapper.FetchAsync<Device>("SELECT * FROM devices");
            return devices;
        }
    }
}
