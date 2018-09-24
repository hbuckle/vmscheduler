export class JobRecurrenceSchedule {
  constructor(
    public weekDays: string[],
    public hours: number[],
    public minutes: number[],
    public monthDays: number[],
    public monthlyOccurrences: any[],
  ) {}
}