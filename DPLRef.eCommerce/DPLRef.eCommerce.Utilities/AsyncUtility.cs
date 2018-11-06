using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace DPLRef.eCommerce.Utilities
{
    class AsyncUtility : UtilityBase, IAsyncUtility
    {
        public AsyncQueueItem CheckForNewItem()
        {
            var queueDir = QueueDir();
            FileInfo file = queueDir.GetFiles().OrderBy(p => p.CreationTime).FirstOrDefault();
            AsyncQueueItem result = null;

            if (file != null && file.Exists)
            {
                var text = File.ReadAllText(file.FullName);
                result = JsonConvert.DeserializeObject<AsyncQueueItem>(text);
                file.Delete();
            }
            return result;
        }

        public void SendEvent(AsyncEventTypes eventType, int eventId)
        {
            var newFile = Path.Combine(QueueDir().FullName, $"{Guid.NewGuid().ToString()}.json");
            var json = JsonConvert.SerializeObject(new AsyncQueueItem()
            {
                AmbientContext = Context, // <== passing along the ambient context
                EventType = eventType,
                EventId = eventId
            });
            File.WriteAllText(newFile, json);
        }

        private DirectoryInfo QueueDir()
        {
            var queuePath = Config.QueuePath;
            var queueDir = new DirectoryInfo(queuePath);
            if (!queueDir.Exists)
                queueDir.Create();
            return queueDir;
        }
    }
}
