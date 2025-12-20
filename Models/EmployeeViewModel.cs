namespace EmployeeMangementSystem.Models
{
    public class EmployeeViewModel
    {
        public int EmpId { get; set; }
        public string F_Name { get; set; }
        public string L_Name { get; set; }
        public string Phone_Number { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }       // From RoleType table
        public string DepartmentName { get; set; } // From Department table
        public DateTime JoinDate { get; set; }
    }
}
