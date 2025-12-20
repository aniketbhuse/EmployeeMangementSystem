namespace EmployeeMangementSystem.Models
{
    public class EmployeeAppraisalViewModel
    {
        public int AppraisalId { get; set; }
        public int EmpId { get; set; }       // Needed for Edit/Delete
        public string F_Name { get; set; }   // Matches LINQ query
        public string L_Name { get; set; }   // Matches LINQ query
        public string OldRole { get; set; }  // Matches LINQ query
        public string NewRole { get; set; }  // Matches LINQ query
    }
}
