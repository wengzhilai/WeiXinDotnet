import { HttpClient, HttpParams } from '@angular/common/http';
// import { getDeepFromObject } from "@Nebular/auth/helpers";
import { Observable } from "rxjs";
import { HttpHelper } from "./HttpHelper";

import { map } from 'rxjs/operators';
import { DtoResultObj } from "../Model/DtoRec/DtoResult";
import { LocalDataSource } from '../Lib/ng2-smart-table/public-api';
import { ServerSourceConf } from '../Lib/ng2-smart-table/lib/lib/data-source/server/server-source.conf';

export class SmartTableDataSource extends LocalDataSource {
  protected conf: ServerSourceConf;
  protected lastRequestCount: number = 0;
  public setting:any={};
  // public whereList=[{ value: "值", opType: "类型",fieldType: "字段类型",fieldName:"字段名"}];
  public whereList=[];
  constructor(
    protected httpHelper: HttpHelper,
    conf: ServerSourceConf | {} = {},
    private inKey: string = null
  ) {
    super();
    this.conf = new ServerSourceConf(conf);
    if (!this.conf.endPoint) {
      throw new Error('没有设置接口地址');
    }
    else {
      console.log("接口地址：" + this.conf.endPoint)
    }
  }

  count(): number {
    return this.lastRequestCount;
  }

  async getElements(): Promise<any> {
    return this.requestElements()
      .pipe(map(res => {
        //用于Query查询
        if(res.dataList!=null){
          res.msg=res.total;
          res.dataList=res.dataList;
        }


        this.lastRequestCount = this.extractTotalFromResponse(res);
        this.data = res.dataList;
        console.log(this.lastRequestCount);
        console.log(this.data);
        return this.data;
      })).toPromise();
  }

  /**
   * Extracts array of data from server response
   * @param res
   * @returns {any}
   */
  protected extractDataFromResponse(res: any): Array<any> {
    console.log("从服务器响应中提取数据数组");
    const rawData = res;
    // const data = !!this.conf.dataKey ? getDeepFromObject(rawData, this.conf.dataKey, []) : rawData;
    const data=res
    if (data.data instanceof Array) {
      return data.data;
    }
    else {
      return [];
    }
  }

  /**
   * Extracts total rows count from the server response
   * Looks for the count in the heders first, then in the response body
   * @param res
   * @returns {any}
   */
  protected extractTotalFromResponse(res: DtoResultObj<any>): number {
    console.log("从第一条提出总数");
    let total: number = 0
    if (res.msg == null || res.msg == "") {
      total = res.dataList.length;
    } else {
      total = parseInt(res.msg);
    }
    return total
  }

  /**
   * 获取提交参数
   */
  public getPostEnt(){
    let allPar = this.createRequesParams();
    let par = allPar
    console.log(par)
    let postBean: any = {};
    postBean.sort=par.get("_sort");
    postBean.order=par.get("_order");
    postBean.rows=par.get("_limit");
    postBean.page=par.get("_page");
    par.keys().forEach((key,num) => {
      console.log(key)
      console.log(num)
      if (key.indexOf('_like') > 0) {
        let keyName = key.substr(0, key.indexOf('_like'));
        if(postBean.whereList==null){
          postBean.whereList=[]
        }
        var column=this.setting.columns[keyName];
        postBean.whereList.push({ value: par.get(key), opType: "like",fieldType: column["type"],fieldName:keyName}); //排序字段
      }
    })
    this.whereList.forEach(element => {
      if(postBean.whereList==null){
        postBean.whereList=[]
      }
      postBean.whereList.push(element);
    });
    postBean.code = this.inKey
    if(postBean.rows==null)postBean.rows=1;
    if(postBean.page==null)postBean.page=10;
    return postBean;
  }

  protected requestElements(): Observable<any> {
    console.log("请求所有数据");
    let postBean: any = this.getPostEnt();
    return this.httpHelper.PostToObservable(this.conf.endPoint, postBean).pipe(x => {
      return x
    })
  }

  protected createRequesParams(): HttpParams {
    console.log("获取头文件");
    let httpParams = new HttpParams();

    httpParams = this.addSortRequestParams(httpParams);
    httpParams = this.addFilterRequestParams(httpParams);
    return this.addPagerRequestParams(httpParams);
  }

protected addSortRequestParams(httpParams: HttpParams): HttpParams {
  console.log("添加排序请求参数");
  if (this.sortConf) {
      this.sortConf.forEach((fieldConf) => {
        httpParams = httpParams.set(this.conf.sortFieldKey, fieldConf.field);
        httpParams = httpParams.set(this.conf.sortDirKey, fieldConf.direction.toUpperCase());
      });
    }

    return httpParams;
  }

  protected addFilterRequestParams(httpParams: HttpParams): HttpParams {
    console.log("添加过虑请求参数");
    if (this.filterConf.filters) {
      this.filterConf.filters.forEach((fieldConf: any) => {
        if (fieldConf['search']) {
          httpParams = httpParams.set(this.conf.filterFieldKey.replace('#field#', fieldConf['field']), fieldConf['search']);
        }
      });
    }

    return httpParams;
  }

  protected addPagerRequestParams(httpParams: HttpParams): HttpParams {
    console.log("添加其它请求参数");

    if (this.pagingConf && this.pagingConf['page'] && this.pagingConf['perPage']) {
      httpParams = httpParams.set(this.conf.pagerPageKey, this.pagingConf['page']);
      httpParams = httpParams.set(this.conf.pagerLimitKey, this.pagingConf['perPage']);
    }

    return httpParams;
  }




    /**
   * 获取默认的表参数
   */
  public static getDefaultSetting() {
    return {
      noDataMessage: "无数据",
      mode: "external",
      // hideHeader: true,
      hideSubHeader: true, //隐藏筛选行
      add: {
        addButtonContent: '<i class="nb-plus"></i>',
        createButtonContent: '<i class="nb-checkmark"></i>',
        cancelButtonContent: '<i class="nb-close"></i>',
      },
      edit: {
        editButtonContent: '<i class="iconfont icon-edit"></i>',
        saveButtonContent: '<i class="nb-checkmark"></i>',
        cancelButtonContent: '<i class="nb-close"></i>',
      },
      delete: {
        deleteButtonContent: '<i class="iconfont icon-ashbin"></i>',
      },
      actions: {
        columnTitle: "操作",
        add:false,
        position:'left'
        // position:"right"
      },
      columns: {},
      pager: {
        display: true,
        perPage: 10
      }
    }
  }
  /**
   * 删除有Hide为true的项
   * @param objJson 
   */
  public static ReMoveHideItem(objJson) {
    console.log("删除有Hide为true的项")
    let reJson = {}
    for (var item in objJson) {
      if (objJson[item]["hide"] == null || objJson[item]["hide"] != true) {
        reJson[item] = objJson[item]
      }
    }
    return reJson
  }
}


export class SmartTableSetting{

  mode:string= 'inline';
  selectMode:string= 'single'; // single|multi
  hideHeader:boolean= false;
  hideSubHeader: boolean=false;
  actions:any= {
    columnTitle: 'Actions', 
    add: true,
    edit: true,
    delete: true,
    custom: [],
    position: 'left', // left|right
  };
  filter:any= {
    inputClass: '',
  };
  edit:any={
    inputClass: '',
    editButtonContent: 'Edit',
    saveButtonContent: 'Update',
    cancelButtonContent: 'Cancel',
    confirmSave: false,
  };
  add: {
    inputClass: '',
    addButtonContent: 'Add New',
    createButtonContent: 'Create',
    cancelButtonContent: 'Cancel',
    confirmCreate: false,
  };
  delete: {
    deleteButtonContent: 'Delete',
    confirmDelete: false,
  };
  attr: {
    id: '',
    class: '',
  };
  noDataMessage: 'No data found';
  columns: {}
  pager: {
    display: true,
    perPage: 10,
  };
  rowClassFunction: () => ""

  
}

