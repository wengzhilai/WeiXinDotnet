
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPsBatchRepository
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<int>> Save(DtoSave<PsBatchEntity> inEnt);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键 ID</param>
        /// <returns></returns>
        Task<ResultObj<int>> Delete(int id);

        /// <summary>
        /// 生成批次csv文件
        /// </summary>
        /// <param name="batchId">批次ID</param>
        /// <param name="url">url前缀</param>
        /// <returns></returns>
        Task<byte[]> MakeCsvByte(int batchId,string url);

        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="inLog"></param>
        /// <returns></returns>
        Task<ResultObj<PsGoodsEntity>> GoodsDetail(PsGoodsLogEntity inLog);
        /// <summary>
        /// 检测产品码
        /// </summary>
        /// <param name="inLog"></param>
        /// <returns></returns>
        Task<ResultObj<PsGoodsEntity>> GoodsCheck(PsGoodsLogEntity inLog,string code);

    }
}
