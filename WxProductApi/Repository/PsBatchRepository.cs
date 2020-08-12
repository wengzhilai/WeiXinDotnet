using System;
using System.Collections.Generic;
using System.Text;
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
        DapperHelper<PsGoodsEntity> dbHelperGoods = new DapperHelper<PsGoodsEntity>();

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
        /// 检测货物
        /// </summary>
        /// <param name="inLog"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResultObj<PsGoodsEntity>> GoodsCheck(PsGoodsLogEntity inLog, string code)
        {
            var reObj = new ResultObj<PsGoodsEntity>();
            var goods = await dbHelperGoods.Single(x => x.id == inLog.goodsGuid);
            if (goods == null || !goods.code.Equals(code))
            {
                reObj.success = false;
                reObj.msg = "产品有误";
            }
            else
            {
                inLog.id = await SequenceRepository.GetNextID<PsGoodsLogEntity>();
                inLog.createTime = Helper.DataTimeHelper.getDateLong(DateTime.Now);
                dbHelperGoods.TranscationBegin();
                try
                {
                    DapperHelper<PsGoodsLogEntity> dbHelperGoodsLog = new DapperHelper<PsGoodsLogEntity>(dbHelperGoods.GetConnection(), dbHelperGoods.GetTransaction());
                    var opNum = await dbHelperGoodsLog.Save(new DtoSave<PsGoodsLogEntity> { data = inLog });
                    if (opNum < 1) throw new Exception("保存日志失败");
                    goods.confirmTime = inLog.createTime;
                    goods.openid = inLog.openid;
                    opNum = await dbHelperGoods.Update(new DtoSave<PsGoodsEntity> { data = goods, saveFieldListExp = x => new object[] { x.confirmTime, x.openid }, whereListExp = x => new object[] { x.id } });
                    if (opNum < 1) throw new Exception("更新检测失败");
                    dbHelperGoods.TranscationCommit();
                    reObj.success = true;
                    reObj.data = goods;
                }
                catch (Exception e)
                {
                    dbHelperGoods.TranscationRollback();
                    reObj.success = false;
                    reObj.msg = e.Message;
                }
            }
            return reObj;
        }
        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="inLog"></param>
        /// <returns></returns>
        public async Task<ResultObj<PsGoodsEntity>> GoodsDetail(PsGoodsLogEntity inLog)
        {
            var reObj = new ResultObj<PsGoodsEntity>();
            var goods = await dbHelperGoods.Single(x => x.id == inLog.goodsGuid);
            if (goods == null)
            {
                reObj.success = false;
                reObj.msg = "产品有误";
            }
            else
            {
                dbHelperGoods.TranscationBegin();
                try
                {
                    DapperHelper<PsGoodsLogEntity> dbHelperGoodsLog = new DapperHelper<PsGoodsLogEntity>(dbHelperGoods.GetConnection(), dbHelperGoods.GetTransaction());
                    inLog.id = await SequenceRepository.GetNextID<PsGoodsLogEntity>();
                    inLog.createTime = Helper.DataTimeHelper.getDateLong(DateTime.Now);

                    var opNum = await dbHelperGoodsLog.Save(new DtoSave<PsGoodsLogEntity> { data = inLog });
                    if (opNum < 1) throw new Exception("保存日志失败");
                    opNum = await dbHelperGoods.Update(new DtoSave<PsGoodsEntity> { data = new PsGoodsEntity { id = inLog.goodsGuid, lookNum = goods.lookNum + 1 }, saveFieldList = new List<string> { "lookNum" }, whereList = new List<string> { "id" } });
                    if (opNum < 1) throw new Exception("更新数量失败");
                    dbHelperGoods.TranscationCommit();

                    goods.allLogs = new List<PsGoodsLogEntity>(await dbHelperGoodsLog.FindAll(x => x.goodsGuid == inLog.goodsGuid));
                    goods.lookNum = goods.allLogs.Count;

                    reObj.success = true;
                    reObj.data = goods;
                }
                catch (Exception e)
                {
                    dbHelperGoods.TranscationRollback();
                    reObj.success = false;
                    reObj.msg = e.Message;
                }
            }
            return reObj;
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]> MakeCsvByte(int batchId,string url)
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
                    #region 首次下载,生成数据，并添加
                    for (int i = 0; i < single.goodsNum; i++)
                    {
                        downList.Add(new PsGoodsEntity
                        {
                            id = Guid.NewGuid().ToString("N"),
                            code = $"{single.code}{new Random(i).Next(1000000, 9999999)}{ new Random(i+single.goodsNum).Next(1000000, 9999999)}",
                            batchId=batchId,
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
                    #endregion
                }
                else
                {
                    downList = new List<PsGoodsEntity>(await dbHelper_good.FindAll(x => x.batchId == batchId));
                }

                #region 更新下载次数
                single.downNum = single.downNum + 1;
                opNum = await dbHelper.Update(new DtoSave<PsBatchEntity> { data = single, saveFieldList = new List<string> { "downNum" }, whereList = new List<string> { "id" } });
                if (opNum == 0)
                {
                    dbHelper.TranscationRollback();
                    return null;
                }
                #endregion
                dbHelper.TranscationCommit();

                #region 生成csv数据
                List<byte> reEnt = new List<byte>();
                reEnt.AddRange(Encoding.UTF8.GetBytes($"代码,二维码地址\r\n"));

                foreach (var item in downList)
                {
                    reEnt.AddRange(Encoding.UTF8.GetBytes($"{item.code},{url}{item.id}\r\n"));
                }
                return reEnt.ToArray();
                #endregion

            }
            catch (Exception e)
            {
                dbHelper.TranscationRollback();

                throw e;
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
            if (inEnt.data.id == 0)
            {
                inEnt.data.id = await SequenceRepository.GetNextID<PsBatchEntity>();
                reObj.data = await dbHelper.Save(inEnt);
                reObj.success = reObj.data > 0;

            }
            else
            {
                var opNum= await dbHelper.Update(inEnt);
                reObj.success = opNum > 0;
            }
            return reObj;
        }
    }
}