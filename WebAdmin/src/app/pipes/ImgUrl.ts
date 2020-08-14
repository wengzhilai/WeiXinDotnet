
import { Pipe, PipeTransform } from '@angular/core';
import { Variables } from "../Config/Variables";
@Pipe({ name: 'ImgUrl' })
export class ImgUrlPipe implements PipeTransform {
  transform(url: string, type: string = "imgUrl", defaultPic: string = "./assets/images/noPic.jpg"): string {
    let str = url;
    if (url != null && url != '' && url != undefined && url.indexOf('http://') == -1) {
      if (type == "imgUrl") {
        str = Variables.ImgUrl + url.replace("~", "").replace("/YL", "");
      }
      else {
        str = Variables.Api + url.replace("~", "").replace("/YL", "");
      }
    }
    else if (url == null || url == '' || url == undefined) {
      str = defaultPic
    }
    // console.log("图片地址:" + str)
    return str;
  }
}
