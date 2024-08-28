using System.Web;
using System.Web.Mvc;

namespace FUEL_DISPATCH_API.CrystalRpt
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
