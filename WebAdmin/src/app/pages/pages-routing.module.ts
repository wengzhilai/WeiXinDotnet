import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { PagesComponent } from './pages.component';


const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'query',
      loadChildren: () => import('./query/query.module')
        .then(m => m.QueryModule),
    },
    {
      path: '',
      redirectTo: 'query',
      pathMatch: 'full',
    },
    {
      path: '**',
      component: PagesComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
