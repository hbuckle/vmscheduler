import { Component, OnInit, Input } from '@angular/core';
import { Schedule } from '../schedule';

@Component({
  selector: 'app-schedule-detail',
  templateUrl: './schedule-detail.component.html',
  styleUrls: ['./schedule-detail.component.css']
})
export class ScheduleDetailComponent implements OnInit {

  @Input() schedule: Schedule;

  constructor() { }

  ngOnInit() {
  }

}
