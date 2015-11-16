using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarFinder.Models {
    public class CarsByYMM {

        public CarsByYMM() {
            this.Cars = new HashSet<Car>();
            //this.Comments = new HashSet<TicketCommentsModel>();
            //this.History = new HashSet<TicketHistoriesModel>();
            //this.Notification = new HashSet<TicketNotificationsModel>();
        }

        public int Id { get; set; }
        public string model_year { get; set; }
        public string make { get; set; }
        public string model_name { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

    }
}