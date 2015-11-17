using Bing;
using CarFinder.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarFinder.Controllers {
    public class CarSearchController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarSearch/CarSearch
        public async Task<ActionResult> Index(string year, string make, string model, string trim) {

            if (string.IsNullOrWhiteSpace(year)) {
                year = "2015";
                ViewBag.make = null;
                ViewBag.model = null;
                ViewBag.trim = null;
            }
            else {
                ViewBag.make = new SelectList(await db.Make_by_Year(year),year);
            }
            if (!string.IsNullOrWhiteSpace(make)) {
                ViewBag.model = new SelectList(await db.Model_by_Year_Make(year,make), make);
            }
            if (!string.IsNullOrWhiteSpace(model)) {
                ViewBag.trim = new SelectList(await db.Trims_by_Year_Make_Model(year, make, model),model);
            }

            ViewBag.year = new SelectList(await db.All_Years(year));
            var cars = await db.FindCars(year, make, model, trim, null, null, null, null, null, null);
            return View(cars);            
        }

        public async Task<ActionResult> Details(int id) {

            //Car carStuff = await db.Car.FindAsync(id);
            //ApplicationUser recalls = await db.Users.

            //return View(carStuff);

            var car = db.Car.Find(id);

            //if (car == null) {
            //    return await Task.FromResult(NotFound());
            //}

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

                //try {
                    var response = await httpClient.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + car.model_year + "/make/" + car.make + "/model/" + car.model_name + "?format=json");
                    recalls = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                //}
                //catch (Exception e) {
                //    return InternalServerError(e);
                //}
            }

            return View(new { car, imageUrl, recalls });
        }


    }
}