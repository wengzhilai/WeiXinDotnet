export class LocalStorageHelper {
    public static GetCookie(cookieName: string): string {
        var reObj = localStorage.getItem(cookieName);
        return reObj
    }

    public static SetCookie(cookieName: string, value: string, validity?: number, validityType?: string, domain?: string, path?: string, needsSecureConnection?: boolean) {
        localStorage.setItem(cookieName, value);
    }

    public static GetObjectCookie(cookieName: string): object {
        let restr = this.GetCookie(cookieName)
        if (restr == null || restr == "" || restr == "null") {
            return null;
        }
        return JSON.parse(restr)
    }

    public static SetObjectCookie(cookieName: string, obj: object, validity?: number, validityType?: string, domain?: string, path?: string, needsSecureConnection?: boolean) {
        let value: string = JSON.stringify(obj);
        this.SetCookie(cookieName, value, validity, validityType, domain, path, needsSecureConnection)
    }

    public static DeleteCookie(cookieName: string, domain?: string, path?: string) {
        localStorage.removeItem(cookieName);
    }

    public static Exists(cookieName: string): boolean {
        for (let index = 0; index < localStorage.key.length; index++) {
            if (cookieName = localStorage.key[index]) {
                return true
            }
        }
        return false;
    }
}
