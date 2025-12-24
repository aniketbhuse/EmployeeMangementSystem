namespace EmployeeMangementSystem.Models
{
    public class EmployeeAppraisal
    {
        public int AppraisalId { get; set; }
        public int EmpId { get; set; }
        public int OldRoleId { get; set; }
        public int NewRoleId { get; set; }
    }
}


