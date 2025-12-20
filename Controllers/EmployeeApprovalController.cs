using EmployeeMangementSystem.Data;
using EmployeeMangementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeMangementSystem.Controllers
{
    public class EmployeeApprovalController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public EmployeeApprovalController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {

            var apprisal = (from ea in _context.EmployeeAppraisal
                                      join e in _context.Employee
                                          on ea.EmpId equals e.EmpId
                                      join rOld in _context.Role
                                          on ea.OldRoleId equals rOld.RoleId
                                      join rNew in _context.Role
                                          on ea.NewRoleId equals rNew.RoleId
                                      select new EmployeeAppraisalViewModel
                                      {
                                          AppraisalId = ea.AppraisalId,
                                          EmpId = e.EmpId,
                                          F_Name = e.F_Name,
                                          L_Name = e.L_Name,
                                          OldRole = rOld.RoleType,
                                          NewRole = rNew.RoleType
                                      }).ToList();

            return Json(apprisal);

        }

        public IActionResult Edit(int id)
        {
            var appraisal = _context.EmployeeAppraisal.Find(id);
            if (appraisal == null) return NotFound();

            return View(appraisal); // create Edit.cshtml view
        }

        // POST: Edit action
        [HttpPost]
        public IActionResult Edit(EmployeeAppraisal model)
        {
            if (ModelState.IsValid)
            {
                _context.EmployeeAppraisal.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index"); // redirect to main table view
            }
            return View(model);
        }

        // POST: Delete action
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var appraisal = _context.EmployeeAppraisal.Find(id);
            if (appraisal == null) return NotFound();

            _context.EmployeeAppraisal.Remove(appraisal);
            _context.SaveChanges();
            return Ok();
        }
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
    }
}
