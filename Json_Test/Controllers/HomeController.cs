using Json_Test.AppLib.Mail;
using Json_Test.AppLib.Template;
using Json_Test.Models;
using Json_Test.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Json_Test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            RequestSendModel_Sk model = new RequestSendModel_Sk();

            return View(model);
        }
        public ActionResult Json()
        {
            ViewBag.Message = "Programovanie Json";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(RequestSendModel_Sk model)
        {
            
            if (ModelState.IsValid)
            {
                bool robotOk = false;
                if (System.Configuration.ConfigurationManager.AppSettings["recaptchaVersion"] == "brendysoft")
                {
                    if (!new ApiKeyValidator().IsValid(model.Password, model.ConfirmPassword))
                    {
                        ModelState.AddModelError("", "Musíte označiť, že nie ste robot.");
                        //Mailer.SendAdminMail("Odoslanie ROBOT správy",
                        //    string.Format("Meno: '{0}'\nEmail: '{1}'\nTelefón: '{2}'\nText: '{3}'",
                        //    model.Name,
                        //    model.Email,
                        //    model.Phone,
                        //    model.Text));

                        //return RedirectToAction("ContactSendSuccess", "HomeSk");
                    }
                    else
                    {
                        robotOk = true;
                    }
                }
                else
                {
                    if (!new GoogleRecaptcha().IsValidRecaptcha())
                    {
                        ModelState.AddModelError("", "Musíte označiť, že nie ste robot.");
                    }
                    else
                    {
                        robotOk = true;
                    }
                }

                 robotOk = true;

                if (robotOk)
                {
                    if (!string.IsNullOrEmpty(model.Captcha))
                    {
                        ModelState.AddModelError("", "Neplatná požiadavka.");
                        Mailer.SendAdminMail("Neplatná požiadavka",
                            string.Format("Meno: '{0}'\nEmail: '{1}'\nText: '{2}'\nCaptcha: '{3}'", model.Name, model.Email, model.Text, model.Captcha));
                    }
                    else
                    {
                        List<TextTemplateParam> paramList = new List<TextTemplateParam>();
                        paramList.Add(new TextTemplateParam("NAME", model.Name));
                        paramList.Add(new TextTemplateParam("EMAIL", model.Email));
                        paramList.Add(new TextTemplateParam("PHONE", model.Phone));
                        paramList.Add(new TextTemplateParam("TEXT", model.Text));
                        paramList.Add(new TextTemplateParam("PRICE", string.IsNullOrEmpty(model.Price) ? string.Empty : string.Format("Vaša predstava o cene: {0}", model.Price)));

                        // Odoslanie uzivatelovi
                        Mailer.SendMailTemplate(
                            "Odoslanie správy",
                            TextTemplate.GetTemplateText("ContactSendSuccess", paramList),
                            model.Email, "_Sk", null);

                        return RedirectToAction("ContactSendSuccess", "Home");
                    }
                }
            }

            return View(model);
        }

        public ActionResult ContactSendSuccess()
        {
            
            return View();
        }

    }
}
