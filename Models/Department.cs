using EmployeeMangementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMangementSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}


 
