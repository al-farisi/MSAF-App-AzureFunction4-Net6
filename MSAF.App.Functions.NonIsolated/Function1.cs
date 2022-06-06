using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MSAF.App.Functions.NonIsolated
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async Task Run([ServiceBusTrigger("%QueueName:Test%", Connection = "ServiceBusConnection", IsSessionsEnabled = false, AutoComplete = false)] string myQueueItem, MessageReceiver messageReceiver, string lockToken, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            if (myQueueItem.Contains("DEAD"))
            {
                await messageReceiver.DeadLetterAsync(lockToken);
                return;
            }
            await messageReceiver.CompleteAsync(lockToken);

        }
    }
}
