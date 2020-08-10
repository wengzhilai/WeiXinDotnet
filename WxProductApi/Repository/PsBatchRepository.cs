using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helper;
using IRepository;
using Models;
using Models.Entity;

namespace Repository
{
    /// <summary>
    /// 图片处理服务
    /// </summary>
    public class PsBatchRepository : IPsBatchRepository
    {
        DapperHelper<PsBatchEntity> dbHelper = new DapperHelper<PsBatchEntity>();
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultObj<int>> Delete(int id)
        {
            var reObj = new ResultObj<int>();
            reObj.data = await dbHelper.Delete(x => x.id == id);
            reObj.success = reObj.data > 0;
            return reObj;
        }
        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public async Task<byte[]> MakeCsvByte(int batchId)
        {
            var single = await dbHelper.Single(x => x.id == batchId);
            if (single == null)
            {
                throw new Exception("批次ID有误");
            }
            dbHelper.TranscationBegin();
            try
            {
                var dbHelper_good = new DapperHelper<PsGoodsEntity>(dbHelper.GetConnection(), dbHelper.GetTransaction());
                List<PsGoodsEntity> downList = new List<PsGoodsEntity>();
                var opNum = 0;
                if (single.downNum == 0)
                {
                    for (int i = 0; i < single.goodsNum; i++)
                    {
                        downList.Add(new PsGoodsEntity
                        {
                            id = Guid.NewGuid().ToString("N"),
                            code = Int64.Parse($"{new Random(1).Next(10000000, 99999999)}{ new Random(1).Next(10000000, 99999999)}"),
                            lookNum = 0,
                            openid = "",
                            confirmTime = 0
                        });
                    }
                    opNum = await dbHelper_good.Saves(new DtoSave<List<PsGoodsEntity>> { data = downList });
                    if (opNum == 0)
                    {
                        dbHelper.TranscationRollback();
                        return null;
                    }
                }
                else
                {
                    downList = new List<PsGoodsEntity>(await dbHelper_good.FindAll(x => x.batchId == batchId));
                }
                single.downNum = single.downNum + 1;
                opNum = await dbHelper.Update(new DtoSave<PsBatchEntity> { data = single, saveFieldList = new List<string> { "downNum" }, whereList = new List<string> { "id" } });
                if (opNum == 0)
                {
                    dbHelper.TranscationRollback();
                    return null;
                }


                
                dbHelper.TranscationCommit();
            }
            catch
            {
                dbHelper.TranscationRollback();

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<ResultObj<int>> Save(DtoSave<PsBatchEntity> inEnt)
        {
            var reObj = new ResultObj<int>();
            inEnt.data.createTime = Helper.DataTimeHelper.getDateLong(DateTime.Now);
            inEnt.data.id = await SequenceRepository.GetNextID<PsBatchEntity>();
            reObj.data = await dbHelper.Save(inEnt);
            reObj.success = reObj.data > 0;
            return reObj;
        }
    }
}