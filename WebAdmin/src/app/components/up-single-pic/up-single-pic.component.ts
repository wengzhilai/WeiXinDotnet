import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

import { FileItem, ParsedResponseHeaders, FileUploader } from '../../Lib/ng2-file-upload';
import { Variables } from '../../Config/Variables';

@Component({
  selector: 'ngx-up-single-pic',
  templateUrl: './up-single-pic.component.html',
  styleUrls: ['./up-single-pic.component.scss']
})
export class UpSinglePicComponent  implements OnInit {
  @Input()
  CanEdit: boolean = true

  @Input()
  pathType: string = "pathType"
  
  @Output()
  ChangeFileJson: EventEmitter<any> = new EventEmitter<any>();


  /**
  * 自定义model变量
  */
  private _myModel;
  /**
   * 返回父组件变化后的值
   */
  @Input()
  get myModel() {
    return this._myModel;
  }

  /**
   * 组件值产生变化后父组件改变
   * @param value
   */
  set myModel(value) {
    this._myModel = value;
    this.myModelChange.emit(value);
  }
  @Output()
  myModelChange: EventEmitter<any> = new EventEmitter();

  uploader: FileUploader 
  
  singleEnt:any;
  lookPicPath:string;
  constructor(
  ) {
    console.log("UpFileComponent")
  }

  ngOnInit(): void {
    console.log('ionViewDidLoad UpFileComponent');
    this.uploader= new FileUploader({
      url: Variables.Api_Upfile,
      method: "POST",
      itemAlias: "file",
      headers:[{
        name: "pathType",
        value: this.pathType
      }],
      autoUpload: true,
      allowedFileType: ["image", "xls", "txt"],
    });

    if(this.myModel!=null && this.myModel!='null'){
      this.lookPicPath=Variables.Api_LookUpfile+this.myModel;
    }

    //上传一个文件成功的回调 
    this.uploader.onWhenAddingFileFailed = (item: any, filter: any, options: any) => {
      console.log(item)
      console.log(filter)
      console.log(options)
      console.log('选择文件类型不允许')
      console.log('该文件类型无效：[' + item.name + '] 只允许上传[' + options.allowedFileType.join(',') + ']')
    }

    this.uploader.onAfterAddingFile = (item => {
      item.withCredentials = false
    })
    this.uploader.onCompleteAll = () => {
      console.log('上传完成')
      this.ReturnJson();
    };
    this.uploader.onSuccessItem = (item: FileItem, response: string, status: number, headers: ParsedResponseHeaders) => {
      console.log("成功一个")
      console.log(response)

      this.singleEnt = JSON.parse(response).data;
      // this.lookPicPath=Variables.Api_LookUpfile+this.singleEnt.fileName+"?path="+this.singleEnt.filePath;
      this.lookPicPath=Variables.Api_LookUpfile+this.singleEnt.url;
      
    };
    this.uploader.onAfterAddingAll = (fileItems: any) => {
      console.log("添加完所有的")
      console.log(fileItems)
    }

  }

  ReturnJson() {
    console.log('返回值')
    this.myModelChange.next(this.singleEnt.url)
  }
}