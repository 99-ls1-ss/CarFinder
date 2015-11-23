using Bing;
using CarFinder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Web.Http;
using DataTables.Mvc;

namespace CarFinder.Controllers {
    public class CarSearch2Controller : Controller {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarSearch/CarSearch
        public async Task<ActionResult> Index(string year, string make, string model, string trim) {
            ViewBag.make = new SelectList(new List<string>());
            ViewBag.model = new SelectList(new List<string>());
            ViewBag.trim = new SelectList(new List<string>());
            if (string.IsNullOrWhiteSpace(year)) {
                year = "2015";                
            }
            else {
                ViewBag.make = new SelectList(await db.Make_by_Year(year), year);
            }
            if (!string.IsNullOrWhiteSpace(make)) {
                ViewBag.model = new SelectList(await db.Model_by_Year_Make(year, make), make);
            }
            if (!string.IsNullOrWhiteSpace(model)) {
                ViewBag.trim = new SelectList(await db.Trims_by_Year_Make_Model(year, make, model), model);
            }

            ViewBag.year = new SelectList(await db.All_Years(year));
            var cars = await db.FindCars(year, make, model, trim, null, null, null, null, null, null);
            return View(cars);
        }

        public async Task<ActionResult> Make_by_Year(string year) {
            var make = new SelectList(await db.Make_by_Year(year));
            return Content(JsonConvert.SerializeObject(make), "application/json");
        }

        public async Task<ActionResult> Model_by_Year_Make(string year, string make) {
            var model = new SelectList(await db.Model_by_Year_Make(year,make));
            return Content(JsonConvert.SerializeObject(model), "application/json");
        }

        public async Task<ActionResult> Trims_by_Year_Make_Model(string year, string make, string model) {
            var trim = new SelectList(await db.Trims_by_Year_Make_Model(year, make, model));
            return Content(JsonConvert.SerializeObject(trim), "application/json");
        }
        
        public async Task<JsonResult> GetCars([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest request,
           string year = "2015", string make = "", string model = "", string trim = "") {

            var filter = request.Search.Value;
            var totalCount =  await db.Count_Cars_Year_Make_Model_Trim_Filter(year, make, model, trim, filter);
            var column = request.Columns.FirstOrDefault(r => r.IsOrdered == true);
            var sortColumn = "";
            var sortDirection = "asc";
            if (column != null) {
                sortColumn = column.Data;
                if (column.SortDirection == Column.OrderDirection.Descendant) {
                    sortDirection = "Desc";
                }
            }
            List<Car> cars = new List<Car>();
            cars = await db.FindCars(year, make, model, trim, filter, true, ((request.Start / request.Length) + 1), request.Length, sortColumn, sortDirection);

            var paged = cars.Select(c =>
            new Car {
                id = + c.id,
                make = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.make + "</a>",
                model_name = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.model_name + "</a>",
                model_trim = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.model_trim + "</a>",
                model_year = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.model_year + "</a>",
                body_style = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.body_style + "</a>",
                engine_num_cyl = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.engine_num_cyl + "</a>",
                engine_power_ps = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.engine_power_ps + "</a>",
                drive_type = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.drive_type + "</a>",
                seats = "<a href=\"/CarSearch/Details/" + c.id + "\">" + c.seats + "</a>"
            });
            return Json(new DataTablesResponse(request.Draw, paged,request.Length ,Convert.ToInt32(totalCount)),
               JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(int id) {

            CarSearch car = new CarSearch();
            car.Car = db.Car.Find(id);


            var client = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));
            client.Credentials = new NetworkCredential("accountKey", "o+nYHPyZpmhku+bXtDn0AFRZ79Jnxd4KS/QkoHd3B3E");
            var marketData = client.Composite(
                "image",
                car.Car.model_year + car.Car.make + car.Car.model_name + car.Car.model_trim,
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
            car.ImageUrl = firstImage != null ? firstImage.MediaUrl : "/Images/img_not_found.jpg";

            //dynamic recalls;

            using (var httpClient = new HttpClient()) {
                httpClient.BaseAddress = new Uri("http://www.nhtsa.gov/");

                //try {
                var response = await httpClient.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + car.Car.model_year + "/make/" + car.Car.make + "/model/" + car.Car.model_name + "?format=json");
                car.Recalls = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                //}
                //catch (Exception e) {
                //    return InternalServerError(e);
                //}
            }

            return View(car);
        }
    }
}
