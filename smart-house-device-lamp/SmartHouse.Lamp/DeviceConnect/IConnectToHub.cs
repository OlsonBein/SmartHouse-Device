using System.Threading.Tasks;

namespace SmartHouse.Lamp.DeviceConnect
{
    public interface IConnectToHub
    {
        Task ClientConnect();
    }
}