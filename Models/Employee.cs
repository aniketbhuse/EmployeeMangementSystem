using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMangementSystem.Models
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string F_Name { get; set; }
        public string L_Name { get; set; }
        public string Phone_Number { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public DateOnly JoinDate { get; set; }

       
    }
}



