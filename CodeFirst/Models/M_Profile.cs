using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class M_Profile
    {
        [Key]
        public int Pid { get; set; }

        public string CompanyName { get; set; }

        public string CompanyLogo { get; set; }

        public string Address { get; set; }

        public string Domain { get; set; }

        [ForeignKey("Users")]
        public string UserID { get; set; }

        public virtual ApplicationUser Users { get; set; }

        public static string GetURL()
        {
            HttpRequest request = HttpContext.Current.Request;
            string url = request.Url.ToString();
            return url;
        }
        static ApplicationDbContext dbcontext;
        static CustomSqlException obj;

        public void SaveProfile()
        {
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    dbcontext.M_Profiles.Add(this);
                    dbcontext.SaveChanges();
                }
                catch (SqlException ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.SqlExceptions, ex.Message, ex.StackTrace, ErorrTypes.SqlExceptions.ToString(), GetURL(), ex.LineNumber);

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