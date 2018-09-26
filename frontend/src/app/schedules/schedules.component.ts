import { Component, OnInit } from '@angular/core';
import { Schedule } from '../schedule';
import { ScheduleService } from '../schedule.service';
import { JobRecurrence } from '../jobrecurrence';
import { JobRecurrenceSchedule } from '../jobrecurrenceschedule';
import { ScheduleMessage } from '../schedulemessage';

@Component({
  selector: 'app-schedules',
  templateUrl: './schedules.component.html',
  styleUrls: ['./schedules.component.css']
})
export class SchedulesComponent implements OnInit {

  schedules: Schedule[];
 
  selectedSchedule: Schedule;

  constructor(private scheduleService: ScheduleService) { }

  getSchedules(): void {
    this.schedules = [];
    this.scheduleService.getSchedules()
      .subscribe(schedules => this.schedules = schedules);
  }

  addnew(name: string): void {
    var newschedule = new Schedule(
      null, name,
      new JobRecurrence(null, null, null, null,
        new JobRecurrenceSchedule(null, null, null, null, null)),
        new ScheduleMessage("none", [], [], [])
    );
    this.selectedSchedule = newschedule;
  }

  onSelect(schedule: Schedule): void {
    this.selectedSchedule = schedule;
  }

  ngOnInit() {
    this.getSchedules();
  }

  formsubmitted(submitted: boolean) {
    if (submitted) {
      console.log('schedules.component.formsubmitted');
      this.scheduleService.createUpdateSchedule(this.selectedSchedule)
        .subscribe(
          () => {
            this.selectedSchedule = null;
            this.getSchedules();
          });
    }
  }

}
