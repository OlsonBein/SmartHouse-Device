using SmartHouse.Entities;


namespace SmartHouse.Lamp.DeviceSetting
{
    public interface ILampSetting
    {
        Device Lamp { get; set; }

        SlaveType Slave { get; set; }

        Device InitDevice();
    }
}