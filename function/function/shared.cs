using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Services.AppAuthentication;

namespace function
{
  public static class Methods
  {
    public static String ConvertBase64String(String input)
    {
      UTF8Encoding utf8 = new UTF8Encoding();
      return utf8.GetString(Convert.FromBase64String(input));
    }
    public static ScheduleMessage ConvertSchedulerXml(String xml)
    {
      XDocument doc = XDocument.Parse(xml);
      String message = doc.Element("StorageQueueMessage").Element("Message").Value;
      return JsonConvert.DeserializeObject(
        message,
        (new ScheduleMessage()).GetType()
      ) as ScheduleMessage;
    }
    public static List<String> GetResourceGroupVms(String resourceGroupId)
    {
      var credentials = Methods.GetAzureCredentials();
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
    public static AzureCredentials GetAzureCredentials() => SdkContext.AzureCredentialsFactory.FromMSI(
      new MSILoginInformation(MSIResourceType.AppService),
      AzureEnvironment.AzureGlobalCloud
    );
    public static String GetQueueSasToken(String connectionString)
    {
      CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
      SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
      {
        Permissions =
          SharedAccessAccountPermissions.Read |
          SharedAccessAccountPermissions.Write |
          SharedAccessAccountPermissions.Delete |
          SharedAccessAccountPermissions.Add |
          SharedAccessAccountPermissions.Create |
          SharedAccessAccountPermissions.ProcessMessages |
          SharedAccessAccountPermissions.Update |
          SharedAccessAccountPermissions.List,
        Services = SharedAccessAccountServices.Queue,
        ResourceTypes =
          SharedAccessAccountResourceTypes.Service |
          SharedAccessAccountResourceTypes.Container |
          SharedAccessAccountResourceTypes.Object,
        SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1),
        Protocols = SharedAccessProtocol.HttpsOnly
      };
      return account.GetSharedAccessSignature(policy);
    }
    public static String GetStorageAccountName(String connectionString)
    {
      CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
      return account.QueueEndpoint.Host.Split('.')[0];
    }
  }
}