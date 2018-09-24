import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Schedule } from './schedule';
import { LogMessageService } from './logmessage.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  private schedulesUrl = '';

  constructor(
    private logMessageService: LogMessageService,
    private http: HttpClient) { }

  getSchedules(): Observable<Schedule[]> {
    this.logMessageService.add('ScheduleService: fetching schedules');
    return this.http.get<Schedule[]>(this.schedulesUrl);
  }
}
