using Microsoft.AspNetCore.Authorization;

namespace WxProductApi
{
    public class AdultPolicyRequirement : IAuthorizationRequirement
    {
        public int Age { get; }
        public AdultPolicyRequirement(int age)
        {
            //年龄限制
            this.Age = age;
        }
    }

}