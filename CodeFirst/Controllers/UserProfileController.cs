using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;
using Microsoft.AspNet.Identity;


namespace CodeFirst.Controllers
{
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(HttpPostedFileBase Uploadlogo, M_Profile model)
        {
            if (ModelState.IsValid)
            {
                if (Uploadlogo != null && Uploadlogo.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(Uploadlogo.FileName);
                    string ext = System.IO.Path.GetExtension(Uploadlogo.FileName);
                    try
                    {
                        string logofile = fileName + "_" + User.Identity.GetUserId() + ext;
                        var path = System.IO.Path.Combine(Server.MapPath("~/Logo"), logofile);
                        Uploadlogo.SaveAs(path);
                        model.CompanyLogo = path;
                        try
                        {
                            model.SaveProfile();
                        }
                        catch (CustomSqlException ex)
                        {
                            ex.LogException();
                            return RedirectToAction("Index");
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return View("Index");
        }

    }
}