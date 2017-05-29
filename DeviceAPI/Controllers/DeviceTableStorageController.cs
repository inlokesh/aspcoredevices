using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

using DeviceAPI.Models;

namespace DeviceAPI.Controllers
{
    public class DeviceTableStorageController
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString"));
        CloudTable devicesTable;
        
        public DeviceTableStorageController()
        {
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Get a reference to a table named "DevicesTable"
            devicesTable = tableClient.GetTableReference("DevicesTable");
            // Create the CloudTable if it does not exist
            devicesTable.CreateIfNotExists();
        }
        
        public void InsertDevice(Device device)
        {
            // Create a Device entity and add it to the table.
            InsertDevice(device.ToDeviceEntity());
        }

        public void InsertDevice(DeviceEntity device)
        {
            if (RetrieveRecord(devicesTable, device.DeviceType, device.DeviceID.ToString()) == null)
            {
                TableOperation tableOperation = TableOperation.Insert(device);
                devicesTable.Execute(tableOperation);
            }
        }

        public void InsertDevices()
        {
            //Create Device1 to Insert
            DeviceEntity deviceEntity1 = new DeviceEntity();
            deviceEntity1.DeviceID = 1;
            deviceEntity1.DeviceName = "Vacation Home Camera 1";
            deviceEntity1.DeviceType = DeviceType.Camera.ToString();
            deviceEntity1.AssignPartitionKey();
            deviceEntity1.AssignRowKey();

            InsertDevice(deviceEntity1);

            //Create Device2 to Insert
            DeviceEntity deviceEntity = new DeviceEntity();
            deviceEntity.DeviceID = 2;
            deviceEntity.DeviceName = "City Home Thermostat 1";
            deviceEntity.DeviceType = DeviceType.Thermostat.ToString();
            deviceEntity.AssignPartitionKey();
            deviceEntity.AssignRowKey();

            InsertDevice(deviceEntity);
        }

        public Device GetDevice(int id)
        {
            return RetrieveRecord(devicesTable, DeviceType.Camera.ToString(), id.ToString()).ToDevice();
        }

        public void RemoveAllDevices()
        {
            DeleteAllDeviceRecords();
        }

        /// <summary>
        /// Delete all the Devices table records
        /// </summary>
        public void DeleteAllDeviceRecords()
        {
            TableQuery<DeviceEntity> tableQuery = new TableQuery<DeviceEntity>();
            foreach (DeviceEntity customerEntity in devicesTable.ExecuteQuery(tableQuery))
            {
                TableOperation tableOperation = TableOperation.Delete(customerEntity);
                devicesTable.Execute(tableOperation);
            }
        }

        public void DeleteDevice(DeviceEntity deviceEntity)
        {
            if(RetrieveRecord(devicesTable, deviceEntity.PartitionKey, deviceEntity.RowKey) != null)
            {
                TableOperation tableOperation = TableOperation.Delete(deviceEntity);
                devicesTable.Execute(tableOperation);
            }
        }

        /// <summary>
        /// Drop the table from the Storage
        /// </summary>
        /// <param name="table">table to drop</param>
        public static void DropTable(CloudTable table)
        {
            if (!table.DeleteIfExists())
            {
                Console.WriteLine("Table does not exists");
            }
        }

        public DeviceEntity RetrieveRecord(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<DeviceEntity>(partitionKey, rowKey);
            TableResult tableResult = table.Execute(tableOperation);
            return tableResult.Result as DeviceEntity;
        }

        public List<Device> GetAllDevices()
        {
            //Declare the Query
            var query = new TableQuery<DeviceEntity>();
            //Execute the query against DevicesTable, and chain the conversion to Devices
            var result = devicesTable.ExecuteQuery(query).ToDevices();            
            return result;
        }
    }
}