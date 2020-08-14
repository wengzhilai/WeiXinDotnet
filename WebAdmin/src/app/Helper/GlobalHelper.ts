import { LocalStorageHelper } from "./LocalStorageHelper";
export class GlobalHelper {



	public static SetToken(token: string) {
		console.log("保存Token：" + token)
		LocalStorageHelper.DeleteCookie("token");
		console.log("读取：" + token)
		console.log(LocalStorageHelper.GetCookie('token'));
		LocalStorageHelper.SetCookie("token", token);
	}

	public static GetToken() {
		return LocalStorageHelper.GetCookie('token');
	}


	public static SetUserObject(user: any) {
		console.log("保存UserObject对象：" )
		console.log(user);
		if (user == null || user == "") {
			LocalStorageHelper.DeleteCookie("UserObject");
		}
		else {
			LocalStorageHelper.SetObjectCookie("UserObject", user);
		}
	}
	public static GetUserObject():any {
		return LocalStorageHelper.GetObjectCookie('UserObject');
	}


	/**
	 * 是否登录
	 */
	static _IsLogin: boolean;
	public static get IsLogin(): boolean {
		let toke = this.GetToken()
		if (toke == null || toke == "")
			this._IsLogin = false
		else
			this._IsLogin = true
		return this._IsLogin
	}
	public static set IsLogin(val: boolean) {
		this._IsLogin = val;
	}
	/**
	 * 退出登录，并清除所有登录信息
	 */
	public static LoginOut(): void {
		console.log("退出登录")
		LocalStorageHelper.DeleteCookie("token");
		LocalStorageHelper.SetCookie("token","");
		LocalStorageHelper.DeleteCookie("UserObject");
		this.IsLogin = false
	}

}