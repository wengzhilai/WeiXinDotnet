import { Component, OnInit } from '@angular/core';
import { NbWindowRef } from '@nebular/theme';
import { DtoResultObj } from '../../Model/DtoRec/DtoResult';

@Component({
  selector: 'ngx-look-model',
  templateUrl: './look-model.component.html',
  styleUrls: ['./look-model.component.scss']
})
export class LookModelComponent implements OnInit {

  title: string;
  messageList: any[] = [];
  buttons = []
  inputs = []
  /**
   * 所有列表的值
   */
  ItmeArr=new Array<any>();
  bean = {}
  screenheight=document.documentElement.clientHeight-300
  constructor(
    public windowRef: NbWindowRef
  ) {
    console.log(this.windowRef.config.context)
  }

  ngOnInit() {
    for (const key in this.inputs) {
      let element = this.inputs[key];
      element["name"]=key
      this.ItmeArr.push(element);
    }
    console.log(this.ItmeArr)
  }
  ButtonClick(even) {
    even(this.bean)
      .then((x: DtoResultObj<any>) => {
        console.log(x)
        if (x.success) {
          this.windowRef.close();
        }
      })
  }


}
