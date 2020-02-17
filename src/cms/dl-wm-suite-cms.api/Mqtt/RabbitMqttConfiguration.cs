using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices;
using dl.wm.suite.common.dtos.Vms.Devices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog.Formatting.Json;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace dl.wm.suite.cms.api.Mqtt
{
  public class RabbitMqttConfiguration : IRabbitMqttConfiguration
  {
    public IConfiguration Configuration { get; }
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceProvider _service;

    private MqttClient _client;

    public RabbitMqttConfiguration(IConfiguration configuration,
      IServiceScopeFactory scopeFactory, IServiceProvider service)
    {
      Configuration = configuration;
      _scopeFactory = scopeFactory;
      _service = service;
    }

    public void EstablishConnection()
    {
      _client = new MqttClient(Configuration.GetSection("RabbitMq:Api").Value);

      _client.Subscribe(new[]
        {
          "wm/ack"
        },
        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

      _client.Subscribe(new[]
        {
          "wm/nack"
        },
        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

      _client.Subscribe(new[]
        {
          "container/post"
        },
        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

      _client.Subscribe(new[]
        {
          "container/put"
        },
        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

      _client.Subscribe(new[]
        {
          "container/delete"
        },
        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
      _client.Subscribe(new[]
        {
          "telemetry/message"
        },
        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

      _client.MqttMsgPublishReceived += ClientMqttMsgPublishReceived;
      _client.ConnectionClosed += ClientConnectionClosed;
      _client.MqttMsgPublished += ClientMqttMsgPublished;
      _client.MqttMsgSubscribed += ClientMqttMsgSubscribed;
      _client.MqttMsgUnsubscribed += ClientMqttMsgUnsubscribed;

      _client.Connect($"CMS-{Guid.NewGuid().ToString()}",
        Configuration.GetSection("RabbitMq:Username").Value
        , Configuration.GetSection("RabbitMq:Password").Value
      );
    }

    private void ClientMqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
    {
    }

    private void ClientMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
    }

    private void ClientMqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
    {
    }

    private void ClientConnectionClosed(object sender, EventArgs e)
    {
    }

    private async void ClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
      var jsonToBeSerialized = System.Text.Encoding.Default.GetString(e.Message);
      TelemetryMessageModel telemetryModelModel = JsonConvert.DeserializeObject<TelemetryMessageModel>(jsonToBeSerialized);
      try
      {
        await DoScopedMeasurementStore(jsonToBeSerialized, telemetryModelModel);
      }
      catch (Exception exception)
      {
        //Todo: Handle Exception
      }
    }

    private async Task DoScopedMeasurementStore(string jsonValue, TelemetryMessageModel telemetryModelModel)
    {
      using (var scope = _scopeFactory.CreateScope())
      {
        var iUpdateDeviceProcessor = scope.ServiceProvider.GetRequiredService<IUpdateDeviceProcessor>();
        await iUpdateDeviceProcessor.StoreMeasurement(telemetryModelModel.Imei, jsonValue, new DeviceForMeasurementModel()
        {
          MeasurementValueJson = jsonValue,
          Altitude = telemetryModelModel.Altitude,
          Speed = telemetryModelModel.Speed,
          Bearing = telemetryModelModel.Bearing,
          Angle = telemetryModelModel.Angle,
          Satellites = telemetryModelModel.NumOfSatellites,
          GeoLat = telemetryModelModel.Latitude,
          GeoLon = telemetryModelModel.Longitude,
          TimeToFix = telemetryModelModel.TimeToFix,
          SignalLength = telemetryModelModel.SignalLength,
          StatusFlags = telemetryModelModel.StatusFlags,
          Timestamp = telemetryModelModel.Timestamp,
          Temperature = telemetryModelModel.Temperature,
          FillLevel = telemetryModelModel.FillLevel,
          TiltX = telemetryModelModel.TiltX,
          TiltY = telemetryModelModel.TiltY,
          TiltZ = telemetryModelModel.TiltZ,
          Light = telemetryModelModel.Light,
          Battery = telemetryModelModel.Battery,
          Gps = telemetryModelModel.Gps,
          NbIot = telemetryModelModel.NbIoT,
          Distance = telemetryModelModel.Distance,
          Tamper = telemetryModelModel.Tamper,
          NbIoTSignalLength = telemetryModelModel.NbIoTSignalLength,
          LatestResetCause = telemetryModelModel.LatestResetCause,
          FirmwareVersion = telemetryModelModel.FirmwareVersion,
          TemperatureEnable = telemetryModelModel.TemperatureEnable,
          DistanceEnable = telemetryModelModel.DistanceEnable,
          TiltEnable = telemetryModelModel.TiltEnable,
          MagnetometerEnable = telemetryModelModel.MagnetometerEnable,
          TamperEnable = telemetryModelModel.TamperEnable,
          BatterySafeMode = telemetryModelModel.BatterySafeMode,
          NbIoTMode = telemetryModelModel.NbIoTMode,
        });
      }
    }
  }
}