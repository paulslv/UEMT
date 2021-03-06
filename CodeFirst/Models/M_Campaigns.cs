﻿using CodeFirst.Helpers;
using CodeFirst.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace CodeFirst.Models
{
    public class M_Campaigns
    {
        public M_Campaigns()
        {
            this.M_UsersListCampaign = new HashSet<M_UsersListCampaign>();
        }
        [Key]
        public int Cid { get; set; }
        public string Name { get; set; }

        [ForeignKey("M_CampTypes")]
        public int CTypeId { get; set; }

        [ForeignKey("NewList")]
        public int ListId { get; set; }
        public string EmailSubject { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string EmailContent { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }

        public virtual M_CampTypes M_CampTypes { get; set; }
        public virtual NewList NewList { get; set; }
        public virtual S_Status S_Status { get; set; }
        public virtual ICollection<M_UsersListCampaign> M_UsersListCampaign { get; set; }

        static ApplicationDbContext dbcontext;
        static CustomSqlException obj;
        static List<int?> listIDs;
        List<string> campaignNames;
        List<int?> campIds;
        static M_Campaigns campaign;
        static List<M_Campaigns> campaigns = null;
        public static string GetURL()
        {
            HttpRequest request = HttpContext.Current.Request;
            string url = request.Url.ToString();
            return url;
        }
        public bool SaveCampaign(string userID)
        {
            UsersCampaign user = new UsersCampaign();
            if (!CheckCampaignExist(this.Name, userID))
            {
                try
                {
                    using (dbcontext = new ApplicationDbContext())
                    {
                        using (var trans = dbcontext.Database.BeginTransaction())
                        {
                            try
                            {
                                dbcontext.M_Campaigns.Add(this);

                                dbcontext.SaveChanges();

                                user.CampaignID = this.Cid;
                                user.UsersID = userID;
                                dbcontext.UsersCampaigns.Add(user);
                                dbcontext.SaveChanges();
                                trans.Commit();
                                return true;
                            }
                            catch (SqlException ex)
                            {
                                trans.Rollback();
                                obj = new CustomSqlException((int)ErorrTypes.SqlExceptions, ex.Message, ex.StackTrace, ErorrTypes.SqlExceptions.ToString(), userID, GetURL(), ex.LineNumber);

                                obj.LogException();
                                throw obj;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), userID, GetURL());

                                obj.LogException();
                                throw obj;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), userID, GetURL());

                    obj.LogException();
                    throw obj;
                }
            }
            else {
                return false;
            }
        }

        public bool CheckCampaignExist(string campaignName, string userID)
        {
            listIDs = new List<int?>();
            bool res = false;
            // listIDs = GetCampaignIDs(userID);
            campaignNames = new List<string>();
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {

                    var campaigndata = from camp in dbcontext.M_Campaigns
                                       join user in dbcontext.UsersCampaigns
                                       on camp.Cid equals user.CampaignID
                                       where user.UsersID == userID
                                       select new { camp.Name };

                    //where user.UsersID == userID);
                    //select camp.Name.Any();
                    if (campaigndata != null)
                    {
                        res = campaigndata.Any(c => c.Name == campaignName);
                        // res = true;
                    }
                    //foreach (var item in listIDs)
                    //{
                    //    campaignNames.Add(dbcontext.M_Campaigns.Where(c => c.Cid == item).Select(c => c.Name).FirstOrDefault());
                    //}
                    //res= campaignNames.Any(c => c == campaignName);
                    return res;
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }

        }

        public List<int?> GetCampaignIDs(string userID)
        {
            campIds = new List<int?>();
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    campIds = dbcontext.UsersCampaigns.Where(u => u.UsersID == userID).Select(c => c.CampaignID).ToList();
                    return campIds;
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }
        }

        public static List<M_Campaigns> ViewCampaigns(string userID)
        {
            campaigns = new List<M_Campaigns>();
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    campaigns = (from camp in dbcontext.M_Campaigns
                                 join user in dbcontext.UsersCampaigns
                                 on camp.Cid equals user.CampaignID
                                 where user.UsersID == userID
                                 select camp).ToList();
                    return campaigns;

                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, "Some problem occured while processing request", ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }
        }

        public static M_Campaigns FindCampaign(int? cid)
        {
            campaign = new M_Campaigns();
            try
            {
                using (dbcontext = new ApplicationDbContext())
                {
                    campaign = dbcontext.M_Campaigns.Find(cid);
                    return campaign;
                }
            }
            catch (SqlException ex)
            {
                obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                obj.LogException();
                throw obj;
            }
            catch (Exception ex)
            {
                obj = new CustomSqlException((int)ErorrTypes.others, "Some problem occured while processing request", ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                obj.LogException();
                throw obj;
            }
        }

        public static List<Subscriber> subscribersToCampaign(int? Lid)
        {
            List<Subscriber> Subscribers = new List<Subscriber>();
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    Subscribers = dbcontext.NewLists.Find(Lid).Subscribers.ToList();
                    return Subscribers;
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }
        }

        public static void UpdateCampaign(UpdateCampModel model)
        {
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    dbcontext.Entry(model.Campaigns).State = System.Data.Entity.EntityState.Modified;
                    dbcontext.SaveChanges();
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }
        }

        public static string EditTemplate(int? cid)
        {
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    // campaign = new M_Campaigns();
                    string emailContent = null;
                    emailContent = dbcontext.M_Campaigns.Where(c => c.Cid == cid).Select(e => e.EmailContent).FirstOrDefault();
                    return emailContent;
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, "Some problem occured while processing request", ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }
        }

        public static void UpdateTemplate(string EmailContent, int? cid)
        {
            campaign = new M_Campaigns();
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    campaign = dbcontext.M_Campaigns.SingleOrDefault(c => c.Cid == cid);
                    if (campaign != null)
                    {
                        campaign.EmailContent = WebUtility.HtmlEncode(EmailContent);
                        dbcontext.SaveChanges();
                    }
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }

        }

        public static void DisableCampaign(int? cid)
        {
            campaign = new M_Campaigns();
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    campaign = dbcontext.M_Campaigns.Find(cid);
                    campaign.IsActive = false;
                    dbcontext.SaveChanges();
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }
        }

        public static void EnableCampaign(int? cid)
        {
            campaign = new M_Campaigns();
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    campaign = dbcontext.M_Campaigns.Find(cid);
                    campaign.IsActive = true;
                    dbcontext.SaveChanges();
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, ex.Message, ex.StackTrace, ErorrTypes.others.ToString(), GetURL());
                    obj.LogException();
                    throw obj;
                }
            }
        }
    }
}