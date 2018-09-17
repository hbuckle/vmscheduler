using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Services.AppAuthentication;

namespace function
{
  public class CustomQueueMessage
  {
    public string Action { get; set; }
    public string ResourceGroupId { get; set; }
  }

  public static class Methods
  {
    public static JObject ConvertSchedulerXml(String xml)
    {
      XDocument doc = XDocument.Parse(xml);
      String message = doc.Element("StorageQueueMessage").Element("Message").Value;
      UTF8Encoding utf8 = new UTF8Encoding();
      String json = utf8.GetString(Convert.FromBase64String(message));
      return JObject.Parse(json);
    }
    public static List<String> GetResourceGroupVms(String resourceGroupId)
    {
      var credentials = SdkContext.AzureCredentialsFactory.FromMSI(
        new MSILoginInformation(MSIResourceType.AppService),
        AzureEnvironment.AzureGlobalCloud
      );
      var azure = Azure
          .Configure()
          .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
          .Authenticate(credentials)
          .WithDefaultSubscription();
      var parsed = ResourceId.FromString(resourceGroupId);
      var virtualMachines = azure.VirtualMachines.ListByResourceGroup(parsed.ResourceGroupName);
      List<String> result = new List<string>();
      foreach (var vm in virtualMachines)
      {
        result.Add(vm.Id);
      }
      return result;
    }
  }
}