using Bing;
using CarFinder.Models;
using Newtonsoft.Json;
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
        public async Task<IHttpActionResult> Cars_by_Id(int id) {

            var car = db.Car.Find(id);

            if(car==null){
                return await Task.FromResult(NotFound());
            }

            var client = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));
            client.Credentials = new NetworkCredential("accountKey", "o+nYHPyZpmhku+bXtDn0AFRZ79Jnxd4KS/QkoHd3B3E");
            var marketData = client.Composite(
                "image",
                car.model_year + car.make + car.model_name + car.model_trim,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                ).Execute();

            var result = marketData.FirstOrDefault();
            var image = result != null ? result.Image : null;
            var firstImage = image != null ? image.FirstOrDefault() : null;
            var imageUrl = firstImage != null ? firstImage.MediaUrl : "~/Images/img_not_found.jpg";

            dynamic recalls;

            using (var httpClient = new HttpClient()) {
                httpClient.BaseAddress = new Uri("http://www.nhtsa.gov/");

                try {
                    var response = await httpClient.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + car.model_year + "/make/" + car.make + "/model/" + car.model_name + "?format=json");
                    recalls = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                }
                catch(Exception e) {
                    return InternalServerError(e);
                }
            }

            return Ok(new { car, imageUrl, recalls });
        }
    }
}
