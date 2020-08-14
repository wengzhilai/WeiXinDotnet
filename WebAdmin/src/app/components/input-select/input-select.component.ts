import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { HttpHelper } from '../../Helper/HttpHelper';
import { DtoResultObj } from '../../Model/DtoRec/DtoResult';

@Component({
  selector: 'input-select',
  templateUrl: './input-select.component.html',
  styleUrls: ['./input-select.component.scss']
})
export class InputSelectComponent implements OnInit {

  /**
   * 接口地址
   */
  @Input()
  ApiUrl: string

  /**
   * 允许多选
   */
  @Input()
  multiple: boolean

  @Input()
  PostEnt: any
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

  ListData: any;
  constructor(
    public httpHelper: HttpHelper,
  ) { 

  }

  ngOnInit() {
    console.log("input-select的参数")
    console.log("ApiUrl："+this.ApiUrl);
    console.log("multiple:"+this.multiple);

    this.LoadSelectOption();
    
  }

  LoadSelectOption() {
    console.log(this._myModel)
    // Fun.ShowLoading();
    this.httpHelper.Post(this.ApiUrl, this.PostEnt).then((x: DtoResultObj<any>) => {
      // Fun.HideLoading();
      if (x.success) {
        this.ListData = x.dataList;
      }
      else{
      }
    });
  }

}
