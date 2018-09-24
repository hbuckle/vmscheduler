import { Component, OnInit, Input } from '@angular/core';
import { Schedule } from '../schedule';

@Component({
  selector: 'app-schedule-form',
  templateUrl: './schedule-form.component.html',
  styleUrls: ['./schedule-form.component.css']
})
export class ScheduleFormComponent implements OnInit {

  frequencies = ['Minute', 'Hour', 'Day', 'Week', 'Month']

  @Input() schedule: Schedule;

  constructor() { }

  ngOnInit() {
  }

}
