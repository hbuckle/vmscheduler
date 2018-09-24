using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Management.Scheduler.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace function
{
  public class Schedule
  {
    public String id { get; set; }
    public String name { get; set; }
    public JobRecurrence recurrence { get; set; }
    public ScheduleMessage message { get; set; }
  }
  public class ScheduleMessage
  {
    [JsonProperty(Required = Required.Always)]
    public String action { get; set; }

    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> resourceGroupIds { get; set; }

    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> virtualMachineIds { get; set; }

    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> tags { get; set; }

    public String ToBase64JsonString()
    {
      String json = JsonConvert.SerializeObject(this);
      UTF8Encoding utf8 = new UTF8Encoding();
      return Convert.ToBase64String(utf8.GetBytes(json));
    }

    public void BlankMessage()
    {
      this.action = "None";
      this.resourceGroupIds = new List<String>();
      this.virtualMachineIds = new List<String>();
      this.tags = new List<String>();
    }
  }
}