using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Management.Scheduler;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Services.AppAuthentication;

namespace function
{
  public static class getschedulerjob
  {
    [FunctionName("getschedulerjob")]
    public static IActionResult Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
      HttpRequest req,
      ILogger log,
      ExecutionContext context
    )
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(context.FunctionAppDirectory)
        .AddEnvironmentVariables()
        .Build();
      var credentials = Methods.GetAzureCredentials();
      SchedulerManagementClient schedulerManagementClient = new SchedulerManagementClient(
        credentials
      );
      var parsed = ResourceId.FromString(config["scheduler_job_collection_id"]);
      schedulerManagementClient.SubscriptionId = parsed.SubscriptionId;
      var jobs = schedulerManagementClient.Jobs.List(parsed.ResourceGroupName, parsed.Name);
      var alljobs = jobs.AsContinuousCollection(x => schedulerManagementClient.Jobs.ListNext(x));
      var result = new List<Schedule>();
      foreach (var job in alljobs)
      {
        var message = Methods.ConvertBase64JsonString(job.Properties.Action.QueueMessage.Message);
        result.Add(new Schedule()
        {
          Id = job.Id,
          Name = job.Name.Split('/')[1],
          Action = (String)message["action"],
          Recurrence = JsonConvert.SerializeObject(job.Properties.Recurrence),
          ResourceGroupIds = message["resourceGroupIds"].Values<string>(),
          VirtualMachineIds = message["virtualMachineIds"].Values<string>()
        });
      }
      return (ActionResult)new JsonResult(result);
    }
  }
}
