using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CodeFirst.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("UltimateEMT", throwIfV1Schema: false)
        {
        }

       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<M_Campaigns> M_Campaigns { get; set; }
        public DbSet<M_CampTypes> M_CampTypes { get; set; }
        public DbSet<NewList> NewLists { get; set; }
        public DbSet<S_Status> S_Status { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<M_UsersListCampaign> M_UsersListCampaign { get; set; }
        public DbSet<ListSusbscriber> ListSusbscribers { get; set; }

        public DbSet<UsersCampaign> UsersCampaigns { get; set; }

        public DbSet<UsersList> UsersList { get; set; }

        public DbSet<M_Tracking> M_Trackings { get; set; }

        public DbSet<CustomSqlException> CustomSqlExceptions { get; set; }
        public DbSet<M_MailStatus> MailStatus { get; set; }
    }

  
}