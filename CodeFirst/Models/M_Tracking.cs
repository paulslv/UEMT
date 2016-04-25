using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class M_Tracking
    {
        [Key]
        public int TrackID { get; set; }

        //[ForeignKey("M_Campaigns")]
        public int CampId { get; set; }

        //[ForeignKey("Subscriber")]
        public int SubsciberId { get; set; }
        public Nullable<System.DateTime> IsOpened { get; set; }

        public virtual M_Campaigns M_Campaigns { get; set; }
        public virtual Subscriber Subscriber { get; set; }
    }
}