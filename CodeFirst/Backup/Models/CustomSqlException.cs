using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace CodeFirst.Models
{
    public class CustomSqlException:Exception
    {


        public CustomSqlException(string Message) : base()
        {
            this.message = Message;
        }
        public CustomSqlException(int ec, string msg, string st, string type, string user,string url) : base()
        {
            this.ErrorCode = ec;
            this.message = msg;
            this.stackTrace = st;
            this.Type = type;
            this.time = DateTime.Now;
            this.userId = user;
            this.url = url;
        }
        public CustomSqlException(int ec, string msg, string st, string type, string user, string url, int linenum) : base()
        {
            this.ErrorCode = ec;
            this.message = msg;
            this.stackTrace = st;
            this.Type = type;
            this.time = DateTime.Now;
            this.userId = user;
            this.url = url;
            this.lineNumber = linenum;
        }
        public CustomSqlException(int ec, string msg, string st, string type, string url, int linenum) : base()
        {
            this.ErrorCode = ec;
            this.message = msg;
            this.stackTrace = st;
            this.Type = type;
            this.time = DateTime.Now;
            this.url = url;
            this.lineNumber = linenum;
        }
        public CustomSqlException(int ec, string msg, string st, string type, string url)
        {
            this.ErrorCode = ec;
            this.message = msg;
            this.stackTrace = st;
            this.Type = type;
            this.url = url;
        }

        [Key]
        public int ID { get; set; }
        public int ErrorCode { get; set; }

        public string message { get; set; }
        public string stackTrace { get; set; }

        public string Type { get; set; }

        public DateTime time { get; set; }

        public string url { get; set; }

        public bool status { get; set; }

        public string userId { get; set; }
        public int lineNumber { get; set; }



        public virtual ApplicationUser User { get; set; }

        static ApplicationDbContext dbcontext = null;
        CustomSqlException obj = null;
        public void LogException()
        {
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    dbcontext.CustomSqlExceptions.Add(this);
                    dbcontext.SaveChanges();
                }
                catch (SqlException ex)
                {
                   obj =new CustomSqlException((int)ErorrTypes.others, "Some problem occured while processing request", ex.StackTrace, ErorrTypes.others.ToString(), GetURL(), ex.LineNumber);
                    throw obj;
                }
            }
        }
        public static string GetURL()
        {
            HttpRequest request = HttpContext.Current.Request;
            string url = request.Url.ToString();
            return url;
        }
    }
    public enum ErorrTypes
    {
        SqlExceptions = 100,
        LogicExceptions = 101,
        ArgumentNullExceptions = 102,
        ApplicationExceptions = 103,
        HttpExceptions = 104,
        others=105,
        SMTPExceptions=106
    }
}