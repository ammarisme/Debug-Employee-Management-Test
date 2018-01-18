using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Linq.Expressions;
using System.IO;
using ClassLibrary.Models;

namespace ClassLibrary.Core
{
    public class DatabaseAccess
    {
        private JavaScriptSerializer javaScriptSerializer;
        public DatabaseAccess()
        {
            javaScriptSerializer = new JavaScriptSerializer();
        }
        public Object DeserializeToDomain(string fileName, string dataObject)
        {
            Object obj = new Object();
            switch (fileName)
            {
                case "Employee":
                    obj = (Object)javaScriptSerializer.Deserialize<Employee>(dataObject);
                    break;
                default:
                    break;
            }

            return obj;
        }

        public object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return Activator.CreateInstance(t);
        }
    }
}