using SmartHouse.Entities;

namespace SmartHouse.Lamp.DeviceSetting
{
    public class LampModel
    {
        public string MAC { get; set; }

        public string Name { get; set; }

        public BaseHouseSlaveInvoker On { get; set; }

        public BaseHouseSlaveInvoker Off { get; set; }
    }
}