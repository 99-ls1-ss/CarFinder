using CarFinder.Models;
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

    }
}