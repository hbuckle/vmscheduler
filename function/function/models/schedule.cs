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
    public String Id { get; set; }
    public String Name { get; set; }
    public JobRecurrence Recurrence { get; set; }
    public ScheduleMessage Message { get; set; }
  }
  public class ScheduleMessage
  {
    [JsonProperty(Required = Required.Always)]
    public String Action { get; set; }

    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> ResourceGroupIds { get; set; }

    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> VirtualMachineIds { get; set; }

    [JsonProperty(Required = Required.Always)]
    public IEnumerable<String> Tags { get; set; }

    public String ToBase64JsonString()
    {
      String json = JsonConvert.SerializeObject(this);
      UTF8Encoding utf8 = new UTF8Encoding();
      return Convert.ToBase64String(utf8.GetBytes(json));
    }

    public void BlankMessage()
    {
      this.Action = "None";
      this.ResourceGroupIds = new List<String>();
      this.VirtualMachineIds = new List<String>();
      this.Tags = new List<String>();
    }
  }
}