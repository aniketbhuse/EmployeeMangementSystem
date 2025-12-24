using EmployeeMangementSystem.Data;
using EmployeeMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace EmployeeMangementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var role = _context.Role
                      .Select(r => new
                      {
                          r.RoleId,
                          r.RoleType
                      }).ToList();
            ViewBag.RoleList = new SelectList(role, "RoleId", "RoleType");
            var departments = _context.Department
                           .Select(d => new
                           {
                               d.DepartmentId,
                               d.DepartmentName
                           }).ToList();
            ViewBag.DepartmentList = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                var employee1 = new Employee
                {
                    F_Name = employee.F_Name,
                    L_Name = employee.L_Name,
                    Phone_Number = employee.Phone_Number,
                    Address = employee.Address,
                    RoleId = employee.RoleId,
                    DepartmentId = employee.DepartmentId,
                    JoinDate = employee.JoinDate
                };

                _context.Employee.Add(employee1);
                _context.SaveChanges();

                var apprisal = new EmployeeAppraisal
                {
                    EmpId = employee1.EmpId,
                    OldRoleId = employee1.RoleId,
                };
                _context.EmployeeAppraisal.Add(apprisal);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Invalid input. " + ex.Message });
            }
        }

       

        [HttpGet]
        public JsonResult GetEmployees()
        {
            var employees = (from e in _context.Employee
                             join r in _context.Role on e.RoleId equals r.RoleId
                             join d in _context.Department on e.DepartmentId equals d.DepartmentId
                             select new EmployeeViewModel
                             {
                                 EmpId = e.EmpId,
                                 F_Name = e.F_Name,
                                 L_Name = e.L_Name,
                                 Phone_Number = e.Phone_Number,
                                 Address = e.Address,
                                 RoleName = r.RoleType,
                                 DepartmentName = d.DepartmentName,
                                 JoinDate = e.JoinDate.ToDateTime(new TimeOnly(0, 0)) // convert DateOnly → DateTime
                             }).ToList();

            return Json(employees);
        }
        public IActionResult EditEmployee(int id)
        {
            var employee = _context.Employee.FirstOrDefault(e => e.EmpId == id);
            if (employee == null) return NotFound();

            return PartialView("_EditEmployeePartial", employee);
        }
        [HttpPost]

        [HttpPost]
        public IActionResult UpdateEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                var employee = _context.Employee.FirstOrDefault(e => e.EmpId == model.EmpId);
                if (employee == null) return NotFound();

                employee.F_Name = model.F_Name;
                employee.L_Name = model.L_Name;
                employee.Phone_Number = model.Phone_Number;
                employee.Address = model.Address;

                _context.SaveChanges();

                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation failed" });
        }
        public JsonResult DeleteEmployee(int id)
        {
            var employee = _context.Employee.FirstOrDefault(e => e.EmpId == id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Employee not found" });
        }

       

}
}
