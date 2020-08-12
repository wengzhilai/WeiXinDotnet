using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Entity;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("user/[controller]/[action]")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class QueryController : ControllerBase ,IQueryController
    {
        private IQueryRepository _query;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        public QueryController(
            IQueryRepository query
            )
        {
            this._query = query;
        }
        

        /// <summary>
        /// 获取数据对象
        /// </summary>
        /// <param name="querySearchModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<Dictionary<string, object>>> getListData(QuerySearchDto querySearchModel)
        {

            ResultObj<Dictionary<string, object>> reObj = new ResultObj<Dictionary<string, object>>();
            try
            {
                if (querySearchModel.whereList == null && !string.IsNullOrWhiteSpace(querySearchModel.whereListStr))
                {
                    querySearchModel.whereList = TypeChange.ToJsonObject<List<QueryRowBtnShowCondition>>(querySearchModel.whereListStr);
                }

                if (querySearchModel.paraList == null && !string.IsNullOrWhiteSpace(querySearchModel.paraListStr))
                {
                    querySearchModel.paraList = TypeChange.ToJsonObject<List<QueryPara>>(querySearchModel.paraListStr);
                }

                //添加AdministratorModel对象加入到参数列表中,以方便在Query里进行过滤
                if (querySearchModel.paraList == null) querySearchModel.paraList = new List<QueryPara>();


                List<QueryPara> QueryPara = new List<QueryPara>();

                if (querySearchModel.paraList == null) querySearchModel.paraList = new List<QueryPara>();
                querySearchModel.paraList.AddRange(QueryPara);

                reObj = await _query.QueryPageExecute(querySearchModel);
                // reObj.Msg="";
                // Session[string.Format("SQL_{0}", querySearchModel.Code)] = sqlStr;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.success = false;
                reObj.msg = ex.Message;
            }
            return reObj;

        }

        /// <summary>
        /// 获取Query对象
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<SysQueryEntity>> getSingleQuery(DtoKey inEnt)
        {
            ResultObj<SysQueryEntity> reObj = new ResultObj<SysQueryEntity>();
            try
            {
                reObj.data = await _query.Single(i => i.code == inEnt.Key);
                reObj.success = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }
        
        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="querySearchModel"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> downFileGet(string code,int page,int rows,string order,string sort)
        {
            QuerySearchDto postJson=new QuerySearchDto();
            postJson.code=code;
            postJson.sort=sort;
            postJson.order=order;
            postJson.page = 1;
            postJson.rows = 1000000000;
            // var reData =await _query.QueryExecuteCsv(querySearchModel);
            // Session[string.Format("SQL_{0}", querySearchModel.Code)] = sqlStr;
            var tmepObj = await _query.QueryExecuteCsv(postJson);
            if(tmepObj.success){
                return File(tmepObj.data.ToArray(), "application/octet-stream", string.Format("{0}.csv", postJson.code));
            }
            return Ok(tmepObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> downFileByCode(DtoKey code)
        {
            var tmepObj = await _query.QueryExecuteCsv(new QuerySearchDto{
                code=code.Key,
                page=1,
                rows=10000
            });
            return File(tmepObj.data.ToArray(), "application/octet-stream", string.Format("{0}.csv", code));
        }

        /// <summary>
        /// 生成配置数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<String>> makeQueryCfg(string code)
        {
            ResultObj<String> reObj = new ResultObj<String>();
            try
            {
                reObj = await _query.MakeQueryCfg(code);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }

        

        /// <summary>
        /// 保存Query
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<int>> save(DtoSave<SysQueryEntity> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                if(string.IsNullOrEmpty(inEnt.data.queryCfgJson)){
                    inEnt.data.queryCfgJson=TypeChange.ObjectToStr((await _query.MakeQueryCfg(inEnt.data.code)).data);
                }
                if(string.IsNullOrEmpty(inEnt.data.rowsBtn)){
                   inEnt.data.rowsBtn=@"
[{
		""title"": ""修改"",
		""class"": ""nb-edit"",
		""readUrl"": ""user/*/singleByKey"",
		""apiUrl"": ""user/*/save""
	},
	{
		""title"": ""删除"",
		""class"": ""nb-trash"",
		""apiUrl"": ""user/*/delete"",
		""confirmTip"": ""确定要删除吗？""
	}
]"; 
                }
                if(string.IsNullOrEmpty(inEnt.data.heardBtn)){
                    inEnt.data.heardBtn=@"
[{
		""title"": ""添加"",
		""class"": ""nb-plus"",
		""click"": ""nowThis.Add('user/*/save')""
	},
	{
		""title"": ""批量删除"",
		""class"": ""ion-delete"",
		""click"": ""nowThis.Exec('user/*/delete','id','删除要删除吗？')""
	}
]                 
                    ";
                }
                if (inEnt.data.id == 0)
                {
                    reObj.data = await _query.Save(inEnt);
                }
                else
                {
                    reObj.data = await _query.Update(inEnt);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }

        /// <summary>
        /// 查找单条
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<SysQueryEntity>> singleByKey(DtoDo<int> inEnt)
        {
            ResultObj<SysQueryEntity> reObj = new ResultObj<SysQueryEntity>();
            try
            {
                reObj.data = await _query.Single(x=>x.id==inEnt.Key);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }

        /// <summary>
        /// 删除单条
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<int>> delete(DtoDo<int> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                reObj.data = await _query.Delete(x=>x.id==inEnt.Key);

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }

 
    }
}