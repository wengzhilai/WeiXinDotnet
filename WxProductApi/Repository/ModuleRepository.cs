
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helper;
using IRepository;
using Models;
using System.Linq;
using Models.Entity;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using System.Linq.Expressions;

namespace Repository
{
    public class ModuleRepository : IModuleRepository
    {
        DapperHelper<SysModuleEntity> dbHelper = new DapperHelper<SysModuleEntity>();
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<SysModuleEntity> SingleByKey(int key)
        {
            return dbHelper.SingleByKey(key);
        }

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        public async Task<ResultObj<SysModuleEntity>> GetMenu(Expression<Func<SysModuleEntity, bool>> where)
        {
            ResultObj<SysModuleEntity> reObj = new ResultObj<SysModuleEntity>();
            var allModel = await dbHelper.FindAll(where);
            reObj.dataList = GetChildItems(allModel, 0);
            return reObj;
        }

        private List<SysModuleEntity> GetChildItems(IEnumerable<SysModuleEntity> inList, int parentId)
        {
            var childList = inList.Where(i => i.parentId == parentId).ToList();
            List<SysModuleEntity> reObj = new List<SysModuleEntity>();
            foreach (var item in childList)
            {
                item.children = GetChildItems(inList, item.id);
                reObj.Add(item);
            }
            return reObj;
        }

        public async Task<ResultObj<int>> Delete(int key)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            reObj.data = await dbHelper.Delete(i => i.id == key);
            reObj.success = reObj.data > 0;
            return reObj;
        }

        public async Task<ResultObj<int>> Save(DtoSave<SysModuleEntity> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            if (inEnt.data.id == 0)
            {
                inEnt.data.id = await new SequenceRepository().GetNextID<SysModuleEntity>();
                reObj.data = await dbHelper.Save(inEnt);
            }
            else
            {
                reObj.data = await dbHelper.Update(inEnt);
            }

            reObj.success = reObj.data > 0;
            return reObj;
        }

        public async Task<ResultObj<SysModuleEntity>> GetMenuByRoleId(List<int> roleIdList)
        {
            ResultObj<SysModuleEntity> reObj = new ResultObj<SysModuleEntity>();
            if (!roleIdList.Contains(1))
            {
                DapperHelper<FaRoleModuleEntityView> roleModule = new DapperHelper<FaRoleModuleEntityView>();
                var allModel = await roleModule.FindAll(string.Format("c.IS_HIDE==0 and a.ROLE_ID in ({0})", string.Join(",", roleIdList)));
                reObj.dataList = GetChildItems(Fun.ClassListToCopy<FaRoleModuleEntityView, SysModuleEntity>(allModel.ToList()), 0);
            }
            else
            {
                reObj.dataList = GetChildItems(await new DapperHelper<SysModuleEntity>().FindAll(i=>i.isHide==0), 0);
            }
            return reObj;
        }

        public async Task<ResultObj<SysModuleEntity>> GetMGetMenuByUserId(int userId)
        {

            DapperHelper<FaUserRoleEntityView> userRole = new DapperHelper<FaUserRoleEntityView>();
            var allRole = await userRole.FindAll(i => i.userId == userId);
            return await GetMenuByRoleId(allRole.Select(i => i.roleId).ToList());
        }
    }
}
