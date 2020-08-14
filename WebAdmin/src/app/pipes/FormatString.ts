import { Pipe, PipeTransform } from '@angular/core';
import { TranslateService } from '../Lib/ngx-translate/public_api';

/**
 * Generated class for the FormatStringPipe pipe.
 *
 * See https://angular.io/docs/ts/latest/guide/pipes.html for more info on
 * Angular Pipes.
 */
@Pipe({
  name: 'formatString',
})
export class FormatStringPipe implements PipeTransform {

  constructor(private translate: TranslateService) {
  }
  /**
   * Takes a value and makes it lowercase.
   */
  transform(value: string, ...args) {


    if (value != null && typeof (value) == "string") {

      //(?<={{) 表示 匹配{{前面的位置
      //(.+?) 最小匹配
      // let regList=value.match(/(?<={{)(.+?)(?=}})/g)
      let tempStr = value;
      let regList = value.match(/{{(.+?)}}/g)
      if (regList != null) {
        for (let index = 0; index < regList.length; index++) {
          let element = regList[index];
          element = element.replace(/{{|}}/g, "");
          let v = this.translate.instant(element)
          tempStr = tempStr.replace(new RegExp("{{" + element + "}}", "gm"), v);
        }
      }

      tempStr = tempStr.replace(/\r/g, '</ion-col></ion-row><ion-row class="row"><ion-col class="col">');
      tempStr = tempStr.replace(/\|/g, '</ion-col><ion-col class="col">');
      tempStr = '<ion-row class="row"><ion-col class="col">' + tempStr + "</ion-col></ion-row>"
      return tempStr;
      // return value.replace(/\r/g, '<br />');
    }
    else {
      return value;
    }
  }
}
