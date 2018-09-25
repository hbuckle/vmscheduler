import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Schedule } from './schedule';
import { LogMessageService } from './logmessage.service';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  private functionname = 'xxx';
  private getschedulerjobUrl() {
    return `https://${this.functionname}.azurewebsites.net/api/getschedulerjob`;
  }
  private createupdateschedulerjobUrl(name: string) {
    return `https://${this.functionname}.azurewebsites.net/api/createupdateschedulerjob/${name}?code=xxx`
  }

  constructor(
    private logMessageService: LogMessageService,
    private http: HttpClient) { }

  getSchedules(): Observable<Schedule[]> {
    this.logMessageService.add('ScheduleService: fetching schedules');
    return this.http.get<Schedule[]>(this.getschedulerjobUrl());
  }

  createUpdateSchedule(schedule: Schedule): Observable<any> {
    console.log(schedule);
    return this.http.put(this.createupdateschedulerjobUrl(schedule.name), schedule, httpOptions).pipe(
      tap(_ => this.log(`updated schedule id=${schedule.id}`)),
      catchError(this.handleError<any>('createUpdateSchedule'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      this.log(`${operation} failed: ${error.message}`);
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  private log(message: string) {
    this.logMessageService.add(message);
  }
}
