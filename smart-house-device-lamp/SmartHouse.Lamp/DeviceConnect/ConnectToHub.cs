using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using NLog;
using SmartHouse.Entities;
using SmartHouse.Lamp.DeviceSetting;
using SmartHouse.Udp.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouse.Lamp.DeviceConnect
{
    public class ConnectToHub : IConnectToHub
    {
        private const string _on = "On";

        private const string _off = "Off";

        private DeviceUdp _device { get; set; }

        private int _numberPort { get; set; }

        private HubConnection _connection { get; set; }

        private ILampSetting _lampSetting { get; set; }

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public ConnectToHub(ILampSetting setting)
        {
       
            _lampSetting = setting;
            var Port = JsonConvert.DeserializeObject<ConnectModel>(File.ReadAllText(Path.GetFullPath(
                 Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..", @"Configs\PortConfig.json"))));
            _numberPort = Port.PortToSend;
            _device = new DeviceUdp(_numberPort);
        }
       
        public async Task ClientConnect()
        {
            string url;
            try
            {
              url   = _device.GetBaseData().Url;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            _connection = new HubConnectionBuilder().WithUrl(url).Build();

            await _connection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Error("There was an error opening the connection", task.Exception.GetBaseException());
                }
                else
                {
                    _logger.Info("Connected");
                }
            });

            await _connection.InvokeAsync(_device.GetBaseData().Device, _lampSetting.InitDevice());
            _connection.On(_on, async () => await DeviceOn());
            _connection.On(_off, async () => await DeviceOff());
            await _connection.InvokeAsync("TurnOn", _lampSetting.InitDevice());

        }

        public async void InitMyDevice(Dictionary<SlaveType, string> urls)
        {
            var pathToInit = urls.FirstOrDefault(x => x.Key == _lampSetting.Slave).Value;
            await _connection.InvokeAsync(pathToInit, _lampSetting.InitDevice());
            _logger.Info("Device Created");
        }

        public async Task DeviceOn()
        {
            await _connection.InvokeAsync("StatusChanger", _lampSetting.Lamp.MAC, SlaveStatus.On);
            _logger.Info("Device is On");
        }

        public async Task DeviceOff()
        {
            await _connection.InvokeAsync("StatusChanger", _lampSetting.Lamp.MAC, SlaveStatus.Off);
            _logger.Info("Device is Off");
        }
    }
}
