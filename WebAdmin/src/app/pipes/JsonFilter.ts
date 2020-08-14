import { Pipe, PipeTransform } from '@angular/core';

/**
 * 过滤JSON数据
 * 格式：<li *ngFor="let it of its | JsonFilter : 'name' : 'value or variable'">{{it}}</li>
 */
@Pipe({
  name: 'JsonFilter',
})
export class JsonFilterPipe implements PipeTransform {
   transform(items: any[], field : string, value : string,type:number=0): any[] {  
        if (!items) return [];
        if(type==0)
          {
            return items.filter(it => it[field] == value);
          }
          else{
            return items.filter(it => it[field] != value);
          }  
    }
}
