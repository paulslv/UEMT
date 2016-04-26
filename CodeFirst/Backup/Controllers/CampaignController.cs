using CodeFirst.Helpers;
using CodeFirst.Models;
using CodeFirst.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;

namespace CodeFirst.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        UsersCampaign user = new UsersCampaign();
        List<int?> cid = new List<int?>();
        List<string> CampNames = new List<string>();
        static string userID = null;
        public string GetUser()
        {
            userID = User.Identity.GetUserId();
            return userID;
        }
        // GET: Campaign
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Campaign()
        {
            user.UsersID = User.Identity.GetUserId();

            CampaignViewModel obj = new CampaignViewModel();
            cid = dbContext.UsersCampaigns.Where(u => u.UsersID == user.UsersID).Select(c => c.CampaignID).ToList();
            if (cid.Count != 0)
            {
                foreach (var item in cid)
                {
                    obj.Campaigns.Add(dbContext.M_Campaigns.Where(u => u.Cid == item).FirstOrDefault());
                }

            }

            //obj.Campaigns = dbContext.M_Campaigns.Where(c => c.IsActive == true).ToList();

            return View(obj);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Creation()
        {
            userID = GetUser();
            ViewBag.campTypes = new SelectList(M_CampTypes.GetCampTypes(), "CTId", "Name");
            ViewBag.List = new SelectList(NewList.GetLists(userID), "ListID", "ListName");
            return View();
        }
        public ActionResult Designer()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Thanks()
        {
            return View();
        }
        public ActionResult UpdateDesign()
        {
            return View();
        }

        public ActionResult registerCampaign(Models.M_Campaigns model)
        {
            TempData["CampInfo"] = model;

            return View("Designer", model);

        }
        public ActionResult saveCampInfo(string UserName)
        {
            user.UsersID = User.Identity.GetUserId();
            M_Campaigns model = (M_Campaigns)TempData["CampInfo"];
            try
            {
                var emailContent = WebUtility.HtmlEncode(UserName);
                model.EmailContent = emailContent;
                model.StatusId = 1;
                model.IsActive = true;
                model.SaveCampaign(userID);
                return RedirectToAction("Campaign");
            }
            catch (CustomSqlException ex)
            {
                if (ex.ErrorCode == 100)
                {
                    ModelState.AddModelError("error", ex.message);
                    return RedirectToAction("Creation");
                }
                else if (ex.ErrorCode == 101)
                {
                    ModelState.AddModelError("Error", "logical exception");
                    return RedirectToAction("Creation");
                }
            }
            return RedirectToAction("Creation");
        }

        [HttpGet]
        public ActionResult editCampaign(int id)
        {
            UpdateCampModel campaign = new UpdateCampModel();
            campaign.Campaigns = dbContext.M_Campaigns.Find(id);
            campaign.Subscribers = dbContext.NewLists.Find(campaign.Campaigns.ListId).Subscribers.ToList();
            WebUtility.HtmlDecode(campaign.Campaigns.EmailContent);
            ViewBag.campTypes = new SelectList(dbContext.M_CampTypes, "CTId", "Name");
            ViewBag.List = new SelectList(dbContext.NewLists, "ListID", "ListName");
            return View("UpdateCamp", campaign);

        }
        [HttpPost]
        public ActionResult editCampaign(UpdateCampModel model)
        {
            if (ModelState.IsValid)
            {
                model.Campaigns.StatusId = 1;
                model.Campaigns.IsActive = true;
                WebUtility.HtmlEncode(model.Campaigns.EmailContent);
                try
                {
                    dbContext.Entry(model.Campaigns).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("Campaign");
        }

        [HttpGet]
        public ActionResult editDesigner(int? id)
        {
            M_Campaigns cmodel = new M_Campaigns();
            TempData["CID"] = id;
            String EmailContent = dbContext.M_Campaigns.Where(c => c.Cid == id).Select(e => e.EmailContent).FirstOrDefault();
            cmodel.EmailContent = EmailContent;
            return View("UpdateDesign", cmodel);
        }
        [HttpPost]
        public ActionResult editDesigner(string EmailContent)
        {
            M_Campaigns model = new M_Campaigns();
            int id = Convert.ToInt32(TempData["CID"]);
            model = dbContext.M_Campaigns.SingleOrDefault(c => c.Cid == id);
            try
            {
                if (model != null)
                {
                    model.EmailContent = WebUtility.HtmlEncode(EmailContent);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("Index", "Home", null);
            }
            catch (SqlException ex)
            {
                throw ex;
                return RedirectToAction("Campaign");
            }
            catch (Exception ex)
            {
                throw ex;
                return RedirectToAction("Campaign");
            }

        }

        public ActionResult DeleteCamp(int? id)
        {
            if (id != null)
            {
                M_Campaigns model = new M_Campaigns();
                model = dbContext.M_Campaigns.Find(id);
                model.IsActive = false;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Campaign"); ;
        }

        public ActionResult EditStatus(int? id)
        {
            if (id != null)
            {
                M_Campaigns model = new M_Campaigns();
                model = dbContext.M_Campaigns.Find(id);
                model.IsActive = true;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Campaign"); ;
        }
        public async Task<ActionResult> SendEmailAsync(int? id)
        {
            M_Campaigns campaign = new M_Campaigns();
            string pattern = "{}";
            //string body = null;
            campaign = dbContext.M_Campaigns.Find(id);
            List<Subscriber> emailToList = new List<Subscriber>();
            emailToList = dbContext.Subscribers.Where(s => s.ListID == campaign.ListId).ToList();
            if (emailToList.Count != 0)
            {
                foreach (var email in emailToList)
                {
                    if (email.Unsubscribe == false)
                    {
                        var emailSender = new EmailSender()
                        {
                            FromAddress = new System.Net.Mail.MailAddress(campaign.FromEmail, campaign.FromName),
                            Subject = campaign.EmailSubject,
                            Body = WebUtility.HtmlDecode(campaign.EmailContent)
                        };
                        Regex reg = new Regex(pattern);
                        emailSender.Body = reg.Replace(emailSender.Body, email.FirstName + " " + email.LastName);
                        // body = emailSender.Body.Replace("{1}", email.FirstName + " " + email.LastName);
                        //  emailSender.Body = body;
                        emailSender.Body += getFooter(campaign.Cid, email.SubscriberID);
                        emailSender.AddToAddress(email.EmailAddress, email.FirstName + " " + email.LastName);
                        await emailSender.SendMailAsync(campaign.CTypeId);
                    }
                }
            }


            return RedirectToAction("Thanks");

        }

        private string getFooter(int campaign, int subscriber)
        {
            string foot, imgNm;
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            imgNm = campaign + "_" + subscriber + ".png";

            foot = "<br><br><br><br><br><br>";
            foot += @"<table width='100%' style='background-color:#ffffff;border-top:1px solid #e5e5e5' > 
            <tr>    
            <td align='center'>";

            foot += " <img src='http://uemttrk.azurewebsites.net/rowemt/img/";
            foot += imgNm;
            foot += "'  height='30px' alt='Logo' />";

            foot += @"
            <br />     
            <p style='font-size:10px'>Copyright © 2016 Return On Web pvt Ltd. All rights reserved.</p>
            </td>
            </tr>
            <tr>
            <td align='center' style='font-size:8px'>
            <p>Don't want it in your inbox ? <a href='" + baseUrl + "/List/UnSubscribe?SubscriberID=" + subscriber;
            //foot += subscriber;
            foot += @"'><unsubscribe>Unsubscribe</unsubscribe></a>.
            </td>
            </tr>
            </table>";
            return foot;
        }
    }
}