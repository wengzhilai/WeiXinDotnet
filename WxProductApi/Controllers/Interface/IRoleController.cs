using Models;
using Models.Entity;
using System.Threading.Tasks;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRoleController
    {
        /**
         * 查询单条
         * @param inObj
         * @return
         */
        Task<ResultObj<FaRoleEntity>> singleByKey(DtoDo<int> inObj);

        /**
         * 删除
         * @param inObj
         * @return
         */
        Task<ResultObj<int>> delete(DtoDo<int> inObj);

        /**
         * 保存基本信息
         * @param inEnt
         * @return
         */
        Task<ResultObj<int>> save(DtoSave<FaRoleEntity> inEnt);


    }
}
