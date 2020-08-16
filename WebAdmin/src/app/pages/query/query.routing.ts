import { Routes, RouterModule } from '@angular/router';
import { QueryComponent } from "./query.component";
import { IndexComponent } from "./index/index.component";
const routes: Routes = [
    {
      path: '',
      component: QueryComponent,
      children: [
        { path: 'index', component: IndexComponent },
      ]
    }
  ];
  
  export const routing = RouterModule.forChild(routes);