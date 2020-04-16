using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReturnRequestController : ControllerBase
    {
        public AppDb Db { get; }

        public ReturnRequestController(AppDb db)
        {
            Db = db;
        }

        [HttpPost]
        public IActionResult PostReturnRequest([FromBody]ReturnRequestModel request)
        {
            Db.Connection.Open();
            request.Db = Db;
            string result = null;
            try{
                result = request.AddReturnRequest();
            }
            catch(NullReferenceException){
                return NoContent();
            }
            Db.Connection.Close();
            return Ok(result);
        }
        
        [HttpPut]
        [Route("reject")]
        public IActionResult PutReturnRequest([FromBody]ReturnRequestModel request)
        {
            Db.Connection.Open();
            request.Db = Db;
            string result = null;
            try{
                result = request.RejectReturnRequest();
            }
            catch(NullReferenceException){
                return NoContent();
            }
            Db.Connection.Close();
            return Ok(result);
        }


        [HttpGet]
        public IActionResult GetReturnRequest()
        {
            int userId=-1;
            string searchField = "";
            string sortField = "";
            string sortDirection = "asc";
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                userId = Convert.ToInt32(HttpContext.Request.Query["id"]);

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["search"]))
                searchField = HttpContext.Request.Query["search"];
            
            
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["sort"]))
                sortField = HttpContext.Request.Query["sort"];

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["direction"]))
                sortDirection = HttpContext.Request.Query["direction"];

            Db.Connection.Open();
            var returnObject = new ReturnRequestModel(Db);
            var result = returnObject.GetReturnRequests(userId,sortField,sortDirection,searchField);
            Db.Connection.Close();
            return new OkObjectResult(result);
        }
    }

}