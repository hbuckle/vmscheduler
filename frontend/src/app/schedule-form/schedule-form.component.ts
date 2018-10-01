import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Schedule } from '../schedule';
import { ResourcesService } from '../resources.service';

@Component({
  selector: 'app-schedule-form',
  templateUrl: './schedule-form.component.html',
  styleUrls: ['./schedule-form.component.css']
})
export class ScheduleFormComponent implements OnInit {

  frequencies = ['Minute', 'Hour', 'Day', 'Week', 'Month']
  resourceGroups: string[];

  @Input() schedule: Schedule;
  @Output() submitted = new EventEmitter<boolean>();

  dayschanged(days: string[]) {
    this.schedule.recurrence.schedule.weekDays = days;
  }

  onSubmit() {
    this.submitted.emit(true);
  }

  getResources(): void {
    this.resourceGroups = [];
    this.resourcesService.getResourceGroups()
      .subscribe(rgs => this.resourceGroups = rgs);
  }

  addTime(hour: number, minute: number) {
    if (this.schedule.recurrence.schedule.hours === null) {
      this.schedule.recurrence.schedule.hours = [];
    }
    if (this.schedule.recurrence.schedule.minutes === null) {
      this.schedule.recurrence.schedule.minutes = [];
    }
    let newhours = this.schedule.recurrence.schedule.hours.slice(0);
    newhours.push(hour);
    this.schedule.recurrence.schedule.hours = newhours;
    let newminutes = this.schedule.recurrence.schedule.minutes.slice(0);
    newminutes.push(minute);
    this.schedule.recurrence.schedule.minutes = newminutes;
  }

  constructor(private resourcesService: ResourcesService) { }

  ngOnInit() {
    this.getResources();
  }

}
