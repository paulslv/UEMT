using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class UsersList
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Users")]
        public string UsersID { get; set; }

        [ForeignKey("NewList")]
        public Nullable<int> ListID { get; set; }
        public virtual NewList NewList { get; set; }
        public virtual ApplicationUser Users { get; set; }
    }
}