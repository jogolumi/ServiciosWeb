using System.Web;
using System.Web.Mvc;

namespace WS_SeguroAsistencias_LBC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
