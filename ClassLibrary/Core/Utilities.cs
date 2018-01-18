using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Linq.Expressions;
using System.IO;

namespace ClassLibrary.Core
{
    /// <summary>
    /// Containing all the methods that do database operations
    /// </summary>
    public class Utilities
    {
        public void Logger(string ex)
        {
            try
            {
                string PATH = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "DatabaseOperations.txt");
                using (StreamWriter writer = new StreamWriter(PATH, true))
                {
                    writer.WriteLine("\n " + ex);
                    writer.Close();
                }
            }
            catch (Exception exception)
            {
            }
        }   
    }
    }
