using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class M_MailStatus
    {
        [Key]
        public int StatusID { get; set; }

        public int SubscriberID { get; set; }

        [ForeignKey("M_Campaigns")]
        public int CampaignID { get; set; }

        [ForeignKey("Users")]
        public string UserID { get; set; }

        public virtual ApplicationUser Users { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public virtual M_Campaigns M_Campaigns { get; set; }
    }
}