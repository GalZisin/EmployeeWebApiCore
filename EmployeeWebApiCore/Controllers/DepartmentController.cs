using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EmployeeWebApiCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebApiCore.Controllers
{
    [Route("api/Department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Department> GetDepartmentList()
        {
            List<Department> dep = new List<Department>();
            Department department = null;
            SqlDAO sd = new SqlDAO();
            try
            {

                string query = "SELECT DepartmentId, DepartmentName FROM dbo.Department";
                //string query = @"
                //    select DepartmentId,DepartmentName from
                //    dbo.Department
                //    ";
                DataTable table = sd.GetSqlQueryDS(query, "Department").Tables[0];
                foreach (DataRow dr in table.Rows)
                {
                    department = new Department();
                    department.DepartmentId = int.Parse(dr["DepartmentId"].ToString());
                    department.DepartmentName = dr["DepartmentName"].ToString();
                    dep.Add(department);
                }
                return Ok(dep);
            }
            catch (Exception e1)
            {
                return BadRequest("something went wrong!");
            }

        }
        [HttpPost]
        public IActionResult AddDepartment(Department dep)
        {
            SqlDAO sd = new SqlDAO();
            string res = "";
            List<string> listResult = new List<string>();
            listResult.Add("Added Successfully!!");

            IActionResult result = null;
            try
            {
                string query = $"INSERT INTO dbo.Department VALUES('{dep.DepartmentName}')";
                res = sd.ExecuteSqlNonQuery(query);
                if (res != "")
                {
                    result = Ok(listResult);
                }
                else
                {
                    result = BadRequest("Failed to Add!!");
                }
            }
            catch (Exception e1)
            {
                result = BadRequest("Failed to Add!!");
            }
            return result;

        }
        [HttpPut]
        public IActionResult UpdateDepartment(Department dep)
        {
            SqlDAO sd = new SqlDAO();
            string res = "";
            List<string> listResult = new List<string>();
            listResult.Add("Updated Successfully!!");

            IActionResult result = null;
            try
            {
                string query = $"UPDATE dbo.Department SET DepartmentName='{dep.DepartmentName}' WHERE DepartmentId={dep.DepartmentId}";
                res = sd.ExecuteSqlNonQuery(query);
                if (res != "")
                {
                    result = Ok(listResult);
                }
                else
                {
                    result = BadRequest("Failed to Update!!");
                }
            }
            catch (Exception e1)
            {
                result = BadRequest("Failed to Update!!");
            }
            return result;

        }
    

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            SqlDAO sd = new SqlDAO();
           IActionResult result = null;
            List<string> listResult = new List<string>();
            listResult.Add("Deleted Successfully!!");
            try
            {
                string query = $"DELETE FROM dbo.Department WHERE DepartmentId={id}";
                string res = sd.ExecuteSqlNonQuery(query);
                if (res != "")
                {
                    result = Ok(listResult);
                }
                else
                {
                    result = BadRequest("Failed to Delete!!");
                }
            }
            catch (Exception e1)
            {
                result = BadRequest("Failed to Delete!!");
            }
            return result;
        }

    }
}
