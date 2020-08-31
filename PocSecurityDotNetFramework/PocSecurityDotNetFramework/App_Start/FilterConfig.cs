using PocSecurity.Filter;
using System.Web;
using System.Web.Mvc;

namespace PocSecurityDotNetFramework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
