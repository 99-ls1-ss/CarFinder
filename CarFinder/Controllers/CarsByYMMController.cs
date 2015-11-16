using CarFinder.Models;
using System.Data;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace CarFinder.Controllers {
    public class CarsByYMMController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments/Create
        public async Task<ActionResult> CarsByYMM(string year, string make, string model, string modelTrim) {

            ViewBag.year = new SelectList(await db.All_Years(year), year);
            ViewBag.make = new SelectList(await db.Cars_by_Year_Make(year, make), year, make);
            ViewBag.model = new SelectList(await db.Cars_by_Year_Make_Model(year, make, model), year, make, model);
            ViewBag.trim = new SelectList(await db.Cars_by_Year_Make_Model_Trim(year, make, model, modelTrim), year, make, model, modelTrim);

            var defaultYear = "Select a Year";
            var defaultMake = "Select a Make";
            var defaultModel = "Select a Model";
            var defaultTrim = "Select a Trim";

            var listYears = await db.Cars_by_Year(year);
            var listYearMake = await db.Cars_by_Year_Make(year, make);
            var listYearMakeModel = await db.Cars_by_Year_Make_Model(year, make, model);
            var listYearMakeModelTrim = await db.Cars_by_Year_Make_Model_Trim(year, make, model, modelTrim);


            if (year == "Select a Year" && make == "Select a Make" && model == "Select a Model") {
                return View(listYears);
            }
            else if (year != "Select a Year" && make == "Select a Make" && model == "Select a Model") {
                return View(listYearMake);
            }
            else if (year != "Select a Year" && make != "Select a Make" && model == "Select a Model") {
                return View(listYearMakeModel);
            }
            else if (year != "Select a Year" && make != "Select a Make" && model != "Select a Model" && model == "Select a Model") {
                return View(listYearMakeModelTrim);
            }

        }

    }
}