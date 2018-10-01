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
import { ListswapperComponent } from './listswapper/listswapper.component';
import { ResourcenamePipe } from './resourcename.pipe';

@NgModule({
  declarations: [
    AppComponent,
    SchedulesComponent,
    LogMessagesComponent,
    ScheduleDetailComponent,
    ScheduleFormComponent,
    DaypickerComponent,
    ListswapperComponent,
    ResourcenamePipe
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
