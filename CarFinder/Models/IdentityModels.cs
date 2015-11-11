using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CarFinder.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) {
        }

        public DbSet<Car> Car { get; set; }

        public async Task<List<string>> All_Years(string year) {
            return await this.Database.SqlQuery<string>("All_Years").ToListAsync();
        }

        public async Task<List<string>> Make_by_Year(string year) {
            var yearParam = new SqlParameter("@model_year", year);
            return await this.Database.SqlQuery<string>("Make_by_Year @model_year", yearParam).ToListAsync();
        }

        public async Task<List<string>> Model_by_Year_Make(string year, string make) {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make", make);
            return await this.Database.SqlQuery<string>("Model_by_Year_Make @model_year, @make", yearParam, makeParam).ToListAsync();
        }

        public async Task<List<string>> Trims_by_Year_Make_Model(string year, string make, string model) {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make", make);
            var modelParam = new SqlParameter("@model_name", model);
            return await this.Database.SqlQuery<string>("Trims_by_Year_Make_Model @model_year, @make, @model_name", yearParam, makeParam, modelParam).ToListAsync();
        }

        public async Task<List<Car>> Cars_by_Year(string year) {
            var yearParam = new SqlParameter("@model_year", year);
            return await this.Database.SqlQuery<Car>("Cars_by_Year @model_year", yearParam).ToListAsync();
        }

        public async Task<List<Car>> Cars_by_Year_Make(string year, string make) {
            var makeParam = new SqlParameter("@make", make);
            var yearParam = new SqlParameter("@model_year", year);
            return await this.Database.SqlQuery<Car>("Cars_by_Year_Make @model_year, @make", yearParam, makeParam).ToListAsync();
        }

        public async Task<List<Car>> Cars_by_Year_Make_Model(string year, string make, string model) {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make", make);            
            var modelParam = new SqlParameter("@model_name", model);
            return await this.Database.SqlQuery<Car>("Cars_by_Year_Make_Model @model_year, @make, @model_name", yearParam, makeParam, modelParam).ToListAsync();
        }

        public async Task<List<Car>> Cars_by_Year_Make_Model_Trim(string year, string make, string model, string trim) {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make", make);            
            var modelParam = new SqlParameter("@model_name", model);
            var trimParam = new SqlParameter("@model_trim", trim);
            return await this.Database.SqlQuery<Car>("Cars_by_Year_Make_Model_Trim @model_year, @make, @model_name, @model_trim", yearParam, makeParam, modelParam, trimParam).ToListAsync();
        }

        public async Task<List<Car>> Cars_by_Id(string id) {
            var idParam = new SqlParameter("@Id", id);
            return await this.Database.SqlQuery<Car>("Cars_by_Id @model_year", idParam).ToListAsync();
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }
}