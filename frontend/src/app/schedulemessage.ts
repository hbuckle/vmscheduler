export class ScheduleMessage {
  constructor(
    public action: string,
    public resourceGroupIds: string[],
    public virtualMachineIds: string[],
    public tags: string[],
  ) {}
}