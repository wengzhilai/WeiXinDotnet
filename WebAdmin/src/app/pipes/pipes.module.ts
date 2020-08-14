import { NgModule } from '@angular/core';
import { DataStrToStringPipe } from './DataStrToString';
import { DateToStringPipe } from './DateToString';
import { FormatStringPipe } from './FormatString';
import { JsonFilterPipe } from './JsonFilter';
import { MyDecimalPipe,MyPercentPipe,MyCurrencyPipe } from './MyDecimalPipe';
import { ImgUrlPipe } from './ImgUrl';

@NgModule({
  declarations: [
    DataStrToStringPipe,
    DateToStringPipe,
    FormatStringPipe,
    JsonFilterPipe,
    MyDecimalPipe,
    MyPercentPipe,
    MyCurrencyPipe,
    ImgUrlPipe,
    ],
  exports: [
    DataStrToStringPipe,
    DateToStringPipe,
    FormatStringPipe,
    JsonFilterPipe,
    MyDecimalPipe,
    MyPercentPipe,
    MyCurrencyPipe,
    ImgUrlPipe,
  ]
})
export class PipesModule { }
