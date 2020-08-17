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
        this.loader = this.dialogService.open(this.loadingDiv, { context: msg, closeOnBackdropClick: false, closeOnEsc: false });
        setTimeout(() => {
            if (this.loader != null)
                this.loader.close();
        }, 5 * 1000);
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



    /**
    * 获取类的所有属性
    * 
    * @param {any} bean 传入的类
    * @param {any} cfg {id:{editable:false}}读取配置是否可以编辑
    * @returns 返回类的所有字段的字符串，以逗号分隔
    * @memberof CommonService
    */
    public static GetBeanNameStr(bean: any, cfg: any) {
        var tmpArr = [];
        for (var item in bean) {
            var objV = bean[item];
            if (typeof (objV) != "string" || objV.indexOf('/Date(') != 0) {
                tmpArr.push(item);
            }
        }
        if (cfg != null) {
            for (let index = 0; index < tmpArr.length; index++) {
                const element = tmpArr[index];
                if (cfg[element] != null && cfg[element]["editable"] != null && cfg[element]["editable"] == false) {
                    tmpArr.splice(index, 1);
                }
            }

        }
        return tmpArr;
    }

    /**
     * 格式化日期
     * 
     * @param {Date} dt 
     * @param {string} fmt 
     * @returns 
     * @memberof CommonService
     */
    public static DateFormat(dt: Date, fmt: string) {
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
    };

    public static UrlToJosn(url: String): any {
        console.log("url转josn");
        if (url == null || url == "" || url.indexOf('?') == -1) return {};
        let arr = url.split('?')[1].split('&');   //先通过？分解得到？后面的所需字符串，再将其通过&分解开存放在数组里
        let obj = {};
        for (let i of arr) {
            obj[i.split('=')[0]] = i.split('=')[1];  //对数组每项用=分解开，=前为对象属性名，=后为属性值
        }
        console.log(obj);
        return obj;
    }

}