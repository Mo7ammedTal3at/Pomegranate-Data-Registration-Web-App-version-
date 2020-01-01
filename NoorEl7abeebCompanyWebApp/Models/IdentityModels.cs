using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NoorEl7abeebCompanyWebApp.Models.MyModels;
using System.ComponentModel.DataAnnotations;

namespace NoorEl7abeebCompanyWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Manager : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Manager> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [Required(ErrorMessage ="لابد من إدخال أسم الشخص")]

        public string Name { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<Manager>
    {
        public ApplicationDbContext()
            : base("MyConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<Export> Exports { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<BoxType> BoxTypes { get; set; }
        public DbSet<RestBoxes> RestBoxeses { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}