using Models;
using Models.Entity;
using System.Threading.Tasks;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModuleController
    {

        /**
         * 查询单个
         * @param inEnt
         * @return
         */
        Task<ResultObj<SysModuleEntity>> singleByKey(DtoDo<int> inEnt);

        /**
         * 保存
         * @param inEnt
         * @return
         */
        Task<ResultObj<int>> save(DtoSave<SysModuleEntity> inEnt);

        /**
         * 删除
         * @param inEnt
         * @return
         */
        Task<ResultObj<int>> delete(DtoDo<int> inEnt);

        /**
         * 获取菜单
         * @return
         */
        Task<ResultObj<SysModuleEntity>> getUserMenu();

        //——代码分隔线——
    }
}
