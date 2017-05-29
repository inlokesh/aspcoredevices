using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace DeviceAPI.Models
{
    public class DeviceEntity : TableEntity
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public void AssignRowKey()
        {
            this.RowKey = this.DeviceID.ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = this.DeviceType;
        }
    }

    public class Device
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }

    public static class Extensions
    {
        public static Device ToDevice(this DeviceEntity deviceEntity)
        {
            var result = new Device();
            result.DeviceID = deviceEntity.DeviceID;
            result.DeviceName = deviceEntity.DeviceName;
            result.DeviceType = deviceEntity.DeviceType;
            return result;
        }

        public static List<Device> ToDevices(this IEnumerable<DeviceEntity> deviceEntities)
        {
            List<Device> result = new List<Device>();
            foreach (DeviceEntity entity in deviceEntities)
            {
                result.Add(entity.ToDevice());
            }
            return result;
        }

        public static DeviceEntity ToDeviceEntity(this Device device)
        {
            DeviceEntity deviceEntity = new DeviceEntity();
            deviceEntity.DeviceID = device.DeviceID;
            deviceEntity.DeviceName = device.DeviceName;
            deviceEntity.DeviceType = device.DeviceType;
            deviceEntity.AssignPartitionKey();
            deviceEntity.AssignRowKey();
            return deviceEntity;
        }
    }

    public enum DeviceType { Camera, Thermostat, SecurityPanel }
}