using System.Web.Mvc;

namespace HelpDesk.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Manager_default",
                "Manager/{controller}/{action}/{id}",
                new { action = "Index", controller = "Manager",id = UrlParameter.Optional },
                new[] { "HelpDesk.Areas.Manager.Controllers" }
            );
        }
    }
}