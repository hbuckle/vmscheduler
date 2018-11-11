import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { SchedulesComponent } from './schedules/schedules.component';
import { LogMessagesComponent } from './logmessages/logmessages.component';
import { ScheduleDetailComponent } from './schedule-detail/schedule-detail.component';
import { ScheduleFormComponent } from './schedule-form/schedule-form.component';
import { DaypickerComponent } from './daypicker/daypicker.component';
import { ResourcenamePipe } from './resourcename.pipe';
import { DragulaModule } from 'ng2-dragula';

@NgModule({
  declarations: [
    AppComponent,
    SchedulesComponent,
    LogMessagesComponent,
    ScheduleDetailComponent,
    ScheduleFormComponent,
    DaypickerComponent,
    ResourcenamePipe
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    DragulaModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
