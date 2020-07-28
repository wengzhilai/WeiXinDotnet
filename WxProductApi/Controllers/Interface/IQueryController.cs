using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueryController
    {
        /**
        * 根据代码查询Query
        * @param inObj
        * @return
        */
        Task<ResultObj<SysQueryEntity>> getSingleQuery(DtoKey inObj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="querySearchModel"></param>
        /// <returns></returns>
        Task<ResultObj<Dictionary<String, Object>>> getListData(QuerySearchDto querySearchModel);

        /**
         * 查询单个Query
         * @param inEnt
         * @return
         */
        Task<ResultObj<SysQueryEntity>> singleByKey(DtoDo<int> inEnt);

        /**
         * 保存Query
         * @param inEnt
         * @return
         */
        Task<ResultObj<int>> save(DtoSave<SysQueryEntity> inEnt);

        /**
         * 删除Query
         * @param inEnt
         * @return
         */
        Task<ResultObj<int>> delete(DtoDo<int> inEnt);

        /**
         * 下载文件
         * @param postJson
         * @return
         */
        Task<IActionResult> downFile(DtoKey postJson);
    }
}
