using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Services.AppAuthentication;

namespace function
{
  public static class startvm
  {
    [FunctionName("startvm")]
    public static void Run(
      [QueueTrigger("start", Connection = "queue_start")]
      String vmId,
      ILogger log
    )
    {
      try {
        log.LogInformation(vmId);
        var credentials = Methods.GetAzureCredentials();
        var azure = Azure
            .Configure()
            .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
            .Authenticate(credentials)
            .WithDefaultSubscription();
        var parsed = ResourceId.FromString(vmId);
        var vm = azure.VirtualMachines.GetById(vmId);
        vm.Start();
      }
      catch(Exception e)
      {
        log.LogError(e.Message);
        throw e;
      }
    }
  }
}