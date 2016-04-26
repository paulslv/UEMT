using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class M_CampTypes
    {
        public static string GetURL()
        {
            HttpRequest request = HttpContext.Current.Request;
            string url = request.Url.ToString();
            return url;
        }
        public M_CampTypes()
        {
            this.M_Campaigns = new HashSet<M_Campaigns>();
        }
        [Key]
        public int CTId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

       // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_Campaigns> M_Campaigns { get; set; }

        static ApplicationDbContext dbcontext = null;
        static List<M_CampTypes> campTypes = null;
        static CustomSqlException obj = null;
        public static List<M_CampTypes> GetCampTypes()
        {
            campTypes = new List<M_CampTypes>();
            using (dbcontext = new ApplicationDbContext())
            {
               
                try
                {
                    campTypes = dbcontext.M_CampTypes.ToList();
                    return campTypes;
                }
                catch (SqlException ex)
                {

                    CustomSqlException obj = new CustomSqlException((int)ErorrTypes.SqlExceptions, "Problem in saving data", ex.StackTrace, ErorrTypes.SqlExceptions.ToString(),  GetURL(), ex.LineNumber);

                    obj.LogException();
                    throw obj;
                }
                catch (Exception ex)
                {
                    obj = new CustomSqlException((int)ErorrTypes.others, "Some problem occured while processing request", ex.StackTrace, ErorrTypes.others.ToString(),  GetURL());

                    obj.LogException();
                    throw obj;
                }
            }
        }
    }
}