import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Schedule } from '../schedule';

@Component({
  selector: 'app-schedule-form',
  templateUrl: './schedule-form.component.html',
  styleUrls: ['./schedule-form.component.css']
})
export class ScheduleFormComponent implements OnInit {

  frequencies = ['Minute', 'Hour', 'Day', 'Week', 'Month']

  @Input() schedule: Schedule;
  @Output() submitted = new EventEmitter<boolean>();

  dayschanged(days: string[]) {
    this.schedule.recurrence.schedule.weekDays = days;
  }

  onSubmit() {
    this.submitted.emit(true);
  }

  constructor() { }

  ngOnInit() {
  }

}
