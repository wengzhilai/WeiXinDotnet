import { Component, OnInit } from '@angular/core';
import { NbWindowRef } from '@nebular/theme';
import { DtoResultObj } from '../../Model/DtoRec/DtoResult';
import { LocalDataSource } from '../../Lib/ng2-smart-table/public-api';

@Component({
  selector: 'ngx-table-edit',
  templateUrl: './table-edit.component.html',
  styleUrls: ['./table-edit.component.scss']
})
export class TableEditComponent implements OnInit {

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
    public windowRef: NbWindowRef
  ) {
    console.log(this.windowRef.config.context)
  }

  /**
   * 用于绑定table的设置
   */
  settings: any;
  source: LocalDataSource;

  configJson = {
    "id": {
      "title": 'ID',
      "type": 'number',
    },
    "name": {
      "title": '别名',
      "type": 'string',
    },
    "columnName": {
      "title": '列名',
      "type": 'string',
    },
    "stauts": {
      "title": '状态',
      "type": 'string',
      "editor": {
        "type": 'list',
        "config": {
          "list": [
            { "value": "启用", "title": '启用' },
            { "value": "禁用", "title": '禁用' }
          ]
        }
      }
    },
    "orderIndex": {
      "title": '排序号',
      "type": 'number',
    },
    "columnType": {
      "title": '列类型',//text,int,datatime,pic,textarea,Checkbox,Radio,auto
      "type": 'string',
      "editor": {
        "type": 'list',
        "config": {
          "list": [
            { "value": "Text", "title": '文本' },
            { "value": "Int", "title": '数字' },
            { "value": "Datatime", "title": '日期' },
            { "value": "Pic", "title": '图片' },
            { "value": "Textarea", "title": '多行文本' },
            { "value": "Checkbox", "title": '复选框' },
            { "value": "Radio", "title": '单选框' },
            { "value": "Auto", "title": '自动' }
          ]
        }
      }
    },
    "columnLong": {
      "title": '字段长度',
      "type": 'number',
    },
    "isRequired": {
      "title": '必填',
      "type": 'checkbox',
    },
    "defaultValue": {
      "title": '默认值',
      "type": 'string',
    },
    "columnTypeCfg": {
      "title": '列配置内容',
      "type": 'string',
    },
    "authority": {
      "title": '权限',
      "type": 'number',
    },
    "introduce": {
      "title": '介绍',
      "type": 'string',
    },
  }
  ngOnInit() {
    this.settings.columns = this.configJson;
    this.settings.mode="inline";
    if(this.bean["allColumns"]==null)this.bean["allColumns"]=[];
    this.source = new LocalDataSource(this.bean["allColumns"]);
    for (const key in this.inputs) {
      let element = this.inputs[key];
      element["name"] = key
      this.ItmeArr.push(element);
    }
    this.source.onChanged().subscribe(x => {
      this.bean["allColumns"] = x.elements;
      console.log(this.bean)
    })

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

  showAdd(even){
    this.source.prepend({})
  }


}
