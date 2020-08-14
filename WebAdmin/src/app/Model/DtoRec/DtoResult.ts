export class DtoResult {
    /**
     * 是否成功
     */
    success:boolean;
    /**
     * 返回代码
     */
    code:string;
    /**
     * 返回描述
     */
    msg:string;
}

export class DtoResultObj<T> extends DtoResult {


    /**
     * 数据
     */
    data:T;
    /**
     * 数据列表
     */
    dataList:T[];
}