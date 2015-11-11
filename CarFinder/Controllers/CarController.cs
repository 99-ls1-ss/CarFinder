using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarFinder.Controllers {
    [RoutePrefix("api/Cars")]
    public class CarController : ApiController {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("AllYears")]
        [HttpGet]
        public async Task<List<string>> All_Years(string year) {
            return await db.All_Years(year);
        }

        [Route("MakeByYear")]
        [HttpGet]
        public async Task<List<string>> Make_by_Year(string year) {
            return await db.Make_by_Year(year);
        }

        [Route("ModelByYearMake")]
        [HttpGet]
        public async Task<List<string>> Model_by_Year_Make(string year, string make) {
            return await db.Model_by_Year_Make(year, make);
        }

        [Route("TrimsByYearMakeModel")]
        [HttpGet]
        public async Task<List<string>> Trims_by_Year_Make_Model(string year, string make, string model) {
            return await db.Trims_by_Year_Make_Model(year, make, model);
        }

        [Route("CarByYear")]
        [HttpGet]
        public async Task<List<Car>> Cars_by_Year(string year) {
            return await db.Cars_by_Year(year);
        }

        [Route("CarsByYearMake")]
        [HttpGet]
        public async Task<List<Car>> Cars_by_Year_Make(string year, string make) {
            return await db.Cars_by_Year_Make(year, make);
        }

        [Route("CarsByYearMakeModel")]
        [HttpGet]
        public async Task<List<Car>> Cars_by_Year_Make_Model(string year, string make, string model) {
            return await db.Cars_by_Year_Make_Model(year, make, model);
        }

        [Route("CarsByYearMakeModelTrim")]
        [HttpGet]
        public async Task<List<Car>> Cars_by_Year_Make_Model_Trim(string year, string make, string model, string trim) {
            return await db.Cars_by_Year_Make_Model_Trim(year, make, model, trim);
        }

        [Route("CarById")]
        [HttpGet]
        public async Task<List<Car>> Cars_by_Id(string id) {
            return await db.Cars_by_Id(id);
        }
    }
}
