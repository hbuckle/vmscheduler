using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Management.Scheduler;
using Microsoft.Azure.Management.Scheduler.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Services.AppAuthentication;

namespace function
{
  public static class createupdateschedulerjob
  {
    [FunctionName("createupdateschedulerjob")]
    public static IActionResult Run(
      [HttpTrigger(AuthorizationLevel.Function, "put", Route = "createupdateschedulerjob/{name}")]
      Microsoft.AspNetCore.Http.HttpRequest req,
      String name,
      ILogger log,
      ExecutionContext context
    )
    {
      try {
        var config = new ConfigurationBuilder()
          .SetBasePath(context.FunctionAppDirectory)
          .AddEnvironmentVariables()
          .Build();
        var credentials = Methods.GetAzureCredentials();
        SchedulerManagementClient schedulerManagementClient = new SchedulerManagementClient(
          credentials
        );
        ScheduleMessage scheduleMessage;
        using (var reader = new StreamReader(req.Body))
        {
          try
          {
            var body = reader.ReadToEnd();
            log.LogInformation(body);
            scheduleMessage = JsonConvert.DeserializeObject(
              body,
              (new ScheduleMessage()).GetType()
            ) as ScheduleMessage;
          }
          catch(Newtonsoft.Json.JsonException q)
          {
            return (ActionResult)new BadRequestObjectResult(q.Message);
          }
        }
        var parsed = ResourceId.FromString(config["scheduler_job_collection_id"]);
        schedulerManagementClient.SubscriptionId = parsed.SubscriptionId;
        schedulerManagementClient.Jobs.CreateOrUpdate(
          parsed.ResourceGroupName,
          parsed.Name,
          name,
          CreateJobDefinition(config["queue_scheduler"], scheduleMessage.ToBase64JsonString())
        );
        return (ActionResult)new OkResult();
      }
      catch(Exception e)
      {
        log.LogError(e.Message);
        log.LogError(e.StackTrace);
        log.LogError(e.Source);
        throw e;
      }
    }
    private static JobDefinition CreateJobDefinition(String connection, String message)
    {
      var jobDefinition = new JobDefinition();
      jobDefinition.Properties = new JobProperties();
      jobDefinition.Properties.Action = new JobAction();
      jobDefinition.Properties.Action.QueueMessage = new StorageQueueMessage();
      jobDefinition.Properties.Action.Type = JobActionType.StorageQueue;
      jobDefinition.Properties.Action.QueueMessage.QueueName = "scheduler";
      jobDefinition.Properties.Action.QueueMessage.StorageAccount = Methods.GetStorageAccountName(connection);
      jobDefinition.Properties.Action.QueueMessage.SasToken = Methods.GetQueueSasToken(connection);
      jobDefinition.Properties.Action.QueueMessage.Message = message;
      jobDefinition.Properties.StartTime = DateTime.UtcNow.AddYears(-1);
      return jobDefinition;
    }
  }
}