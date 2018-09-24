import { JobRecurrenceSchedule } from './jobrecurrenceschedule';

export class JobRecurrence {
  constructor(
    public frequency: string,
    public interval: number,
    public count: number,
    public endTime: string,
    public schedule: JobRecurrenceSchedule,
  ) {}
}