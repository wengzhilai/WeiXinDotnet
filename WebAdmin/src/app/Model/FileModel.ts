/**
 * 文件
 * 
 * @export
 * @class FileModel
 */
export class FileModel {
    constructor() { }
    /**
     * ID
     * 
     * @type {number}
     * @memberof FileModel
     */
    ID: number;
    /**
     * 名称
     * 
     * @type {string}
     * @memberof FileModel
     */
    NAME: string;
    
    /**
     * 路径
     * 
     * @type {string}
     * @memberof FileModel
     */
    PATH: string;
    
    /**
     * USER_ID
     * 
     * @type {number}
     * @memberof FileModel
     */
    USER_ID: number;
    
    /**
     * 大小
     * 
     * @type {number}
     * @memberof FileModel
     */
    LENGTH: number;
    
    /**
     * 添加时间
     * 
     * @type {Date}
     * @memberof FileModel
     */
    UPLOAD_TIME: Date;
    
    /**
     * 备注
     * 
     * @type {string}
     * @memberof FileModel
     */
    REMARK: string;
    
    /**
     * 相对路径
     * 
     * @type {string}
     * @memberof FileModel
     */
    URL: string;
    
    /**
     * 文件类型
     * 
     * @type {string}
     * @memberof FileModel
     */
    FILE_TYPE: string;
    /**
     * 排序
     */
    indexNo:number;
    /**
     * 主键
     * 
     * @type {string}
     * @memberof FileModel
     */
    key:string;
}