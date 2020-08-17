import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';

import { QueryListPage } from './query-list/query-list';
import { QueryRoutingModule } from './query.routes';
import { QueryQueryComponent } from './query/query';
import { QueryComponent } from './query.component';
import { ComponentsModule } from '../../components/components.module';
import { QueryEditComponent } from '../../components/query-edit/query-edit.component';
import { TableEditComponent } from '../../components/table-edit/table-edit.component';
import {
  NbActionsModule,
  NbAlertModule,
  NbButtonModule,
  NbCardModule,
  NbTabsetModule,
  NbUserModule,
  NbRadioModule,
  NbSelectModule,
  NbListModule,
  NbIconModule,
} from '@nebular/theme';
import { Ng2SmartTableModule } from '../../Lib/ng2-smart-table/public-api';

@NgModule({
  entryComponents: [
    QueryEditComponent,
    TableEditComponent,
  ],
  imports: [
    ThemeModule,
    Ng2SmartTableModule,
    QueryRoutingModule,
    ComponentsModule,
    
    NbActionsModule,
    NbButtonModule,
    NbCardModule,
    NbTabsetModule,
    NbUserModule,
    NbRadioModule,
    NbSelectModule,
    NbListModule,
    NbIconModule,
    NbAlertModule,
  ],
  declarations: [
    QueryListPage,
    QueryQueryComponent,
    QueryComponent,
  ],

  exports: [
  ],
})
export class QueryModule {
}
