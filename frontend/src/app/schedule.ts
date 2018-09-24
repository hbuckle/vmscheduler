import { JobRecurrence } from './jobrecurrence';
import { ScheduleMessage } from './schedulemessage';

export class Schedule {
  constructor(
    public id: string,
    public name: string,
    public recurrence: JobRecurrence,
    public message: ScheduleMessage,
  ) {}
}