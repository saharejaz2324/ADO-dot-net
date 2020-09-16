using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ADO_dot_net.DB;
using ADO_dot_net.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ADO_dot_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        Database db = new Database();

        [HttpPost]
        public IActionResult insertData(Inventory val)
        {
            string query = "INSERT INTO Inventory (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity);";
            var parameters = new IDataParameter[]
            {
               new SqlParameter("@Name", val.Name),
               new SqlParameter("@Price", val.Price),
               new SqlParameter("@Quantity", val.Quantity)
           };
            if (db.ExecuteData(query, parameters) > 0)
            {
                return Ok("Inserted");
            }
            else
            {
                return NotFound("Something went Wrong!!");
            }

        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
      {
            string query = "select * from Inventory";
            DataTable dt = db.GetData(query);
            var result = new ObjectResult(dt);
            return result;
        }
    }
}