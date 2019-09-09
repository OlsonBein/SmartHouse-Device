using Newtonsoft.Json;
using SmartHouse.Entities;
using System;
using System.IO;

namespace SmartHouse.Lamp.DeviceSetting
{
    public class LampSetting : ILampSetting
    {
        public Device Lamp { get; set; }

        public SlaveType Slave { get; set; }

        public LampSetting()
        {
            Slave = SlaveType.Device;
        }
        public Device InitDevice()
        {
            Lamp = new Device();
            var _setting = JsonConvert.DeserializeObject<LampModel>(File.ReadAllText(Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\", @"Configs\DeviceConfig.json"))));
            Lamp.MAC = _setting.MAC;
            Lamp.Name = _setting.Name;
            Lamp.On = _setting.On;
            Lamp.Off = _setting.Off;
            return Lamp;
        }
    }
}