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
export class ResourcesService {

  private functionname = 'xxx';

  private getresourcegroupsUrl() {
    return `https://${this.functionname}.azurewebsites.net/api/getresourcegroups`;
  }

  getResourceGroups(): Observable<string[]> {
    this.logMessageService.add('ResourcesService: fetching resource groups');
    return this.http.get<string[]>(this.getresourcegroupsUrl());
  }

  private log(message: string) {
    this.logMessageService.add(message);
  }

  constructor(
    private logMessageService: LogMessageService,
    private http: HttpClient) { }
}
