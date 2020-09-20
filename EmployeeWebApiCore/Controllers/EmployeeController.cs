using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeWebApiCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebApiCore.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public ActionResult<Employee> GetEmployeeList()
        {
            List<Employee> el = new List<Employee>();
            Employee em = null;
            SqlDAO sd = new SqlDAO();
            try
            {
                //string query = @"
                //    select EmployeeId,EmployeeName,Department,
                //    convert(varchar(10),DateOfJoining,120) as DateOfJoining,
                //    PhotoFileName
                //    from
                //    dbo.Employee
                //    ";
                string query = "SELECT EmployeeId, EmployeeName, Department, convert(varchar(10),DateOfJoining,120) as DateOfJoining, PhotoFileName FROM dbo.Employee";
                DataTable table = sd.GetSqlQueryDS(query, "Employee").Tables[0];
                foreach (DataRow dr in table.Rows)
                {
                    em = new Employee();
                    em.EmployeeId = int.Parse(dr["EmployeeId"].ToString());
                    em.EmployeeName = dr["EmployeeName"].ToString();
                    em.Department = dr["Department"].ToString();
                    em.DateOfJoining = dr["DateOfJoining"].ToString();
                    em.PhotoFileName = dr["PhotoFileName"].ToString();

                    el.Add(em);
                }
                return Ok(el);
            }
            catch (Exception e1)
            {
                return BadRequest("something went wrong!");
            }

        }
        [HttpPost]
        public IActionResult AddEmployee(Employee emp)
        {
            SqlDAO sd = new SqlDAO();
            string res = "";
            List<string> listResult = new List<string>();
            listResult.Add("Added Successfully!!");

            IActionResult result = null;
            try
            {
                string query = $"INSERT INTO dbo.Employee VALUES('{emp.EmployeeName}', '{emp.Department}', '{emp.DateOfJoining}', '{emp.PhotoFileName}')";
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
        public IActionResult UpdateEmployee(Employee emp)
        {
            SqlDAO sd = new SqlDAO();
            string res = "";
            List<string> listResult = new List<string>();
            listResult.Add("Updated Successfully!!");

            IActionResult result = null;
            try
            {
                string query = $"UPDATE dbo.Employee SET EmployeeName='{emp.EmployeeName}', Department='{emp.Department}'," +
                    $" DateOfJoining='{emp.DateOfJoining}', PhotoFileName='{emp.PhotoFileName}' WHERE EmployeeId={emp.EmployeeId}";
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
        public IActionResult DeleteEmployee(int id)
        {
            SqlDAO sd = new SqlDAO();
            IActionResult result = null;
            List<string> listResult = new List<string>();
            listResult.Add("Deleted Successfully!!");
            try
            {
                string query = $"DELETE FROM dbo.Employee WHERE EmployeeId={id}";
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
        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public ActionResult<Department> GetAllDepartmentNames()
        {
            SqlDAO sd = new SqlDAO();
            Department dep = null;
            List<Department> depNames = new List<Department>();
            try
            {
                string query = "SELECT DepartmentName FROM dbo.Department";
                DataTable table = sd.GetSqlQueryDS(query, "Department").Tables[0];
                foreach (DataRow dr in table.Rows)
                {
                    dep = new Department();
                    dep.DepartmentName = dr["DepartmentName"].ToString();

                    depNames.Add(dep);
                }
                return Ok(depNames);
            }
            catch
            {
                return BadRequest("something went wrong!");
            }

        }

        [Route("SaveFile")]
        [HttpPost]
        //public string SaveFile([FromForm] FileUpload objectFile)
        public IActionResult SaveFile()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file != null)
                {
                    string path = _webHostEnvironment.ContentRootPath + "\\Photos\\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var stream = new FileStream(path + file.FileName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return new JsonResult(file.FileName);
                    //using(FileStream fileStream = System.IO.File.Create(path + file.FileName))
                    //{
                    //    file.CopyTo(fileStream);
                    //    fileStream.Flush();
                    //    return "Uploaded";
                    //}
                }
                else
                {
                    return new JsonResult("anonymous.PNG");
                }
            }
            catch (Exception ex1)
            {
                return new JsonResult("anonymous.PNG");
            }

        }
    }
}
