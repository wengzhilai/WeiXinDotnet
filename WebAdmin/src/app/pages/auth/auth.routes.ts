import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthLoginPage } from './login/auth-login';
import { AuthPages } from "./auth.component";
export const routes: Routes = [
  {
    path: '',
    component: AuthPages,
    children: [
      {
        path: '',
        component: AuthLoginPage,
      },
      {
        path: 'login',
        component: AuthLoginPage,
      }
    ],
  },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule { }

export const routedComponents = [
  AuthLoginPage,
  AuthPages,
];