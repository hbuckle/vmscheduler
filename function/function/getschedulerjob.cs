using System;
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
    public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
    {
      log.LogInformation("getschedulerjob called");
      var config = new ConfigurationBuilder()
        .SetBasePath(context.FunctionAppDirectory)
        .AddEnvironmentVariables()
        .Build();
      var credentials = SdkContext.AzureCredentialsFactory.FromMSI(
        new MSILoginInformation(MSIResourceType.AppService),
        AzureEnvironment.AzureGlobalCloud
      );
      var azure = Azure
            .Configure()
            .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
            .Authenticate(credentials)
            .WithDefaultSubscription();
      SchedulerManagementClient schedulerManagementClient = new SchedulerManagementClient(
        credentials
      );
      schedulerManagementClient.SubscriptionId = azure.SubscriptionId;
      var parsed = ResourceId.FromString(config["scheduler_job_collection_id"]);
      var jobs = schedulerManagementClient.Jobs.List(parsed.ResourceGroupName, parsed.Name);
      var result = jobs.AsContinuousCollection(x => schedulerManagementClient.Jobs.ListNext(x));
      return (ActionResult)new JsonResult(result);
    }
  }
}
