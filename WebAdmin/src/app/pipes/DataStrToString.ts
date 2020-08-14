import { Pipe, PipeTransform } from '@angular/core';

/**
 * Generated class for the DataStrToStringPipe pipe.
 *
 * See https://angular.io/docs/ts/latest/guide/pipes.html for more info on
 * Angular Pipes.
 */
@Pipe({
  name: 'DataStrToString',
})
export class DataStrToStringPipe implements PipeTransform {
  /**
   * Takes a value and makes it lowercase.
   */
  transform(value: string, fmt:string="yyyy-MM-dd hh:mm") {
    if(value==null || value=='0001-01-01T00:00:00') return ""
    if(dt< new Date("1900/1/1")) return ""

    let tmp=value.replace(/-/g,':').replace(' ',':').replace('T',':');
    let valueArr:Array<any>=tmp.split(':');
    for(var i=0;i<6;i++){
      if(valueArr[i]==null){
        valueArr[i]=0;
      }
    }
    
    var dt=new Date(valueArr[0],(valueArr[1]-1),valueArr[2],valueArr[3],valueArr[4],valueArr[5]);
  
    var o = {
      "M+": dt.getMonth() + 1,                 //月份
      "d+": dt.getDate(),                    //日
      "h+": dt.getHours(),                   //小时
      "m+": dt.getMinutes(),                 //分
      "s+": dt.getSeconds(),                 //秒
      "q+": Math.floor((dt.getMonth() + 3) / 3), //季度
      "S": dt.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt))
      fmt = fmt.replace(RegExp.$1, (dt.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
      if (new RegExp("(" + k + ")").test(fmt))
        fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
  }
}
