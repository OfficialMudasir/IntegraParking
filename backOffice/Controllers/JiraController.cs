using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace backOffice.Controllers
{
    public class JiraController : Controller
    {
        //
        // GET: /Jira/

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult IssueChanged()
        {
            string sLine = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff") + " - IssueChange (Get): ";
            foreach (string key in Request.Params.AllKeys) {
                sLine += key + "=" + Request.Params[key] + "\n\r";
            }
            using (StreamWriter sw = new StreamWriter(Path.Combine(Server.MapPath("~/App_Data"), "jira.log"), true))
            {
                sw.WriteLine(sLine);
                sw.Close();
            }
            
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult IssueChanged(string id, string data)
        {                
            string sLine = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff") + " - IssueChange (Post):\n\r" + id + "\n\r" + data + "\n\r";
            foreach (string key in Request.Form.AllKeys)
            {
                sLine += key + "=" + Request.Form[key] + "\n\r";
            }
            foreach (string key in Request.QueryString.AllKeys)
            {
                sLine += key + "=" + Request.QueryString[key] + "\n\r";
            }
            /*foreach (string key in Request.Params.AllKeys)
            {
                sLine += key + "=" + Request.Params[key] + "\n\r";
            }*/
            
            if (Request.InputStream != null)
            {
                sLine += "InputStream:";
                using (StreamReader sr = new StreamReader(Request.InputStream))
                {
                    sLine += sr.ReadToEnd() + "\n\r";
                    sr.Close();
                }
            }
            using (StreamWriter sw = new StreamWriter(Path.Combine(Server.MapPath("~/App_Data"), "jira.log"), true))
            {
                sw.WriteLine(sLine);
                sw.Close();
            }

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

    }
}
