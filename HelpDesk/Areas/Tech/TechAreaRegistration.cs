using System.Web.Mvc;

namespace HelpDesk.Areas.Tech
{
    public class TechAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Tech";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Tech_default",
                "Tech/{controller}/{action}/{id}",
                new { action = "Index", controller = "Tech",id = UrlParameter.Optional },
                new[] { "HelpDesk.Areas.Tech.Controllers" }
            );
        }
    }
}