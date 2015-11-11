using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarFinder.Controllers {
    public class TestController : ApiController {

        private ApplicationDbContext db = new ApplicationDbContext();
        private SqlConnection conn = new SqlConnection();

        [Route("api/GetEmail/{email}")]
        public IHttpActionResult GetEmail(string email) {
            email = "email1@email.com";
            return Ok(email);
        }

        [Route("api/GetListOfStrings")]
        public IHttpActionResult GetListOfStrings() {
            
            return Ok(new string[] {"a", "b", "cat", "dog"});
        }

        [Route("api/GetListOfNames")]
        public IHttpActionResult GetListOfNames() {

            return Ok(new string[] { "Brandon", "Jimmy" });
        }

        //[Route("api/GetListOfYears")]
        //public IHttpActionResult GetListOfYears() {

        //    using (var conn = new SqlConnection())
        //    using (var command = new SqlCommand("Make_by_Year", conn) {
        //        CommandType = CommandType.StoredProcedure                
        //    }) {
        //        command.Parameters.Add("@model_year");
        //        conn.Open();
        //        command.ExecuteNonQuery();
        //        conn.Close();
        //    }
        //    return Ok();
        //}
    }
}
