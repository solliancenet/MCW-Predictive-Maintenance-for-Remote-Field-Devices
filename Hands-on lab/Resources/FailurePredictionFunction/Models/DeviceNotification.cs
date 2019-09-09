using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Fabrikam.Oil.Pumps
{
    public class DeviceNotification : TableEntity
    {
        public DeviceNotification()
        {
            this.PartitionKey = "Devices";
            this.RowKey = "Unknown";
        }
        
        public DeviceNotification(string deviceId)
        {
            this.PartitionKey = "Devices";
            this.RowKey = deviceId;
        }
        public DateTime LastNotificationUtc {get;set;}
    }
}