using System;
using System.Web.Mvc;
using Presentation.Models;
using System.Configuration;
using Ak.MVC.Authentication;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn(BaseModel model, String returnUrl)
        {
            var realLogin = ConfigurationManager.AppSettings["login"];
            var realPass = ConfigurationManager.AppSettings["pass"];

            var typedLogin = model.LogOn.Login;
            var typedPass = model.LogOn.Password;


            typedPass = typedPass.EncryptPassword();


            if (realLogin != null && typedLogin == realLogin && typedPass == realPass)
            {
                Authenticate.Set(realLogin, Response);

                if (String.IsNullOrEmpty(returnUrl))
                    return RedirectToRoute("Author");
            
                return Redirect(returnUrl);
            }
            
            return Redirect(@"\");
        }

        public ActionResult LogOff()
        {
            Authenticate.Clean(Request);

            return Redirect(@"\");
        }
    }
}
