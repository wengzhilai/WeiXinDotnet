import { Component } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpHelper } from "../../../Helper/HttpHelper";
import { Fun } from "../../../Config/Fun";
import { GlobalHelper } from "../../../Helper/GlobalHelper";
import { DtoResultObj } from "../../../Model/DtoRec/DtoResult";
import { UserDto } from "../../../Model/DtoRec/UserDto";


@Component({
    selector: 'auth-login',
    templateUrl: 'auth-login.html',
})
export class AuthLoginPage {
    i18n = 'Login'
    submitted=""
    rememberMe = false;
    userForm: FormGroup;
    promise: Promise<string>;
    constructor(
      protected router: Router,
      private formBuilder: FormBuilder,
      public httpHelper: HttpHelper,
    ) {
  
    }
    ngOnInit() {
      this.userForm = this.formBuilder.group({
        loginName: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
        password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]]
      });
      // this.promise=this.getPromise('aaa');
    }

    getPromise(obj): Promise<string> {
      return new Promise((resolve, reject) => {
          setTimeout(() => {
              resolve('Promise with AsyncPipe complete!'+obj);
          }, 2000);
      });
  }
  
    async submit() {
  
      if (this.userForm.invalid) {
        let formErrors = Fun.FormValidMsg(this.userForm, this.i18n);
        Fun.Hint(formErrors.ErrorMessage, "输入无效")
        return;
      }
      //认证登录
      Fun.ShowLoading();
      GlobalHelper.SetToken(null);
      this.httpHelper.Post("user/Login/userLogin", this.userForm.value).then((result: DtoResultObj<UserDto>) => {
        Fun.HideLoading();
        if (result.success) {
          GlobalHelper.SetToken(result.code)
          GlobalHelper.SetUserObject(result.data)
          this.router.navigateByUrl("pages");
        }
        else {
          Fun.Hint(result.msg);
        }
        console.log(result);
      })
    }
    GetErrMsg(obj: any) {
      return Fun.FormErrMsg(obj)
    }
  }