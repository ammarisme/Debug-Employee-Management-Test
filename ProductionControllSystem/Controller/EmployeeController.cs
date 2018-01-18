using ClassLibrary.Core;
using ClassLibrary.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FrontEnd.Controllers
{
    public class EmployeeController : ApiController
    {
        Database db = new Database();

        [HttpPost]
        [Route("api/Employee/Add")]
        public HttpResponseMessage Add(Employee employee)
        {
            db.Insert<Employee>(employee);
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }
    }
}