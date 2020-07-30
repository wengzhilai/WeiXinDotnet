

using System;
using Microsoft.AspNetCore.Authorization;

namespace WxProductApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionCheckAttribute : AuthorizeAttribute
    {

        public PermissionCheckAttribute(string policy) : base(policy)
        {
        }

        public PermissionCheckAttribute()
        {
            var t= this;
        }
    }
}