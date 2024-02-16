using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectEHSA.Models
{
    public class EHSAModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public decimal HourlySalary { get; set; }
        public int TotalWorkHours { get; set; }
        public decimal TotalSalary { get; set; }
    }
}