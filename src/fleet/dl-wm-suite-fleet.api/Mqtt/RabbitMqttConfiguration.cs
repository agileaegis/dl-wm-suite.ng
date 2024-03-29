﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace dl.wm.suite.fleet.api.Mqtt
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

      _client.MqttMsgPublishReceived += ClientMqttMsgPublishReceived;
      _client.ConnectionClosed += ClientConnectionClosed;
      _client.MqttMsgPublished += ClientMqttMsgPublished;
      _client.MqttMsgSubscribed += ClientMqttMsgSubscribed;
      _client.MqttMsgUnsubscribed += ClientMqttMsgUnsubscribed;

      _client.Connect($"INTER-{Guid.NewGuid().ToString()}",
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

    private void ClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {

    }
  }
}