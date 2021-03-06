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
      try
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
          var encodedMessage = job.Properties?.Action?.QueueMessage?.Message;
          ScheduleMessage scheduleMessage = new ScheduleMessage();
          try
          {
            if (!String.IsNullOrEmpty(encodedMessage))
            {
              var message = Methods.ConvertBase64String(encodedMessage);
              scheduleMessage = JsonConvert.DeserializeObject(
                message,
                (new ScheduleMessage()).GetType()
              ) as ScheduleMessage;
            }
            else
            {
              scheduleMessage.BlankMessage();
            }
          }
          catch (FormatException f)
          {
            log.LogInformation($"Could not decode scheduler job message: {f.Message}");
            scheduleMessage.BlankMessage();
          }
          catch(Newtonsoft.Json.JsonException j)
          {
            log.LogInformation($"Could not deserialize scheduler job message: {j.Message}");
            scheduleMessage.BlankMessage();
          }
          var schedule = new Schedule()
          {
            id = job.Id,
            name = job.Name.Split('/')[1],
            message = scheduleMessage
          };
          if (job.Properties.Recurrence == null)
          {
            schedule.recurrence = new Microsoft.Azure.Management.Scheduler.Models.JobRecurrence();
            schedule.recurrence.Schedule = new Microsoft.Azure.Management.Scheduler.Models.JobRecurrenceSchedule();
          }
          else
          {
            schedule.recurrence = job.Properties.Recurrence;
          }
          result.Add(schedule);
        }
        return (ActionResult)new JsonResult(result);
      }
      catch(Exception e)
      {
        log.LogError(e.Message);
        log.LogError(e.StackTrace);
        throw e;
      }
    }
  }
}
