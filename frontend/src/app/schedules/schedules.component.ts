import { Component, OnInit } from '@angular/core';
import { Schedule } from '../schedule';
import { ScheduleService } from '../schedule.service';

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
    this.scheduleService.getSchedules()
      .subscribe(schedules => this.schedules = schedules);
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
        .subscribe(() => this.selectedSchedule = null);
    }
  }

}
