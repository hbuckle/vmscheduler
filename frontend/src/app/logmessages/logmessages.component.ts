import { Component, OnInit } from '@angular/core';
import { LogMessageService } from '../logmessage.service';

@Component({
  selector: 'app-messages',
  templateUrl: './logmessages.component.html',
  styleUrls: ['./logmessages.component.css']
})
export class LogMessagesComponent implements OnInit {

  constructor(public logMessageService: LogMessageService) {}

  ngOnInit() {
  }

}
