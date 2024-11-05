using UserAccountMangment.Models;

namespace UserAccountMangment.Authentication
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class CheckPermissionAttribute :Attribute
    {
        private readonly Permission _permission;
        public CheckPermissionAttribute( Permission permission)
        {
           _permission = permission;
        }
        public Permission Permission { get; }
    }
}
