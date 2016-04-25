using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class Country
    {
        public static string GetURL()
        {
            HttpRequest request = HttpContext.Current.Request;
            string url = request.Url.ToString();
            return url;
        }
        public Country()
        {
            this.NewLists = new HashSet<NewList>();
        }
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public virtual ICollection<NewList> NewLists { get; set; }

        static ApplicationDbContext dbcontext = null;
        static List<Country> countries = new List<Country>();
        static CustomSqlException obj = null;
        public  static List<Country> GetCountries()
        {
            using (dbcontext = new ApplicationDbContext())
            {
                try
                {
                    countries = dbcontext.Countries.ToList();
                    return countries;
                }
                catch (SqlException ex)
                {

                    CustomSqlException obj = new CustomSqlException((int)ErorrTypes.SqlExceptions, "Problem in saving data", ex.StackTrace, ErorrTypes.SqlExceptions.ToString(), GetURL(), ex.LineNumber);

                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, "Some problem occured while processing request", ex.StackTrace, ErorrTypes.others.ToString(),GetURL());

                    obj.LogException();
                    throw obj;
                }
            }
        }
    }
}