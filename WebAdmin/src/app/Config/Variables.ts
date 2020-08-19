import { KeyValuePair } from "../Model/KeyValuePair";
export class Variables {

  constructor() { }

  static System:string="study";

  /**
   * 调试模式
   */
  static Debug:boolean = false;

  /**
   * 分页数
   */
  static PageSize:number = 10;

  /**
   * 接口地址
   */
  static Api:string = 'http://t1.ngrok.wjbjp.cn/';
  static ImgUrl:string = Variables.Api+'file/UpFile/LookfileByPath/';
  // static Api:string = 'http://coreapi.wjbjp.cn/';
  // static ImgUrl:string = 'http://coreapi.wjbjp.cn/';
  // static Api:string = 'http://dotnetapi.wjbjp.cn/Api/';
  // static ImgUrl:string = 'http://dotnetapi.wjbjp.cn/';
   
  /**
   * 图片地址
   */
  public static _api:string = Variables.Api;
  public static _imgUrl:string = Variables.ImgUrl;


  /**
   * 保存接收的文件目录
   */
  static savePhoneTempPath;

  /**
   * 下载多语言是否成功
   */
  static LoadLanguageSucc=false;
  /**
   * 下载多语言的路径
   */
  static LoadLanguageSuccPath=""

  /**
   * 上传文件接口
   */
  static get Api_Upfile(){
    return "http://127.0.0.1:5000/file/UpFile/UploadPhotos";
  }

  /**
   * 查看图片地址
   */
  static get Api_LookUpfile(){
    return "http://127.0.0.1:5000/file/UpFile/LookfileByPath/";
  }


  
  static LanguageArray:Array<KeyValuePair> = [
    <KeyValuePair>{Key:"ch",Value:"简体中文"},
    // <KeyValuePair>{Key:"en",Value:"English"}
  ] //设置语言

  static AllFileMIME:Array<KeyValuePair> = [
    <KeyValuePair>{Type:"ppt",Key:"application/vnd.ms-powerpoint",Value:"ppt pps pot"},
    <KeyValuePair>{Type:"txt",Key:"application/pdf",Value:"txt text conf def list log in"},
    <KeyValuePair>{Type:"doc",Key:"application/msword",Value:"doc dot"},
    <KeyValuePair>{Type:"doc",Key:"application/vnd.openxmlformats-officedocument.wordprocessingml.document",Value:"docx"},
    <KeyValuePair>{Type:"xls",Key:"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",Value:"xlsx"},
    <KeyValuePair>{Type:"ppt",Key:"application/vnd.openxmlformats-officedocument.presentationml.presentation",Value:"pptx"},
    <KeyValuePair>{Type:"xls",Key:"application/vnd.ms-excel",Value:"xls xlm xla xlc xlt xlw"},
    <KeyValuePair>{Type:"pdf",Key:"application/pdf",Value:"pdf"},
    <KeyValuePair>{Type:"apk",Key:"application/vnd.android.package-archive",Value:"apk"},
    <KeyValuePair>{Type:"zip",Key:"application/zip",Value:"zip"},
    <KeyValuePair>{Type:"zip",Key:"application/x-7z-compressed",Value:"7z"},
    <KeyValuePair>{Type:"rar",Key:"application/x-rar-compressed",Value:"rar"},
  ] //设置语言

  /**
   * 是否首页订阅，
   */
  static homeSubscribeNotification:boolean=false;

  //是否tab加载了订阅
  static homeMarkNotification:boolean=false;
  //是否登录加载了订阅
  static loginSubscribeNotification:boolean=false;


  /**
   * 是否允许选择文件
   */
  static isAllowUpfile:boolean = false; 
  /**
   * 允许选择图片数量
   */
  static maximumImagesCount:number = 1;
  /**
   * 上传图片的质量
   * 
   */
  static quality:number = 50;
  /**
   * 拍照后是否允许简单编辑
   * 
   */
  static isAllowEdit:boolean = false; 
  /**
   * 允许编辑图片的最大宽
   * 
   * @static
   * @type {number}
   * @memberof Config
   */
  static imgWidth:number = 800;
  /**
   * 允许编辑图片的最大高
   * 
   * @static
   * @type {number}
   * @memberof Config
   */
  static imgHeight:number = 800;
  /**
   * 摄后将图像保存到设备上的相册
   * 
   * @static
   * @type {boolean}
   * @memberof Config
   */
  static isSaveToPhotoAlbum:boolean = false;  
  /**
   * 选择要使用的相机（正面或背面）。在Camera.Direction中定义。默认为BACK。返回：0前：1
   * 
   * @static
   * @type {boolean}
   * @memberof Config
   */
  static isCorrectOrientation:boolean = true;   //拍摄方向限制
}