using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DeviceAPI.Models;

namespace DeviceAPI.Controllers
{
    public class DeviceController : ApiController
    {
        private DeviceTableStorageController deviceTableController = new DeviceTableStorageController();

        // GET: api/Device
        public List<Device> Get()
        {
            return deviceTableController.GetAllDevices();
        }

        // GET: api/Device/5
        public Device Get(int id)
        {
            if(id == 0)
                deviceTableController.InsertDevices();
            return deviceTableController.GetDevice(id);
        }

        // POST: api/Device
        public void Post([FromBody]Device value)
        {
            deviceTableController.InsertDevice(value);
        }

        // PUT: api/Device/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Device/5
        public void Delete([FromBody]Device value)
        {
            deviceTableController.DeleteDevice(value.ToDeviceEntity());
        }
    }
}
