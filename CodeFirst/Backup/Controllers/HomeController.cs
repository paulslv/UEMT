using CodeFirst.Models;
using CodeFirst.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace CodeFirst.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        List<Subscriber> Slists = new List<Subscriber>();
        List<int?> listIDs = new List<int?>();
        List<M_Campaigns> campaigns = new List<M_Campaigns>();
        int cnt,cnt1;
        public ActionResult Index()
        {
            string uid = User.Identity.GetUserId();
            DashboardViewModel model = new DashboardViewModel();
            listIDs = dbContext.UsersList.Where(u => u.UsersID == uid).Select(l => l.ListID).ToList();
            foreach (var item in listIDs)
            {
                
                cnt += dbContext.Subscribers.Where(l => l.ListID == item && l.Unsubscribe==false).Count();
                cnt1 += dbContext.Subscribers.Where(l => l.ListID == item && l.Unsubscribe == true).Count();
            }
            model.NoOfLists = dbContext.UsersList.Where(u => u.UsersID == uid).Count();
            model.NoOfSubscribers = cnt;
            model.NoOfUnSubscribers = cnt1;
            model.NoOfCampaigns = dbContext.UsersCampaigns.Where(u => u.UsersID == uid).Count();
            return View(model);
           // return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}