using Models;
using Models.Entity;
using System.Threading.Tasks;


namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoginController
    {
        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<string>> userLogin(LogingDto inEnt);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<int>> loginReg(LogingDto inEnt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Result> deleteUser(DtoKey userName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<bool>> userEditPwd(EditPwdDto inEnt);

    }

    /// <summary>
    /// 
    /// </summary>
    public class UserCodeLoginDto
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
