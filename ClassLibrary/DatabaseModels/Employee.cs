using ClassLibrary.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary.Models
{
    [FileName("Employee")]
    public class Employee
    {
        [DBField("EmployeeNo")]
        public string EmployeeNo { get; set; }
        [DBField("EmployeeName")]
        public string EmployeeName { get; set; }
        [DBField("Department")]
        public string Department { get; set; }
        [DBField("Email")]
        public string Email { get; set; }
        [DBField("Address")]
        public string Address { get; set; }
        [DBField("ContactNo")]
        public string ContactNo { get; set; }
    }
}