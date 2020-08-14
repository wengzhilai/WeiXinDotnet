import { Component, OnInit } from '@angular/core';
import { NbWindowRef } from '@nebular/theme';
import { DtoResultObj } from '../../Model/DtoRec/DtoResult';

@Component({
  selector: 'ngx-query-filter',
  templateUrl: './query-filter.component.html',
  styleUrls: ['./query-filter.component.scss']
})
export class QueryFilterComponent implements OnInit {

  title: string;
  messageList: any[] = [];
  buttons = []
  inputs = []
  /**
   * 所有列表的值
   */
  ItmeArr = new Array<any>();
  bean = {}
  screenheight = document.documentElement.clientHeight - 300
  constructor(
    public windowRef: NbWindowRef,
  ) {
    console.log(this.windowRef.config.context)
  }

  ngOnInit() {
    for (const key in this.inputs) {
      let element = this.inputs[key];
      //是不显示配置了 filterable =false
      if (element["filterable"] != null && element["filterable"] == false) {
        continue;
      }
      //如果没有配置filter，则采用editor的配置
      if (element["filter"] == null) {
        element["filter"] = element["editor"]
      }
      if (element["filter"] == null) {
        element["filter"] = {};
      }

      //生成操作类型
      if (element["filter"].opType == null) {
        let opTypeList = [];
        switch (element["type"]) {
          case "Date":
            opTypeList = [{ opType: "beteen", opName: "区间" }]
            element["filter"].opTypeValue = "beteen";
            break;
          case "int":
            opTypeList = [{ opType: ">", opName: "大于" }, { opType: "<", opName: "小于" }, { opType: "=", opName: "等于" }]
            element["filter"].opTypeValue = ">";
            break;
          default:
            opTypeList = [{ opType: "like", opName: "包含" }, { opType: "=", opName: "等于" }]
            element["filter"].opTypeValue = "like";
            break;
        }
        if (opTypeList.length > 0) {
          opTypeList.push({ opType: "is null", opName: "为空" })
          opTypeList.push({ opType: "is not null", opName: "不为空" })

          element["filter"].opType = opTypeList;
        }
      }


      element["name"] = key
      this.ItmeArr.push(element);
    }
    console.log(this.ItmeArr)
  }
  ButtonClick(even) {

    even(this.bean, this.ItmeArr)
      .then((x: DtoResultObj<any>) => {
        console.log(x)
        if (x.success) {
          this.windowRef.close();
        }
      })
  }

}
