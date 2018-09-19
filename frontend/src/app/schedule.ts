export class Schedule {
  id: string;
  name: string;
  action: string;
  recurrence: string;
  resourceGroupIds: string[];
  virtualMachineIds:  string[];
}