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
  public static class stopvm
  {
    [FunctionName("stopvm")]
    public static void Run(
      [QueueTrigger("stop", Connection = "queue_stop")]
      String vmId,
      ILogger log
    )
    {
      try {
        log.LogInformation(vmId);
        var credentials = SdkContext.AzureCredentialsFactory.FromMSI(
          new MSILoginInformation(MSIResourceType.AppService),
          AzureEnvironment.AzureGlobalCloud
        );
      var azure = Azure
          .Configure()
          .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
          .Authenticate(credentials)
          .WithDefaultSubscription();
      var parsed = ResourceId.FromString(vmId);
      var vm = azure.VirtualMachines.GetById(vmId);
      vm.Deallocate();
      }
      catch(Exception e)
      {
        log.LogError(e.Message);
        throw e;
      }
    }
  }
}