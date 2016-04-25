using CodeFirst.Models;
using CodeFirst.ViewModels;
using Excel;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using Excel.Log;

namespace CodeFirst.Controllers
{

    public class ListController : Controller
    {
        NewList list = null;
        Subscriber subscribers = new Subscriber();
        List<int?> listIds = new List<int?>();
        ListViewModel LVmodel = new ListViewModel();
        string userID = null;
        public string GetUser()
        {
            userID = User.Identity.GetUserId();
            return userID;
        }
        // GET: List
        [Authorize]
        public ActionResult Index()
        {
            list = new NewList();
            LVmodel = new ListViewModel();
            userID = GetUser();
           
            try
            {
                LVmodel = list.ViewList(userID);
            }
            catch (CustomSqlException ex)
            {
                ModelState.AddModelError("listindex", ex.message);
                return View();
            }

            return View(LVmodel);
        }
               
        [Authorize]
        [HttpGet]
        public ActionResult NewList()
        {
            ViewBag.country = new SelectList(Country.GetCountries(), "CountryId", "CountryName");
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult NewList(NewList model)
        {

            if (ModelState.IsValid)
            {
                string UsersID = User.Identity.GetUserId();
                List<string> ListNames = new List<string>();
                string success = null;
                if (model != null)
                {
                    try
                    {
                        success=model.SaveList(UsersID);
                        if (success != "Saved")
                        {
                            ModelState.AddModelError("Errorlistname", success);
                            ViewBag.country = new SelectList(Country.GetCountries(), "CountryId", "CountryName");
                            return View();
                        }
                    }
                    catch (CustomSqlException ex)
                    {
                        if (ex.ErrorCode == 100)
                        {
                            ModelState.AddModelError("error", ex.message);
                            return RedirectToAction("NewList");
                        }
                        else if (ex.ErrorCode == 101)
                        {
                            ModelState.AddModelError("Error", "logical exception");
                            return RedirectToAction("NewList");
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("error", "Errr occured in business logic");
                        return RedirectToAction("NewList");
                    }
                   
                }
                else {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return RedirectToAction("Index");
        }
       
        [Authorize]
        public FilePathResult SampleExcel(string fileName)
        {
            return new FilePathResult(@"~\ExcelFiles\" + fileName + ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        [Authorize]
        public FileContentResult SampleCSV(string fileName)
        {
            //return new FilePathResult(@"~\CSVFiles\" + fileName + ".csv", "text/csv");
            var path = "FirstName, LastName, EmailAddress, AlternateEmailAddress, Address, Country, City";
            return File(new System.Text.UTF8Encoding().GetBytes(path), "text/csv", "SampleCSV.csv");
        }
        [Authorize]
        public ActionResult EditList(int? id)
        {
            ViewBag.country = new SelectList(Country.GetCountries(), "CountryId", "CountryName");
            NewList model = new NewList();
            try
            {
                model = model.EditList(id);
            }
            catch (CustomSqlException ex)
            {
                if (ex.ErrorCode == 100)
                {
                    ModelState.AddModelError("error", ex.message);
                    return RedirectToAction("EditList");
                }
                else if (ex.ErrorCode == 101)
                {
                    ModelState.AddModelError("Error", "logical exception");
                    return RedirectToAction("EditList");
                }
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditList(NewList model)
        {
            bool result;
            try
            {
                result = model.UpdateList();
                return RedirectToAction("Index");
            }
            catch (CustomSqlException ex)
            {
                ModelState.AddModelError("upadateList", ex.message);
                return RedirectToAction("EditList");
            }
        }

        [AllowAnonymous]
        public ActionResult UnSubscribe(int? SubscriberID)
        {
            Subscriber model = new Subscriber();
            bool res;
            if (SubscriberID != null)
            {
                try
                {
                    res = model.Unsub(SubscriberID);
                    if (res)
                    {
                        return JavaScript("window.close();");
                    }
                }
                catch (CustomSqlException ex)
                {
                    ModelState.AddModelError("Unsub", ex.message);
                }
            }
            return JavaScript("window.close()");

        }

    }
}