import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { 
  NbSidebarModule, 
  NbMenuModule,
  NbDialogModule,
  NbCardModule,
  NbButtonModule,
  NbWindowModule,
  } from '@nebular/theme';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ThemeModule } from './@theme/theme.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    RouterModule,
    BrowserModule,
    HttpClientModule,


    AppRoutingModule,
    BrowserAnimationsModule,
    
    NbSidebarModule.forRoot(), 
    NbMenuModule.forRoot(),
    NbDialogModule.forRoot(),
    NbWindowModule.forRoot(),
    NbCardModule,
    NbButtonModule,
    ThemeModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
