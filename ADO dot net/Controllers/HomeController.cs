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
        //[HttpGet("{tableName}")]
        //public IActionResult getDataFromSpecificTable(string tableName, object obj)
        //{

        //}

        // update the data into the database through SQL Query

        [HttpPut("{id}")]
        public IActionResult ModifyData(int id, Inventory inventory)
        {
            string query = "UPDATE Inventory SET Name = @Name, Price = @Price , Quantity = @quantity WHERE id = " + id;
            var parameters = new IDataParameter[]
            {
               new SqlParameter("@Name", inventory.Name),
               new SqlParameter("@Price", inventory.Price),
               new SqlParameter("@Quantity", inventory.Quantity)
            };

            if (db.ExecuteData(query, parameters) > 0)
            {
                return Ok("Updated");
            }
            else
            {
                return NotFound("Something went Wrong!!");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteData(int id)
        {
            string query = "DELETE FROM Inventory WHERE id = " + id;
            if (db.DeleteData(query) > 0)
            {
                return Ok("Deleted Record Successfully");
            }
            else
            {
                return NotFound("Something went Wrong!!");
            }
        }

        [HttpPost("createTable/{tableName}")]
        public IActionResult CreateTable(string tableName)
        {
            string query = "CREATE TABLE " + tableName + " ( id int identity(1,1) NOT NULL PRIMARY KEY, name VARCHAR(255) NOT NULL)";
            if (db.CreateTable(query))
            {
                return Ok("Created");
            }
            else
            {
                return NotFound("Table existing in the database");
            }
        }

        [HttpPost("alterTable/{tableName}")]
        public IActionResult alterTable(string tableName, [FromBody]AlterTable columnContent)
        {
            string query = "ALTER TABLE " + tableName + " ADD " + columnContent.columnName + " " + columnContent.columnType;
            if (db.AddColumn(query))
            {
                return Ok("Added");
            }
            else
            {
                return NotFound("Already Exist");
            }
        }


        //Stored Procedure 

        [HttpGet("getAllSP")]
        public async Task<List<Inventory>> GetAllThroughSP()
        {
            return await db.getAll();
        }

    }
}