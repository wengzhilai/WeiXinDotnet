using System;
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
    public class PsAdminRepository : IPsAdminRepository
    {
        DapperHelper<PsAdminEntity> dbHelper = new DapperHelper<PsAdminEntity>();
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
        /// 保存
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<ResultObj<int>> Save(DtoSave<PsAdminEntity> inEnt)
        {
            var reObj = new ResultObj<int>();
            inEnt.data.createTime = Helper.DataTimeHelper.getDateLong(DateTime.Now);
            inEnt.data.id=await SequenceRepository.GetNextID<PsAdminEntity>();
            reObj.data = await dbHelper.Save(inEnt);
            reObj.success = reObj.data > 0;
            return reObj;
        }
    }
}