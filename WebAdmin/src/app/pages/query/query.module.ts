import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QueryComponent } from "./query.component";
import { IndexComponent } from './index/index.component';
import { routing } from "./query.routing";


@NgModule({
  declarations: [
    QueryComponent,
    IndexComponent,
  ],
  imports: [
    routing,
    CommonModule
  ]
})
export class QueryModule { }
