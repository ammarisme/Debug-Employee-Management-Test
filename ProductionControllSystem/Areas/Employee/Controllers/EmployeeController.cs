    using System.Collections.Generic;
using System.Web.Mvc;
using ClassLibrary.Core;
using FrontEnd.Areas.Employees.Models;
using FrontEnd.Controllers;
using System;
using System.Linq;

namespace FrontEnd.Areas.Employees.Controllers
{
    public class EmployeeController : Controller
    {
        /*
         * Employee types : 1 (normal), 2 (redistill), 3 (double distill)
         */
        Database db = new Database();
        public EmployeeController()
        {

        }

        public ActionResult Add()
        {
            return View();
        }
    }
}