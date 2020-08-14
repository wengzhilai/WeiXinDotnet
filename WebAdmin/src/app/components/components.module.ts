import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { TranslateModule } from '../Lib/ngx-translate/public_api';
import { QRCodeModule } from '../Lib/angularx-qrcode/QRCodeModule';
import { PipesModule } from '../pipes/pipes.module';
import { QueryEditComponent } from './query-edit/query-edit.component';
import { InputSelectComponent } from './input-select/input-select.component';
import { TableEditComponent } from './table-edit/table-edit.component';
import { LookModelComponent } from './look-model/look-model.component';
import { QueryFilterComponent } from './query-filter/query-filter.component';
import { EditModelComponent } from './edit-model/edit-model.component';

import {
  NbActionsModule,
  NbButtonModule,
  NbCardModule,
  NbCheckboxModule,
  NbTabsetModule,
  NbDatepickerModule, NbIconModule,
  NbInputModule,
  NbRadioModule,
  NbSelectModule,
  NbUserModule,
} from '@nebular/theme';
import { FileUploadModule } from '../Lib/ng2-file-upload';
import { UpSinglePicComponent } from './up-single-pic/up-single-pic.component';
import { InputCheckboxComponent } from './input-checkbox/input-checkbox.component';
import { Ng2SmartTableModule } from '../Lib/ng2-smart-table/public-api';


@NgModule({
  entryComponents: [
    EditModelComponent, QueryEditComponent,, TableEditComponent,LookModelComponent,
    QueryFilterComponent,UpSinglePicComponent,InputCheckboxComponent
  ],
  declarations: [
    EditModelComponent, QueryEditComponent, InputSelectComponent, TableEditComponent, 
    LookModelComponent,QueryFilterComponent,UpSinglePicComponent,InputCheckboxComponent
  ],
  imports: [
    TranslateModule,
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    PipesModule,
    NbActionsModule,
    NbButtonModule,
    NbCardModule,
    NbCheckboxModule,
    NbTabsetModule,
    NbDatepickerModule, NbIconModule,
    NbInputModule,
    NbRadioModule,
    NbSelectModule,
    NbUserModule,
    Ng2SmartTableModule,
    QRCodeModule,
    FileUploadModule,
  ],
  exports: [
    EditModelComponent,
    QueryEditComponent,
    TableEditComponent,
    LookModelComponent,
    QueryFilterComponent,
    UpSinglePicComponent,
    InputCheckboxComponent,
  ]
})
export class ComponentsModule { }
