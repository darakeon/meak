using System;
using System.Web.Mvc;
using Presentation.Models;
using Ak.MVC.Authentication;
using Structure.Helpers;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn(BaseModel model, String returnUrl)
        {
            var realLogin = Config.Login;
            var realPass = Config.Pass;

            var typedLogin = model.LogOn.Login;
            var typedPass = model.LogOn.Password;


            typedPass = typedPass.EncryptPassword();


            if (realLogin != null && realPass != null
                && typedLogin == realLogin && typedPass == realPass)
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
