using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace function
{
  public static class processschedulermessage
  {
    [FunctionName("processschedulermessage")]
    public static void Run(
      [QueueTrigger("scheduler", Connection = "queue_scheduler")]
      String xml,
      [Queue("stop", Connection = "queue_stop")]
      ICollector<String> stop,
      [Queue("start", Connection = "queue_start")]
      ICollector<String> start,
      ILogger log
    )
    {
      try {
        log.LogInformation(xml);
        JObject message = Methods.ConvertSchedulerXml(xml);
        String action = (String)message["action"];
        log.LogInformation(action);
        List<String> vms = new List<string>();
        foreach (String resourceGroupId in message["resourceGroupIds"])
        {
          log.LogInformation(resourceGroupId);
          vms.AddRange(Methods.GetResourceGroupVms(resourceGroupId));
        }
        foreach (String vmId in message["virtualMachineIds"])
        {
          log.LogInformation(vmId);
          vms.Add(vmId);
        }
        switch (action)
        {
          case "start":
            foreach (String vm in vms)
            {
              log.LogInformation(vm);
              start.Add(vm);
            }
            break;
          case "stop":
            foreach (String vm in vms)
            {
              log.LogInformation(vm);
              stop.Add(vm);
            }
            break;
          default:
            break;
        }
      }
      catch(Exception e)
      {
        log.LogError(e.Message);
        throw e;
      }
    }
  }
}
