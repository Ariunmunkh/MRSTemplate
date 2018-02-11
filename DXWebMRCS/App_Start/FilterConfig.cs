using System.Web;
using System.Web.Mvc;
using DXWebMRCS.App_Start;

namespace DXWebMRCS {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequreSecureConnectionFilter());
        }
    }
}