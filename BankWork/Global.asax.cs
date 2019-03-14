
namespace BankWork
{
    using System.Web.Http;
    using System.Web.Mvc;
    using BankWork.App_Start;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacConfig.RegisterDependies();
        }
    }
}
