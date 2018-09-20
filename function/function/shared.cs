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
  public class Schedule
  {
    public String Id { get; set; }
    public String Name { get; set; }
    public String Action { get; set; }
    public String Recurrence { get; set; }
    public IEnumerable<String> ResourceGroupIds { get; set; }
    public IEnumerable<String> VirtualMachineIds { get; set; }
  }
  public class ScheduleMessage
  {
    [JsonProperty(Required = Required.Always)]
    public String action { get; set; }
    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> resourceGroupIds { get; set; }
    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> virtualMachineIds { get; set; }
    public String ToBase64JsonString()
    {
      String json = JsonConvert.SerializeObject(this);
      UTF8Encoding utf8 = new UTF8Encoding();
      return Convert.ToBase64String(utf8.GetBytes(json));
    }
  }

  public static class Methods
  {
    public static JObject ConvertBase64JsonString(String input)
    {
      UTF8Encoding utf8 = new UTF8Encoding();
      String json = utf8.GetString(Convert.FromBase64String(input));
      return JObject.Parse(json);
    }
    public static JObject ConvertSchedulerXml(String xml)
    {
      XDocument doc = XDocument.Parse(xml);
      String message = doc.Element("StorageQueueMessage").Element("Message").Value;
      return ConvertBase64JsonString(message);
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