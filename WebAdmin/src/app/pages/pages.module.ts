import { NgModule } from '@angular/core';
import { NbMenuModule } from '@nebular/theme';

import { PagesComponent } from './pages.component';
import { PagesRoutingModule } from './pages-routing.module';
import { RouterModule } from '@angular/router'; 
import { ThemeModule } from '../@theme/theme.module';
@NgModule({
  imports: [
    PagesRoutingModule,
    RouterModule,
    NbMenuModule,

    ThemeModule,
  ],
  declarations: [
    PagesComponent,
  ],
})
export class PagesModule {
}
