using EmployeeDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
using NPOI.HSSF.UserModel;

namespace EmployeeDapper.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employee
        private EmployeesRepository repository;
        /*private string connectionstring;*/
        /*new code*/


        public EmployeesController()
        {
            repository = new EmployeesRepository();
            /* connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;*/
        }

        // GET: Employee
        public ActionResult Index(RequestModel request,int ? i)
        {
            if (request.OrderBy == null)
            {
                request = new RequestModel
                {
                    Search = request.Search,
                    OrderBy = "name",
                    IsDescending = false
                };
            }
            ViewBag.Request = request;
            return View(repository.GetAll(request).ToList().ToPagedList(i?? 1,3));
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View(repository.Get(id));
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            var list = new List<string>() { "Thane", "Mulund", "Nahur", "Bhandup" };
            ViewBag.list = list;
            return View();

        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee, bool editAfterSaving = false)
        {
            if (ModelState.IsValid)
            {
                var lastInsertedId = repository.Create(employee);
                if (lastInsertedId > 0)
                {
                    TempData["Message"] = "Record added successfully";
                }
                else
                {
                    TempData["Error"] = "Failed to add record";
                }
                if (editAfterSaving)
                {
                    return RedirectToAction("Edit", new { Id = lastInsertedId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int Id)
        {
            var list = new List<string>() { "Thane", "Mulund", "Nahur", "Bhandup" };
            ViewBag.list = list;
            return View(repository.Get(Id));
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var recordAffected = repository.Update(employee);
                if (recordAffected > 0)
                {
                    TempData["Message"] = "Record updated successfully";
                }
                else
                {
                    TempData["Error"] = "Failed to update record";
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View(repository.Get(id));
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(Employee employee)
        {
            var recordAffected = repository.Delete(employee.Id);
            if (recordAffected > 0)
            {
                TempData["Message"] = "Record deleted successfully";
            }
            else
            {
                TempData["Error"] = "Failed to delete record";
            }
            return RedirectToAction("Index");
        }

        /* [HttpPost]
         public FileResult Export()
         {
             IDbConnection db = new SqlConnection(connectionstring);
              DataTable dt = new DataTable("grid");
             dt.Columns.AddRange(new DataColumn[3]
             {
                  new DataColumn("Name"),
                  new DataColumn("City"),
                  new DataColumn("Address"),
             });
             var emps = from emp in db.Employees.ToList() select emp;
             foreach(var emp in emps)
             {
                 dt.Rows.Add(emp.Name, emp.city, emp.Address);
             }
             using (XLWorkbook wb= new XLWorkbook())
             {
                 wb.Worksheets.Add(dt);
                 using (MemoryStream stream = new MemoryStream())
                 {
                     wb.SaveAs(stream);
                     return File(stream.ToArray(), "appliaction/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                 }
             }
          }
 */

        public ActionResult ExportToExcel(RequestModel request)
        {

            EmployeesRepository repository = new EmployeesRepository();
            var model = repository.GetAll(request);

            HSSFWorkbook templateWorkbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)templateWorkbook.CreateSheet("Index");
            List<Employee> _friend = model.ToList();
            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
            HSSFCellStyle style = (HSSFCellStyle)templateWorkbook.CreateCellStyle();

            HSSFFont font = (HSSFFont)templateWorkbook.CreateFont();
            font.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            HSSFCell cell;
            style.SetFont(font);

            cell = (HSSFCell)dataRow.CreateCell(0);
            cell.CellStyle = style;
            cell.SetCellValue("No.");

            cell = (HSSFCell)dataRow.CreateCell(1);
            cell.CellStyle = style;
            cell.SetCellValue("Name");
            cell = (HSSFCell)dataRow.CreateCell(2);
            cell.CellStyle = style;
            cell.SetCellValue("City");
            cell = (HSSFCell)dataRow.CreateCell(3);
            cell.CellStyle = style;
            cell.SetCellValue("Address");

            for (int i = 0; i < _friend.Count; i++)
            {
                dataRow = (HSSFRow)sheet.CreateRow(i + 1);
                dataRow.CreateCell(0).SetCellValue(i + 1);
                dataRow.CreateCell(1).SetCellValue(_friend[i].Name);
                dataRow.CreateCell(2).SetCellValue(_friend[i].City);
                dataRow.CreateCell(3).SetCellValue(_friend[i].Address);
            }

            MemoryStream ms = new MemoryStream();
            templateWorkbook.Write(ms);
            return File(ms.ToArray(), "application/vnd.ms-excel", "Employee.xls");
        }
    

        }
    }