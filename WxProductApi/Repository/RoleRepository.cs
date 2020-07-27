
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
    public class RoleRepository : IRoleRepository
    {
        DapperHelper<FaRoleEntity> dbHelper = new DapperHelper<FaRoleEntity>();
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<FaRoleEntity> SingleByKey(int key)
        {
            var ent = await dbHelper.SingleByKey(key);
            DapperHelper<FaRoleModuleEntityView> roleModule = new DapperHelper<FaRoleModuleEntityView>();
            ent.moduleIdStr = (await roleModule.FindAll(i => i.ROLE_ID == key)).Select(i => i.MODULE_ID).ToArray();
            return ent;
        }

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        public Task<IEnumerable<FaRoleEntity>> FindAll(Expression<Func<FaRoleEntity, bool>> where)
        {
            return dbHelper.FindAll(where);
        }

        async public Task<List<FaUserRoleEntityView>> UserRoleList(int userId)
        {
            DapperHelper<FaUserRoleEntityView> dp = new DapperHelper<FaUserRoleEntityView>();
            var reList = await dp.FindAll(x => x.userId == userId);
            return reList.ToList();
        }

        /// <summary>
        ///  权限字符串，第一位表示创建者，第二位管理员，第三位表示超级管理员
        ///  判断的权限，1添加，2修改，4查看
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authority"></param>
        /// <param name="prowNum">验证的权限，1添加，2修改，4查看</param>
        /// <param name="isCreater">是否是创建者</param>
        /// <returns></returns>
        async public Task<bool> CheckAuth(CheckAuthDto inEnt)
        {

            if (string.IsNullOrEmpty(inEnt.Authority)) return true;
            if (!inEnt.Authority.IsOnlyNumber()) return true;
            //是创建者
            if (inEnt.IsCreater)
            {
                if (Fun.GetPowerList(inEnt.Authority.Substring(0, 1)).Contains(inEnt.PowerNum)) return true;
            }

            var allRole = await UserRoleList(inEnt.UserId);
            //是普通管理员
            if (allRole.SingleOrDefault(i => i.roleId == 2) != null && inEnt.Authority.Length > 1)
            {
                if (Fun.GetPowerList(inEnt.Authority.Substring(1, 1)).Contains(inEnt.PowerNum)) return true;
            }

            //是超级管理员
            if (allRole.SingleOrDefault(i => i.roleId == 1) != null && inEnt.Authority.Length > 2)
            {
                if (Fun.GetPowerList(inEnt.Authority.Substring(2, 1)).Contains(inEnt.PowerNum)) return true;
            }
            return true;
        }

        public async Task<ResultObj<int>> Delete(int key)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            reObj.data = await dbHelper.Delete(i => i.id == key);
            reObj.success = reObj.data > 0;
            return reObj;
        }

        public async Task<ResultObj<int>> Save(DtoSave<FaRoleEntity> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                dbHelper.TranscationBegin();
                DapperHelper<FaModuleEntity> moduleDapper = new DapperHelper<FaModuleEntity>(dbHelper.GetConnection(), dbHelper.GetTransaction());

                if (inEnt.data.id == 0)
                {
                    inEnt.data.id = await new SequenceRepository().GetNextID<FaRoleEntity>();
                    reObj.data = await dbHelper.Save(inEnt);
                }
                else
                {
                    reObj.data = await dbHelper.Update(inEnt);
                }
                reObj.success = reObj.data > 0;
                if (!reObj.success)
                {
                    reObj.msg = "保存角色失败";
                    dbHelper.TranscationRollback();
                }
                else
                {
                    var allModule = await moduleDapper.FindAll(string.Format("ID in ({0})", string.Join(",", inEnt.data.moduleIdStr)));
                    var moduleIdList = allModule.Select(i => i.id).ToList();
                    var parent = allModule.GroupBy(i => i.parentId).Select(x => x.Key).Where(x => x != 0).Select(x => x).ToList();
                    moduleIdList = moduleIdList.Concat(parent).ToList();
                    moduleIdList=moduleIdList.GroupBy(i=>i).Select(i=>i.Key).ToList();
                    new DapperHelper().Init(dbHelper.GetConnection(), dbHelper.GetTransaction());
                    await new DapperHelper().Exec("delete from fa_role_module where ROLE_ID = " + inEnt.data.id);
                    var opNum = await new DapperHelper().Exec(string.Format("insert into fa_role_module(ROLE_ID,MODULE_ID) select {0} ROLE_ID,ID MODULE_ID from fa_module where ID IN ({1}) ", inEnt.data.id, string.Join(",", moduleIdList)));
                    if (opNum != moduleIdList.Count())
                    {
                        reObj.success = false;
                        reObj.msg = "保存角色模块失败";
                        dbHelper.TranscationRollback();
                    }
                    else
                    {
                        reObj.success=true;
                        dbHelper.TranscationCommit();
                    }
                }

            }
            catch (Exception e)
            {
                reObj.success = false;
                reObj.msg = "保存角色失败";
                LogHelper.WriteErrorLog(this.GetType(), reObj.msg, e);
                dbHelper.TranscationRollback();
            }
            return reObj;
        }

    }
}
