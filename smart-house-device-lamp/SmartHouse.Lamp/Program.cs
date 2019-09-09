using Microsoft.Extensions.DependencyInjection;
using SmartHouse.Entities;
using SmartHouse.Lamp.DeviceConnect;
using SmartHouse.Lamp.DeviceSetting;
using System;

namespace SmartHouse.Lamp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<Device>()
                .AddSingleton<ILampSetting, LampSetting>()
                .AddSingleton<IConnectToHub, ConnectToHub>()
                .BuildServiceProvider();

            var device  = serviceProvider.GetService<ILampSetting>();
            device.InitDevice();
            var connect = serviceProvider.GetService<IConnectToHub>();
            connect.ClientConnect();
            Console.ReadLine();
        }
    }
}