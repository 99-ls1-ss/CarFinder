using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

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

        public  async Task<int> Count_Cars_Year_Make_Model_Trim_Filter(string year, string make, string model, string trim, string filter) {
            var yearParam = new SqlParameter("@model_year", year ?? "");
            var makeParam = new SqlParameter("@make", make ?? "");
            var modelParam = new SqlParameter("@model_name", model ?? "");
            var trimParam = new SqlParameter("@model_trim", trim ?? "");
            var filterParam = new SqlParameter("@filter", filter ?? "");
            var result = await this.Database.SqlQuery<int>("Count_Cars_Year_Make_Model_Trim_Filter @model_year, @make, @model_name, @model_trim, @filter",
                yearParam, makeParam, modelParam, trimParam, filterParam).FirstAsync();
            return result;
        }

        public async Task<List<Car>> FindCars(string year, string make, string model, string trim, string filter, bool? paging, int? page, int? perPage, string sortColumn, string sortDirection) {
            var yearParam = new SqlParameter("@model_year", year ?? "");
            var makeParam = new SqlParameter("@make", make ?? "");
            var modelParam = new SqlParameter("@model_name", model ?? "");
            var trimParam = new SqlParameter("@model_trim", trim ?? "");
            var filterParam = new SqlParameter("@filter", filter ?? "");
            var pagingParam = new SqlParameter("@paging", paging ?? true);
            var pageParam = new SqlParameter("@page", page ?? 1);
            var perPageParam = new SqlParameter("@perPage", perPage ?? 50);
            var sortColumnParam = new SqlParameter("@sortColumn", sortColumn ?? "");
            var sortDirectionParam = new SqlParameter("@sortDirection", sortDirection ?? "asc");

            return await this.Database.SqlQuery<Car>(
                "FindCars @model_year, @make, @model_name, @model_trim, @filter, @paging, @page, @perPage, @sortColumn, @sortDirection", 
                yearParam, makeParam, modelParam, trimParam, filterParam, pagingParam, pageParam, perPageParam, sortColumnParam, sortDirectionParam).ToListAsync();
        }

        public async Task<List<Car>> Cars_by_Id(int? id) {
            var idParam = new SqlParameter("@Id", id ?? 1);
            return await this.Database.SqlQuery<Car>("Cars_by_Id @Id", idParam).ToListAsync();
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        //internal Task<object> FindCars(string year, string make, string model, string trim, string p1, bool p2, int p3, int p4) {
        //    throw new System.NotImplementedException();
        //}
    }
}