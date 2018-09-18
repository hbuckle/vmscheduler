import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { SchedulesComponent } from './schedules/schedules.component';
import { MessagesComponent } from './messages/messages.component';
import { ScheduleDetailComponent } from './schedule-detail/schedule-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    SchedulesComponent,
    MessagesComponent,
    ScheduleDetailComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
