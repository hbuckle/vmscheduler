import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Schedule } from './schedule';
import { MessageService } from './message.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  private schedulesUrl = '';

  constructor(
    private messageService: MessageService,
    private http: HttpClient) { }

  getSchedules(): Observable<Schedule[]> {
    this.messageService.add('ScheduleService: fetching schedules');
    return this.http.get<Schedule[]>(this.schedulesUrl);
  }
}
