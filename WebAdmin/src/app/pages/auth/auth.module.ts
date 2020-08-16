import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { NbLayoutModule, NbCardModule, NbCheckboxModule, NbInputModule, NbAlertModule, NbButtonModule } from '@nebular/theme';


import { routes } from './auth.routes';
import { AuthLoginPage } from './login/auth-login';
import { AuthRoutingModule } from './auth.routes';
import { AuthBlockPage } from "./auth-block/auth-block.component";
import { AuthPages } from "./auth.component";



@NgModule({
  imports: [
    CommonModule,
    NbLayoutModule,
    NbCardModule,
    NbCheckboxModule,
    NbAlertModule,
    NbInputModule,
    NbButtonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AuthRoutingModule,
  ],
  declarations: [
    AuthLoginPage,
    AuthBlockPage,
    AuthPages,
  ],
  entryComponents: [
    AuthLoginPage,
  ],
  exports: [
    AuthLoginPage,
    AuthBlockPage,
    AuthPages,
  ],
})
export class AuthModule {
}
