import { Component, OnInit, ViewChild, ViewContainerRef, Renderer2 } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SmartTableDataSource } from '../../../Helper/SmartTableDataSource';
import { HttpHelper } from '../../../Helper/HttpHelper';
import { NbWindowService } from '@nebular/theme';
import { DtoResultObj } from '../../../Model/DtoRec/DtoResult';
import { Variables } from '../../../Config/Variables';
import { Fun } from '../../../Config/Fun';
import { DtoDo } from '../../../Model/DtoPost/DtoDo';
import { DtoSaveObj } from '../../../Model/DtoPost/DtoSaveObj';
import { EditModelComponent } from '../../../components/edit-model/edit-model.component';
// import { RoleEditComponent } from '../../../components/role-edit/role-edit.component';
import { QueryEditComponent } from '../../../components/query-edit/query-edit.component';
import { TableEditComponent } from '../../../components/table-edit/table-edit.component'
import { QueryFilterComponent } from '../../../components/query-filter/query-filter.component';
import { ServerSourceConf } from '../../../Lib/ng2-smart-table/lib/lib/data-source/server/server-source.conf';

@Component({
  selector: 'query',
  templateUrl: './query.html',
  styleUrls: ['./query.scss']
})
export class QueryQueryComponent implements OnInit {
  @ViewChild('samrtTable', { static: true, read: ViewContainerRef }) container: ViewContainerRef;
  @ViewChild('btnHead', { static: true }) template;
  source: SmartTableDataSource;
  queryEnt: any = {
    REMARK: "　"
  };
  /**
   * 表头按钮
   */
  headBtnSet: Array<any> = [];
  /**
   * 行按钮
   */
  rowBtnSet: Array<any> = [];
  //用于加载设置，如果为 false，则不显示Table,
  LoadSetting: boolean = false;
  /**
   * 用于绑定table的设置
   */
  settings: any = SmartTableDataSource.getDefaultSetting();;
  /**
   * 读取配置文件的设置
   */
  configJson: any = {}
  /** 显示的列数 */
  clmNum: number = 0

  selectedArr = []
  code: any;
  thisUrl: string = ""
  constructor(
    private routerIonfo: ActivatedRoute,
    private HttpHelper: HttpHelper,
    private windowService: NbWindowService,
    private renderer: Renderer2
  ) {
  }
  ngOnInit() {


    console.log(this.code)

    this.routerIonfo.queryParams.subscribe(params => {
      this.code = params['code'];
      console.log(params)
      this.LoadData();
    });
  }


  LoadData() {
    //隐藏table，在显示的时候，才会刷新列数据
    this.LoadSetting = false;
    let postEnt = { key: this.code }
    return this.HttpHelper.Post("user/query/GetSingleQuery", postEnt).then((data: DtoResultObj<any>) => {
      if (data.success) {
        //显示table
        this.LoadSetting = true;

        this.queryEnt = data.data
        //隐藏，hide=true的字段
        let t: any = {}
        //设置列配置
        eval("t=" + this.queryEnt.queryCfgJson)
        this.configJson = t
        for (const key in this.configJson) {
          this.clmNum++;
        }
        this.clmNum += 2;

        //设置表头按钮配置
        eval("t=" + this.queryEnt.heardBtn)
        this.headBtnSet = t
        //如果没有配置筛选，则添加
        this.headBtnSet.push({
          "title": "导出",
          "class": "ion-archive",
          "click": "nowThis.onExportXls()"
        })
        this.headBtnSet.push({
          "title": "筛选",
          "class": "ion-funnel",
          "click": "nowThis.OpenFilter()"
        })
        //读取行按钮
        try {
          eval("t=" + this.queryEnt.rowsBtn)
          this.rowBtnSet = t
        } catch (error) {

        }
        if (this.rowBtnSet == null) this.rowBtnSet = []

        let tempCol = SmartTableDataSource.ReMoveHideItem(this.configJson);
        // for (const item in tempCol) {
        //   if (tempCol[item]["renderComponent"] == "SmartTableFormatValuePage") {
        //     tempCol[item]["renderComponent"] = SmartTableFormatValuePage
        //   }
        // }
        this.settings.columns = tempCol
        this.LoadSetting = true
        //配置是否有筛选框
        if (this.queryEnt.showCheckbox != 1) {
          this.settings.selectMode = "single"
        }

        this.settings.actions.edit = false;
        this.settings.actions.delete = false;
        if (this.rowBtnSet.length > 0) {
          if (this.rowBtnSet[0].class != null) {
            this.settings.actions.edit = true
            this.settings.edit.editButtonContent = '<i class="' + this.rowBtnSet[0].class + '"></i>'
          }
          if (this.rowBtnSet.length > 1) {
            if (this.rowBtnSet[1].class != null) {
              this.settings.actions.delete = true
              this.settings.delete.deleteButtonContent = '<i class="' + this.rowBtnSet[1].class + '"></i>'
            }
          }
        }


        let smartTableCofnig: ServerSourceConf = new ServerSourceConf();
        smartTableCofnig.endPoint = 'user/query/getListData';
        smartTableCofnig.dataKey = "code"


        this.source = new SmartTableDataSource(this.HttpHelper, smartTableCofnig, this.code);
        this.source.setting = this.settings;

        this.AddHeadBtn()

      }

    }, (x) => {
      console.log(x)
    })
  }

  AddHeadBtn() {
    // setTimeout(() => {
    //   var table = this.container.element.nativeElement.children[0];
    //   this.renderer.appendChild(table, this.template.nativeElement)
    // }, 100);
  }

  userRowSelect(event) {
    this.selectedArr = event.selected
    console.log(this.selectedArr)
  }

  /**
   * 表头按钮事件
   * @param event 
   */
  HeadBtnClick(nowThis, event) {
    if (event != null) {
      eval(event)
    }
  }
  // Add(){
  //   this.OpenEditWindow("添加模块", {})
  // }
  Add(apiUrl, openModal: any = null, defaultData = null, readUrl = null) {
    console.log(apiUrl)
    console.log(defaultData)
    console.log(readUrl)
    // return;
    Fun.ShowLoading();
    this.GetBean(defaultData, readUrl).then((x: DtoResultObj<any>) => {
      Fun.HideLoading();
      console.log(x);
      if (x == null || !x.success) {
        console.log("获取取初始值失败")
        //如果获取取初始值失败，则用列表数据
        x.data = defaultData;
      }
      console.log("获取取初始值")
      console.log(x.data)

      let title = "修改"
      if (defaultData != null) {
        title = "添加"
      }
      let thisOpenMode = this.GetComponents(openModal);
      // console.log(thisOpenMode);
      console.log(x.data);
      // console.log(this.configJson);
      this.windowService.open(thisOpenMode, {
        closeOnBackdropClick:false,
        closeOnEsc:false,
        windowClass: "DivWindow",
        title: title,
        context: {
          bean: x.data,
          inputs: this.configJson,
          buttons: [{
            name: "确定", click: (x) => {
              return new Promise(async (resolve, reject) => {
                console.log(x);
                console.log(this.configJson);
                console.log(Fun.GetBeanNameStr(x, this.configJson));
                //更新时间选择器
                for (const key in x) {
                  if (x.hasOwnProperty(key)) {
                    if (typeof (x[key]) == "object" && x[key] instanceof Date) {
                      if (x[key] != null) {
                        x[key] = Fun.DateFormat(x[key], "yyyy-MM-dd");
                      }
                    }
                  }
                }
                console.log(x);


                if (window.confirm('确定要保存吗？')) {
                  let postClass: DtoSaveObj<any> = new DtoSaveObj<any>();
                  postClass.data = x;
                  postClass.saveFieldList = Fun.GetBeanNameStr(x, this.configJson);
                  await Fun.ShowLoading();


                  this.HttpHelper.Post(apiUrl, postClass).then((data: DtoResultObj<any>) => {
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

    })

  }

  /**导出Excel */
  async onExportXls() {

    var object = this.source.getPostEnt();
    var lists = new Array<string>();
    for (const key in object) {
      if (Object.prototype.hasOwnProperty.call(object, key)) {
        const element = object[key];
        lists.push(key + "=" + element);
      }
    }
    var getStr = lists.join("&");

    var url = Variables.Api + "user/query/downFileGet?" + getStr;

    this.downFile(url);

  }

  downFile(url) {
    var link = document.createElement("a");
    link.setAttribute("href", url);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }


  filterBean: any = {};
  // whereList=[{ value: "值", opType: "类型",fieldType: "字段类型",fieldName:"字段名"}];
  whereList = [];
  /**
   * 打开筛选功能
   */
  async OpenFilter() {

    this.windowService.open(QueryFilterComponent, {
      windowClass: "DivWindow",
      title: "筛选",
      context: {
        bean: this.filterBean,
        inputs: this.configJson,
        buttons: [
          {
            name: "重置", click: (x) => {
              return new Promise(async (resolve, reject) => {
                for (const key in x) {
                  x[key] = "";
                }

              });
            }
          },
          {
            name: "确定", click: (x, cfg) => {
              return new Promise(async (resolve, reject) => {
                console.log(x);
                console.log(cfg);
                console.log(this.configJson);
                let whereObj = {};
                //更新时间选择器
                this.whereList = [];
                for (const key in x) {
                  if (x[key] == null || x[key] == "") {
                    continue;
                  }
                  if (this.configJson[key].filter == null) {
                    whereObj[key] = x[key];
                    continue;
                  }
                  switch (this.configJson[key].filter.type) {
                    case "Date":
                      whereObj[key] = Fun.DateFormat(x[key][0], "yyyy-MM-dd") + "~" + Fun.DateFormat(x[key][1], "yyyy-MM-dd");
                      break;
                    default:
                      whereObj[key] = x[key];
                      break;
                  }
                  this.whereList.push({ value: whereObj[key], opType: this.configJson[key].filter.opTypeValue, fieldType: this.configJson[key].type, fieldName: key });

                }
                this.filterBean = x;
                console.log(this.filterBean);
                console.log(whereObj);
                console.log(this.whereList);
                resolve({ "success": true });
                this.ReLoad();
              });
            }
          },
        ]
      }
    });
  }

  ReLoad() {
    this.source.whereList = this.whereList
    this.source.refresh()
  }
  /**
   * 删除事件
   * @param event 添加事件
   */
  onDelete(event): void {

    if (this.rowBtnSet.length > 1) {
      this.DeleteApi(this.rowBtnSet[1].apiUrl, event.data.id, this.rowBtnSet[1].confirmTip, this.rowBtnSet[1].urlType)
    }

  }


  onSave(nowThis, event) {
    if (this.rowBtnSet.length > 0) {
      this.Add(this.rowBtnSet[0].apiUrl, this.rowBtnSet[0].openModal, event.data, this.rowBtnSet[0].readUrl)
    }
  }
  /**
   * 删除
   * @param apiUrl 
   * @param Key 
   * @param confirmTip 
   */
  async DeleteApi(apiUrl, Key, confirmTip, type) {
    if (window.confirm(confirmTip)) {
      await Fun.ShowLoading();
      switch (type) {
        case "down":
          var url = Variables.Api + apiUrl + "?key=" + Key;
          this.downFile(url)
          break;
        default:
          let postClass: DtoDo = new DtoDo();
          postClass.key = Key;
          this.HttpHelper.Post(apiUrl, postClass).then((data: DtoResultObj<any>) => {
            Fun.HideLoading()
            console.log(data)
            if (data.success) {
              this.source.refresh()
            }
            else {
              Fun.Hint(data.msg)
            }
          });
          break;
      }

    }
  }

  Exec(apiUrl, Key, confirmTip) {
    let allKeyList = []
    this.selectedArr.forEach(element => {
      allKeyList.push(element[Key])
    });
    this.DeleteApi(apiUrl, allKeyList.join(","), confirmTip, null)
  }


  /**
   * 获取初始值
   * @param defaultData 行选择的值
   * @param readUrl 加载的URL
   */
  GetBean(defaultData = null, readUrl = null): Promise<any> {
    if (readUrl != null && defaultData != null && defaultData.id != null) {
      return this.HttpHelper.Post(readUrl, { key: defaultData.id })
    }
    else {
      if (defaultData == null) defaultData = {}
      return new Promise((resolve, rejeact) => { resolve({ "success": true, "data": defaultData }) });
    }
  }




  GetComponents(name) {
    switch (name) {
      // case "RoleEditComponent":
      //   return RoleEditComponent
      case "QueryEditComponent":
        return QueryEditComponent;
      case "TableEditComponent":
        return TableEditComponent;
      default:
        return EditModelComponent
    }
  }
}