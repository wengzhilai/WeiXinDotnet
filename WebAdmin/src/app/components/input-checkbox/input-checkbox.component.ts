import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { HttpHelper } from '../../Helper/HttpHelper';

@Component({
  selector: 'input-checkbox',
  templateUrl: './input-checkbox.component.html',
  styleUrls: ['./input-checkbox.component.scss']
})
export class InputCheckboxComponent implements OnInit {



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



  checked: boolean;
  constructor(
    public httpHelper: HttpHelper,
  ) { 

  }

  ngOnInit() {
    console.log("input-checkBox的参数")
    console.log("myModel"+this.myModel);
    if(this.myModel==1 || this.myModel=="1" ){
      this.checked=true;
    }
    else{
      this.checked=false;
    }
    
  }


  toggle(checked: boolean) {
    console.log(checked)
    if(checked){
      this.myModel=1;
    }
    else{
      this.myModel=0;
    }
  }
}
