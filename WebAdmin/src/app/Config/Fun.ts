import { FormGroup } from '@angular/forms';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { TemplateRef } from '@angular/core';

export class Fun {

    //加载对话框
    public static loader: NbDialogRef<any>;
    //加载显示对象
    public static loadingDiv: TemplateRef<any>;

    //提示框
    public static dialogDiv: TemplateRef<any>;

    private static dialogService: NbDialogService
    public static init(_dialogService: NbDialogService, _loadingDiv: TemplateRef<any>, _dialogDiv: TemplateRef<any>) {
        this.dialogService = _dialogService;
        this.loadingDiv = _loadingDiv;
        this.dialogDiv = _dialogDiv;
    }


    /**
     * 处理验证信息
     * 
     * @param {FormGroup} userForm 
     * @param {any} validationMessages 
     * @returns 
     * @memberof CommonService
     */
    public static FormValidMsg(userForm: FormGroup, validationMessages) {
        if (validationMessages == null) validationMessages = {};
        var formErrors: any = {};
        if (!userForm) {
            return;
        }
        let defautMsg = {
            'required': () => {
                return "不能为空";
            },
            'minlength': (ent: any) => {
                return "长度不能少于" + ent.requiredLength + "个,当前是" + ent.actualLength + "个";
            },
            'maxlength': (ent: any) => {
                return "长度不能超过" + ent.requiredLength + "个,当前是" + ent.actualLength + "个";
            }
        };
        for (const field in userForm.value) {
            const control = userForm.get(field);
            if (!control.valid) {
                // 默认名称为字段
                let keyName = field;
                // 错误的信息
                let keyMesg = "";

                if (typeof (validationMessages) == "object") {
                    //是否配置了中文名称
                    if (validationMessages[field]['aliasName'] != null) {
                        keyName = validationMessages[field]['aliasName'];
                    }
                }
                else if (typeof (validationMessages) == "string") {
                    keyName = validationMessages + "." + field
                }
                console.log(control.errors)

                //所有错误
                for (const key in control.errors) {
                    console.log(key)
                    //判断是否有配置
                    if (typeof (validationMessages) == "object") {
                        const messages = validationMessages[field];
                        //是否配置了错误信息
                        if (messages[key] != null) {
                            keyMesg += messages[key];
                        }
                        else {
                            if (defautMsg[key] != null) {
                                keyMesg += defautMsg[key](control.errors[key]);
                            }
                            else {
                                keyMesg += key;
                            }
                        }
                    }
                    else {
                        if (defautMsg[key] != null) {
                            keyMesg += defautMsg[key](control.errors[key]);
                        }
                        else {
                            keyMesg += key;
                        }
                    }


                }
                formErrors[keyName] = keyMesg;
            }
        }

        let errMsg = "";
        for (const field in formErrors) {
            errMsg += "" + field + "：" + formErrors[field] + "、"
        }
        return { "ErrorItemArr": formErrors, "ErrorMessage": errMsg };
    }

    /**
     * 显示加载
     * 
     * @param {string} [msg='{{"public.refreshingText"|translate}}'] 
     * @memberof CommonService
     */
    public static ShowLoading(msg = "数据加载中....") {
        this.loader = this.dialogService.open(this.loadingDiv, { context: msg,closeOnBackdropClick:false,closeOnEsc:false });
        setTimeout(() => {
            this.loader.close();
        }, 5*1000);
    };

    /**
     * 隐藏加载
     * 
     * @memberof CommonService
     */
    public static HideLoading() {
        if (this.loader != null) {
            this.loader.close();
            this.loader = null;
        }
    }

    /**
     * 提示信息
     * 
     * @param {string} msg 
     * @param {string} [title=null] 
     * @returns 
     * @memberof CommonService
     */
    public static async Hint(msg: string, title: string = null) {
        if (msg == null || msg == '') {
            return;
        }
        if (title == null || title == '') {
            title = "提示";
        }
        try {
            this.dialogService.open(this.dialogDiv, {
                context: {
                    header: title.replace(/\r/g, "<br />"),
                    subHeader: msg.replace(/\\r\\n/g, "<br />"),
                    buttons: ["确定"]
                }
            });


        } catch (e) {
            this.dialogService.open(this.dialogDiv, {
                context: {
                    header: title,
                    subHeader: msg,
                    buttons: ["确定"]
                }
            });
        }
    }
    public static FormErrMsg(control: any) {
        if (!control) {
            return;
        }
        let defautMsg = {
            'required': () => {
                return "不能为空";
            },
            'minlength': (ent: any) => {
                return "长度不能少于" + ent.requiredLength + "个,当前是" + ent.actualLength + "个";
            },
            'maxlength': (ent: any) => {
                return "长度不能超过" + ent.requiredLength + "个,当前是" + ent.actualLength + "个";
            }
        };;
        let keyMesg = "";

        if (!control.valid) {
            //所有错误
            for (const key in control.errors) {
                //判断是否有配置
                if (defautMsg[key] != null) {
                    keyMesg += defautMsg[key](control.errors[key]);
                }
                else {
                    keyMesg += key;
                }
            }
        }

        return keyMesg
    }
}