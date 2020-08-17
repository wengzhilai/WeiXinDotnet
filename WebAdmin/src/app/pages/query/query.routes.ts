import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QueryListPage } from './query-list/query-list';

import { QueryQueryComponent } from './query/query';
import { QueryComponent } from './query.component';

export const routes: Routes = [
  {
    path: '',
    component: QueryComponent,
    children: [
      {
        path: 'list',
        component: QueryListPage,
      },
      {
        path: 'query',
        component: QueryQueryComponent,
      }
    ],
  },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class QueryRoutingModule { }
