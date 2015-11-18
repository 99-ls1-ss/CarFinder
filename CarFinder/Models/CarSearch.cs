using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarFinder.Models {
    public class CarSearch {

        public Car Car { get; set; }
        public dynamic Recalls { get; set; }
        public string ImageUrl { get; set; }
    }
}