using ClassLibrary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FrontEnd.Areas.Accounts.Models
{ 
    public class DashboardViewModel : Employee
    {
        public List<Employee> employees;
    }
}
