import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Variables } from "../Config/Variables";
import { GlobalHelper } from "./GlobalHelper";
import { DtoResult, DtoResultObj } from "../Model/DtoRec/DtoResult";
import { Observable } from "rxjs";
@Injectable({
  providedIn: 'root'
})
export class HttpHelper {

  constructor(
    private http: HttpClient,
  ) {
  }
  Post(apiName, postBean: any) {
    console.group("开始请求[" + apiName + "]参数：");
    console.time("Post时间");
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };

    var token = GlobalHelper.GetToken()
    if (token.length > 0) {
      httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token,
        })
      };
    }
    console.log(httpOptions)
    console.log("请求参数")
    console.log(JSON.stringify(postBean));
    console.log(postBean)

    return new Promise((resolve, reject) => {
      this.http
        .post(Variables.Api + apiName, postBean, httpOptions)
        .toPromise()
        .then((result: any) => {

          console.log("返回结果：");
          console.log(result);
          console.timeEnd("Post时间");
          console.groupEnd();
          resolve(result);
          // return result;

        }, (error: HttpErrorResponse) => {
          console.error('请求失败:');
          console.error("接中地址：" + Variables.Api + apiName)
          console.error("参数：")
          console.error(postBean)
          console.error("错误信息")
          console.error(error)
          console.timeEnd("Post时间");
          console.groupEnd();

          var errObj: DtoResultObj<any> = new DtoResultObj<any>();
          errObj.success = false;
          switch (error.status) {
            case 401:
              errObj.msg = "登录超时,请重新登录";
              break;
            case 0:
              errObj.msg = "请检查网络";
              break;
            default:
              errObj.msg = error.message;
              break;
          }
          resolve(errObj);
        })
        .catch((error: any) => {
          console.error('请求失败');
          console.error(error);
          console.groupEnd();

          var errObj: DtoResultObj<any> = new DtoResultObj<any>();
          errObj.success = false;
          errObj.msg = error.message;
          resolve(errObj);
        });
    });
  }

  PostToObservable(apiName, postBean: any, callback = null) {

    console.group("开始请求[" + apiName + "]参数：");
    console.time("Post时间");
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };

    var token = GlobalHelper.GetToken()
    if (token.length > 0) {
      httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token,
        })
      };
    }
    console.log(httpOptions)
    console.log("请求参数")
    console.log(postBean)

    return Observable.create((observer) => {
      this.http.post(Variables.Api + apiName, postBean, httpOptions).subscribe({
        next: (res: DtoResultObj<any>) => {
          console.log("返回结果：");
          console.log(res)
          console.timeEnd("Post时间");
          console.groupEnd();
          observer.next(res);
          observer.complete();
        },
        error: err => {
          console.error('请求失败');
          console.error(err);
          var errObj: DtoResultObj<any> = new DtoResultObj<any>();
          errObj.success = false;
          errObj.msg = err.message;

          var errObj: DtoResultObj<any> = new DtoResultObj<any>();
          errObj.success = false;
          switch (err.status) {
            case 401:
              errObj.msg = "登录超时,请重新登录";
              break;
            case 0:
              errObj.msg = "请检查网络";
              break;
            default:
              errObj.msg = err.message;
              break;
          }



          observer.next(errObj);
          observer.complete();
        }
      });
    });
  }

}
