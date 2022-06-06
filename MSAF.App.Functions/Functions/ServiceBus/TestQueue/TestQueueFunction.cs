using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace MSAF.App.Functions.Functions.ServiceBus.TestQueue
{
    public class TestQueueFunction
    {
        private readonly ILogger _logger;

        public TestQueueFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TestQueueFunction>();
        }

        [Function("TestQueueFunction")]
        public async Task Run([ServiceBusTrigger("%QueueName:Test%", Connection = "ServiceBusConnection", IsSessionsEnabled = false)] string mymessage, string messageReceiver, string lockToken, FunctionContext context, ILogger log)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {mymessage}");

            BindingContext bindingContext = context.BindingContext;
            var bindingData = bindingContext.BindingData;

            //await messageReceiver.DeadLetterAsync(Guid.Parse(message.LockToken));
        }

        //[FunctionName("TestQueueFunction2")]
        //public async Task Run([ServiceBusTrigger("%QueueName:Test%", Connection = "ServiceBusConnection", IsSessionsEnabled = false, AutoComplete = false)] string myQueueItem, MessageReceiver messageReceiver, string lockToken, ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        //    if (myQueueItem.Contains("DEAD"))
        //    {
        //        await messageReceiver.DeadLetterAsync(lockToken);
        //        return;
        //    }
        //    await messageReceiver.CompleteAsync(lockToken);

        //}
    }
}
