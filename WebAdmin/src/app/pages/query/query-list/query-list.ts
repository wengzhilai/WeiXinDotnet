import { Component, OnInit, ViewChild, ElementRef, ContentChild, ViewContainerRef, Renderer2 } from '@angular/core';
import { HttpHelper } from '../../../Helper/HttpHelper';
import { SmartTableDataSource } from '../../../Helper/SmartTableDataSource';
import { Fun } from '../../../Config/Fun';
import { DtoResult, DtoResultObj } from '../../../Model/DtoRec/DtoResult';
import { NbWindowService } from '@nebular/theme';
import { DtoSaveObj } from '../../../Model/DtoPost/DtoSaveObj';
import { DtoDo } from '../../../Model/DtoPost/DtoDo';
import { QueryEditComponent } from '../../../components/query-edit/query-edit.component';
import { ServerSourceConf } from '../../../Lib/ng2-smart-table/lib/lib/data-source/server/server-source.conf';

@Component({
  selector: 'query-list',
  templateUrl: './query-list.html',
  styleUrls: ['./query-list.scss']
})
export class QueryListPage implements OnInit {
  @ViewChild('samrtTable', { static:true,read: ViewContainerRef }) container: ViewContainerRef;
  @ViewChild('btnHead',{static:true}) template;
  source: SmartTableDataSource;
  /** 静态方法，获取默认配置 */
  settings: any = SmartTableDataSource.getDefaultSetting();
  configJson: any = {}
  /** 显示的列数 */
  clmNum: number = 0
  constructor(
    private HttpHelper: HttpHelper,
    private windowService: NbWindowService,
    private renderer: Renderer2
  ) {
    let smartTableCofnig: ServerSourceConf = new ServerSourceConf();
    smartTableCofnig.endPoint = 'user/query/getListData';
        smartTableCofnig.dataKey = "code"
        smartTableCofnig.pagerLimitKey = ""
    this.source = new SmartTableDataSource(this.HttpHelper, smartTableCofnig,"query");
    this.configJson = {
      "id": {
        "title": '查询ID',
        "type": 'number',
        "filter": false,
        "editable": false,
        "name":"id",
      },
      "name": {
        "title": '查询名',
        "filter": true,
        "type": 'string',
        "name":"name",
      },
      "code": {
        "title": '代码',
        "type": 'string',
        "name":"code",
      },
      "auto_load": {
        "title": '自动加载',
        "defaultValue": 1,
        "type": 'string',
        "valuePrepareFunction": (v) => { return v == 1 ? '是' : '否' },
        "name":"autoLoad",
        "editor": {
          "type": 'list',
          "config": {
            "list": [
              { "value": true, "title": '是' },
              { "value": false, "title": '否' }
            ]
          }
        }
      },
      "page_size": {
        "title": '页面大小',
        "type": 'number',
        "defaultValue": 10,
        "name":"pageSize",
      },
      "show_checkbox": {
        "title": '允许多选',
        "type": 'string',
        "defaultValue": 1,
        "name":"showCheckbox",
        "valuePrepareFunction": (v) => { return v == 1 ? '是' : '否' },
        "editor": {
          "type": 'list',
          "config": {
            "list": [
              { "value": true, "title": '是' },
              { "value": false, "title": '否' }
            ]
          }
        }
      },
      "is_debug": {
        "title": '是否隐藏',
        "type": 'string',
        "defaultValue": 1,
        "name":"isDebug",
        "valuePrepareFunction": (v) => { return v == 1 ? '是' : '否' },
        "editor": {
          "type": 'list',
          "config": {
            "list": [
              { "value": true, "title": '是' },
              { "value": false, "title": '否' }
            ]
          }
        }
      },
      "filtr_level": {
        "title": '过滤层级',
        "type": 'number',
        "defaultValue": 1,
        "name":"filterLevel",
      },
      "query_conf": {
        "title": '查询脚本',
        "type": 'string',
        "inputWidth": 12,
        "isTabs": true,
        "hide": true,
        "name":"queryConf",
        "editor": {
          "type": 'textarea'
        }
      },
      "query_cfg_json": {
        "title": '列配置信息',
        "type": 'string',
        "isTabs": true,
        "hide": true,
        "inputWidth": 12,
        "name":"queryCfgJson",
        "editor": {
          "type": 'textarea'
        }
      },

      "in_para_json": {
        "title": '传入的参数',
        "type": 'string',
        "isTabs": true,
        "hide": true,
        "inputWidth": 12,
        "name":"inParaJson",
        "editor": {
          "type": 'textarea'
        }
      },
      "js_str": {
        "title": 'JS脚本',
        "type": 'string',
        "isTabs": true,
        "hide": true,
        "inputWidth": 12,
        "name":"jsStr",
        "editor": {
          "type": 'textarea'
        }
      },
      "rows_btn": {
        "title": '行按钮',
        "isTabs": true,
        "hide": true,
        "name":"rowsBtn",
        "type": 'string'
      },
      "heard_btn": {
        "title": '表头按钮',
        "isTabs": true,
        "hide": true,
        "name":"heardBtn",
        "type": 'string'
      },

      "remark": {
        "title": '备注',
        "isTabs": true,
        "hide": true,
        "type": 'string',
        "inputWidth": 12,
        "name":"remark",
        "editor": {
          "type": 'textarea'
        }
      }
    }
    for (const key in this.configJson) {
      this.clmNum++;
    }
    this.clmNum += 2;

    //隐藏，hide=true的字段
    this.settings.columns = this.configJson;
    this.settings.actions["add"] = true;
    this.settings.pager["perPage"] = 10;
    this.source.setting=this.settings;

  }

  ngOnInit() {
    setTimeout(() => {
      var table = this.container.element.nativeElement.children[0];
      var tableHeald = table.children[0]
      var tableHealdFirstRow = table.children[0].children[0]
      // console.log(this.container)
      // console.log(tableHeald)
      // console.log(tableHealdFirstRow)
      // console.log(this.template.nativeElement)
      // const text = this.renderer.createText('Hello world!');
      this.renderer.appendChild(table, this.template.nativeElement)
      // this.renderer.insertBefore(tableHeald,this.template.nativeElement,tableHealdFirstRow)
    }, 1000);
  }



  /**
   * 
   * @param event 添加事件
   */
  async onSave(event): Promise<void> {
    if (event == null || event.data==null) {
      this.OpenEditWindow("添加模块", {})
      return;
    }

    console.log(event.data)
    //先根据ID找到对象
    await Fun.ShowLoading();
    this.HttpHelper.Post("user/query/singleByKey", { key: event.data.id }).then(async (x: DtoResultObj<any>) => {
      await Fun.HideLoading()
      if (x.success) {
        let title = "修改模块";
        if (event.data != null) {
          title = "添加模块"
        }
        this.OpenEditWindow(title, x.data)
      }
    })



  }

  /**
   * 
   * @param title 标题
   * @param data 修改数据
   */
  OpenEditWindow(title: string, data: any) {
    this.windowService.open(QueryEditComponent, {
      windowClass: "DivWindow",
      title: title,
      context: {
        bean: data,
        inputs: this.configJson,
        buttons: [{
          name: "确定", click: (x) => {
            return new Promise(async (resolve, reject) => {
              console.log(x);
              if (window.confirm('确定要保存吗？')) {
                let postClass: DtoSaveObj<any> = new DtoSaveObj<any>();
                postClass.data = x;
                postClass.saveFieldList = Fun.GetBeanNameStr(x,null);
                await Fun.ShowLoading();

                this.HttpHelper.Post("user/query/save", postClass).then((data: DtoResultObj<any>) => {
                  Fun.HideLoading();
                  console.log(data)
                  if (data.success) {
                    this.source.refresh()
                  }
                  else {
                    Fun.Hint(data.msg)
                  }
                  resolve(data);
                });
              } else {
              }
            });
          }
        }]
      }
    });
  }


  /**
   * 
   * @param event 添加事件
   */
  onDelete(event): void {
    console.log(event.data)
    if (window.confirm('确定要删除吗?')) {
      Fun.ShowLoading();
      let postClass: DtoDo = new DtoDo();
      postClass.key = event.data.id;
      this.HttpHelper.Post("user/query/delete", postClass).then((data: DtoResult) => {
        Fun.HideLoading()
        if (data.success) {
          this.source.refresh()
        }
      });
    }
  }

}
