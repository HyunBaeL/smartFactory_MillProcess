using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFactory_MillProcess.Models
{
    public class Employee
    {
        public int EmployeeId {  get; set; }
        public string EmployeeCode { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime HireDate { get; set; }
        public string EmployeeStatus { get; set; }
        public bool SafetyEducation { get; set; }
    }
}
