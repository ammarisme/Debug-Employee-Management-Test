using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using ClassLibrary.Core;
using System.Globalization;
using System.IO;
using System.Configuration;
using System.Reflection;
using ClassLibrary.Core.Attributes;

namespace SFA_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service : IService
    {
        public delegate object operationOnObject(string file, object obj);
        public delegate object databaseOperation();
        public List<databaseOperation> operations;
        private string secretKey = System.Configuration.ConfigurationSettings.AppSettings["SecretKey"].ToString();
        QueryProviderFactory qFactory = new QueryProviderFactory();
        public Service()
        {
        }
        public Response SelectAll(string fileName)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");

            DatabaseAccess dbAccess = new DatabaseAccess();

            Response response = new Response();
            List<Object> list = new List<Object>();
            response.Data = javaScriptSerializer.Serialize(queryProvider.SelectAll(fileName));
            response.ID = 200;

            return response;
        }

        public Response Insert(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(queryProvider.Insert(fileName, obj));
            
            Logger("INSERT " + fileName + " - " + response.Data);
            return response;

        }
        public Response InsertDefinition(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(queryProvider.InsertDefinition(fileName, obj));
            return response;

        }
        public Response Update(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(queryProvider.Update(fileName, obj));
            Logger("UPDATE :" + fileName + " - " + response.Data);
            return response;
        }
        public Response UpdateAllFields(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(queryProvider.UpdateAllFields(fileName, obj));
            Logger("UPDATE ALL : " + fileName + " - " + response.Data);
            return response;
        }

        public Response DeleteAll()
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();

            response.Data = javaScriptSerializer.Serialize(queryProvider.DeleteAll(fileName));
            return response;
        }

        public Response Delete(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(queryProvider.Delete(fileName, obj));
            return response;

        }

        public Response Select(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.ID = 200;
            List<Object> list = new List<Object>();
            response.Data = javaScriptSerializer.Serialize(queryProvider.Select(fileName, obj));

            return response;
        }

        public Response Find(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.ID = 200;
            List<Object> list = queryProvider.Select(fileName, obj);
            obj = list.LastOrDefault<Object>();
            response.Data = javaScriptSerializer.Serialize(obj);

            return response;
        }

        public Response SelectLast(string dataObject)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;

            string fileName = woc.Headers["PhysicalFileName"];
            string key = woc.Headers["SecretKey"];
            if (key != secretKey)
            {
                return new Response { Data = "Action Not authorized", ID = 500 };
            }
            Response response = new Response();
            Object obj = new Object();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            iQueryProvider queryProvider = qFactory.GetQueryProvider("mssql");
            DatabaseAccess dbAccess = new DatabaseAccess();
            // do a set of database operations

            obj = dbAccess.DeserializeToDomain(fileName, dataObject);
            response.Data = javaScriptSerializer.Serialize(queryProvider.SelectLast(fileName, obj));

            response.ID = 200;
            List<Object> list = new List<Object>();

            return response;
        }

        public Response CompareQaAndLiveDatabases()
        {
            return new Response { Data = "", ID = 200 };
        }

        public Response Cutover()
        {
            string eshaContainers = "HS1,26,0,0,0|HS2,33,1527,2227.1,2175.7|HS3,25,51673,64655.9,62069.7|HS4,26,0,0,0|HS5,33,1454,1839.3,1827.9|HS6,25,49495,61925.4,59448.4|HS7,24,37791,47386.3,45680.4|HS8,26,0,0,0|";
            eshaContainers += "CS9,9,7275,8356.3,6083.4|CS10,9,287,330.3,245.4|EXTRA,10,46.7,51.7,30|BV1,22,33500,0,12905.8|BV2,22,33500,0,12905.3|BV3,22,0,0,0|BV4,22,0,0,0|";
            eshaContainers += "AV1,35,0,0,0|AV2,35,59257,62489.3,20996.4|AV3,35,71786,75675.8,25427.1|AV4,35,0,0,0|AV5,35,0,0,0|AV6,22,20411,21630.1,7267.7|AV7,22,71792,75682.1,25429.2|AV8,22,71800,75706.5,25437.4|AV9,22,71550,75426.9,25343.4|AV10,22,71292,75154.9,25252|AV11,22,70648,74477.7,25024.5|AV12,22,750,790.6,265.6|AV13,22,71954,75852.8,25486.5|AV14,22,750,790.6,265.6|AV15,22,71760,75648.4,25417.9|AV16,22,0,0,0|AV17,22,35696,37582.6,12627.8|AV18,22,0,0,0";

            //string[] seeduwaArray = seeduwaContainers.Split('|');
            string[] eshaArray = eshaContainers.Split('|');
            string results = "";

            //foreach (var item in seeduwaArray)
            //{
            //    string[] container = item.Split(',');
            //    results += CutoverSeeduwa(container[0], decimal.Parse(container[1]), decimal.Parse(container[2]), decimal.Parse(container[3]), decimal.Parse(container[4]), decimal.Parse(container[5]), decimal.Parse(container[6]));
            //}
            foreach (var item in eshaArray)
            {
                string[] container = item.Split(',');
                results += CutoverEsha(container[0], container[1], decimal.Parse(container[2]), decimal.Parse(container[3]), decimal.Parse(container[4]));
            }
            return new Response { Data = results, ID = 200 };
        }

        //private string CutoverSeeduwa(string containerName, decimal currentDip, decimal bulkVolume, decimal temperature, decimal density, decimal strength, decimal pureVolume)
        //{
        //    string returnMessage = "";
        //    DBConnection dbConnection = new DBConnection();
        //    try
        //    {
        //        dbConnection.cmd.CommandText = "UPDATE MFGTEST.CONTAIN SET FILPWEI=0, FILWEI=" + GetRounded((bulkVolume * density) / 1000) + " , CURDEP=" + currentDip + " ,PUREVOL=" + GetRounded(pureVolume) + " , FILVOL=" + GetRounded(bulkVolume) + ", DENST=" + density + ",TEMPRT=" + temperature + " , STRENG =" + strength + "  WHERE CONAME ='" + containerName + "'";
        //        returnMessage += (" | " + dbConnection.cmd.ExecuteNonQuery());
        //        dbConnection.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //        throw;
        //    }
        //    return returnMessage;
        //}

        private string CutoverEsha(string conname, string liquidTypeId,  decimal curWaight, decimal bulkvol, decimal purevol )
        {
            DBConnection dbConnection = new DBConnection();
            String returnmessage = "";
            try
            {
                decimal density = curWaight != 0 && bulkvol != 0 ? GetRounded((curWaight / bulkvol) * 1000) : 0;
                decimal strength = purevol != 0 && bulkvol != 0 ? GetRounded((purevol / bulkvol) * 100) : 0;

                if (dbConnection.con.State == System.Data.ConnectionState.Closed){
                    dbConnection.con.Open();
                    dbConnection.tr = dbConnection.con.BeginTransaction();
                    dbConnection.cmd.Transaction = dbConnection.tr;
                }

                dbConnection.cmd.CommandText = "UPDATE DCSLMFG.CONTAIN SET FILWEI=" + curWaight + ", PUREVOL=" + purevol + " , FILVOL=" + bulkvol + " , LIQTYPE='" + liquidTypeId + "', STRENG=" + strength + " , DENST=" + density + " , SOLUID=''  where CONAME ='" + conname + "' AND LOCID='ES_PC'";
                returnmessage += (" | " + dbConnection.cmd.ExecuteNonQuery()+" - " + dbConnection.cmd.CommandText+"\n");

                dbConnection.cmd.CommandText = "UPDATE DCSLMFG.CONTAINER_CUT SET FILWEI=" + curWaight + ", PUREVOL=" + purevol + " , FILVOL=" + bulkvol + " , LIQTYPE='" + liquidTypeId + "', STRENG=" + strength + " , DENST=" + density + " , SOLUID=''  where CONAME ='" + conname + "' AND LOCID='ES_PC'";
                returnmessage += "CUT " +dbConnection.cmd.ExecuteNonQuery();
                dbConnection.Commit();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return returnmessage;
        }

        public decimal GetRounded(decimal value)
        {
            return Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public static void Logger(string ex)
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