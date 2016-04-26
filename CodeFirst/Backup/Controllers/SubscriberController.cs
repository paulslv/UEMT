using CodeFirst.Models;
using CodeFirst.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel;
using LumenWorks.Framework.IO.Csv;
using System.Data;
using System.IO;

namespace CodeFirst.Controllers
{
    public class SubscriberController : Controller
    {
        string userID = null;
        NewList list = null;
        Subscriber subscriber = null;
        public string GetUser()
        {
            userID = User.Identity.GetUserId();
            return userID;
        }
        // GET: Subscriber
        [HttpGet]
        public ActionResult AddSubcriber(int? id)
        {
            userID = GetUser();
            list = new NewList();
            subscriber = new Subscriber();
            if (id == null)
            {
                // ViewBag.ListName = new SelectList(dbcontext.NewLists, "ListID", "ListName", "Select List");
                ViewBag.ListName = new SelectList(NewList.GetLists(), "ListID", "ListName", "Select List");
            }
            else {
                subscriber.ListID = id;
                return View(subscriber);
            }
            return View(subscriber);
        }
        [HttpPost]
        public ActionResult AddSubcriber(Subscriber model)
        {
            userID = GetUser();
            if (model != null)
            {
                try
                {
                    model.saveSubscriber(userID);
                }
                catch (CustomSqlException ex)
                {
                    ModelState.AddModelError("addsub", ex.message);
                    return RedirectToAction("AddSubcriber/" + model.ListID);
                }
                
            }
            return RedirectToAction("Index", "List");
        }

        [HttpGet]
        public ActionResult ViewSubscribers(int? id, int? page)
        {
            userID = GetUser();
            const int pageSize = 30;
            int pageNumber = (page ?? 1);
            subscriber = new Subscriber();
            List<Subscriber> subscribersToList = new List<Subscriber>();
            if (id != null)
            {
                ViewBag.ListID = id;
                try
                {
                    subscribersToList = subscriber.GetSubscribersbyListID(id);
                    return View(subscribersToList.ToPagedList(pageNumber, pageSize));
                }
                catch (CustomSqlException ex)
                {
                    ModelState.AddModelError("viewsub", ex.message);
                    return RedirectToAction("index", "List");
                }
               
            }
            else {
                try
                {
                    subscribersToList = subscriber.GetAllSubscribers(userID);
                    return View(subscribersToList.ToPagedList(pageNumber, pageSize));
                }
                catch (CustomSqlException ex)
                {
                    ModelState.AddModelError("viewsub", ex.message);
                    return RedirectToAction("index", "Home");
                };
            }
        }
        [HttpGet]
        public ActionResult ImportSubcriber(int? id)
        {
            SubscribersViewModel model = new SubscribersViewModel();
            if (id == null)
            {
                ViewBag.ListName = new SelectList(NewList.GetLists(), "ListID", "ListName", "Select List");
            }
            else {
                model.ListID = id;
                return View(model);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ImportSubcriber(HttpPostedFileBase UploadFile, SubscribersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (UploadFile != null && UploadFile.ContentLength > 0)
                {
                    if (UploadFile.FileName.EndsWith(".xlsx"))
                    {
                        try
                        {
                            subscriber.ImportExcel(UploadFile, model);
                        }
                        catch (CustomSqlException ex)
                        {
                            ModelState.AddModelError("importsub", ex.message);
                            return RedirectToAction("ImportSubcriber/" + model.ListID);
                        }
                       
                    }
                    else if (UploadFile.FileName.EndsWith(".xls"))
                    {
                        try
                        {
                            subscriber.ImportExcel(UploadFile, model);
                        }
                        catch (CustomSqlException ex)
                        {
                            ModelState.AddModelError("importsub", ex.message);
                            return RedirectToAction("ImportSubcriber/" + model.ListID);
                        }

                    }
                    else if (UploadFile.FileName.EndsWith(".csv"))
                    {
                        try
                        {
                            subscriber.ImportCSV(UploadFile, model);
                        }
                        catch (CustomSqlException ex)
                        {
                            ModelState.AddModelError("importsub", ex.message);
                            return RedirectToAction("ImportSubcriber/" + model.ListID);
                        }
                    }
                }
            }
            return RedirectToAction("ViewSubscribers", model.ListID);
        }

        [HttpGet]
        public ActionResult EditSusbscriber(int? id)
        {
            if (id != null)
            {
                Subscriber model = new Subscriber();
                try
                {
                    model = model.Edit(id);
                    return View(model);
                }
                catch (CustomSqlException ex)
                {
                    ModelState.AddModelError("editsub", ex.message);
                    return RedirectToAction("EditSusbscriber/" + id);
                }
            }
            else {
                ModelState.AddModelError("idnull", "Error while processing request contact administrator");
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult EditSusbscriber(Subscriber model)
        {
            if (model != null)
            {
                try
                {
                    model.Update();
                    return RedirectToAction("ViewSubscribers/" + model.ListID);
                }
                catch (CustomSqlException ex)
                {
                    ModelState.AddModelError("editsub", ex.message);
                    return RedirectToAction("EditSusbscriber/"+model.SubscriberID);
                }
            }
            else {
                ModelState.AddModelError("modelnull", "Error while processing request contact administrator");
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteSusbscriber(int? id)
        {
            Subscriber model = new Subscriber();
            if (id != null)
            {
                try
                {
                    model.Delete(id);
                    return RedirectToAction("Index", "List");
                }
                catch (CustomSqlException ex)
                {
                    ModelState.AddModelError("editsub", ex.message);
                    return RedirectToAction("ViewSubscribers/" + ViewBag.id);
                }
            }
            else {
                ModelState.AddModelError("idnull", "Error while processing request contact administrator");
                return RedirectToAction("Index", "List");
            }
        }

        public ActionResult Unsubscribers(int? id)
        {
            userID = GetUser();
            subscriber = new Subscriber();
            SubscribersViewModel model = new SubscribersViewModel();
            try
            {
                model.SubscribersToList = subscriber.Unsubscriber(id);
                return View(model);
            }
            catch (CustomSqlException ex)
            {
                ModelState.AddModelError("unsub", ex.message);
                return RedirectToAction("Index","List");
            }
        }
    }
}