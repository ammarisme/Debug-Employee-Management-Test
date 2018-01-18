using ClassLibrary.Core; using ClassLibrary.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace ClassLibrary.Core
{
    public class Database
    {
        private string serviceUrl;
        JavaScriptSerializer javaScriptSerializer;
        WebRequest request;
        WebResponse ws;
        Response response;
        DataContractJsonSerializer jsonSerializer;
        private string secretKey ="";
        public Database()
        {
            javaScriptSerializer = new JavaScriptSerializer();
            jsonSerializer = new DataContractJsonSerializer(typeof(Response));
            secretKey = System.Configuration.ConfigurationSettings.AppSettings["SecretKey"].ToString();

            serviceUrl = ConfigurationSettings.AppSettings["DeveloperWebServiceURL"].ToString() + "/";
        }
        public string GetAttribute(Type t)
        {
            // Get instance of the attribute.
            FileNameAttribute MyAttribute =
                (FileNameAttribute)Attribute.GetCustomAttribute(t, typeof(FileNameAttribute));

            if (MyAttribute == null)
            {
                Console.WriteLine("The attribute was not found.");
                return "";
            }
            else
            {
                // Get the Name value.
                return MyAttribute.FileName;
            }
        }
        public Response Insert<T>(Object domainObject)
        {
            // arrange the sales header
            T ob = (T)domainObject;

            string fileName = GetAttribute(typeof(T));
            foreach (var propertyInfo in ob.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(ob, null) == null)
                    {
                        propertyInfo.SetValue(ob, @"", null);
                    }
                }
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            ser.WriteObject(mem, javaScriptSerializer.Serialize(ob));
            string data = System.Text.Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
            
            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/json";
            webClient.Headers["PhysicalFileName"] = fileName;
            webClient.Headers["SecretKey"] = secretKey;
            webClient.Encoding = System.Text.Encoding.UTF8;
            // send the object to the web service
            Response response = new Response();
            try {
            string responseStream = webClient.UploadString(serviceUrl + "Insert", "POST", data);
            response = javaScriptSerializer.Deserialize<Response>(responseStream);
            }catch(Exception ex){
                LogError("Insert filename="+fileName+" "+ex.Message);
            }
            
            return response;
        }
        public Response UpdateAllFields<T>(Object domainObject)
        {
            // arrange the sales header
            T ob = (T)domainObject;

            string fileName = GetAttribute(typeof(T));
            foreach (var propertyInfo in ob.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(ob, null) == null)
                    {
                        propertyInfo.SetValue(ob, @"", null);
                    }
                }
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            ser.WriteObject(mem, javaScriptSerializer.Serialize(ob));
            string data = System.Text.Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/json";
            webClient.Headers["PhysicalFileName"] = fileName;
            webClient.Headers["SecretKey"] = secretKey;


            webClient.Encoding = System.Text.Encoding.UTF8;
            // send the object to the web service
            try
            {
                string responseStream = webClient.UploadString(serviceUrl + "UpdateAllFields", "POST", data);
                Response response = javaScriptSerializer.Deserialize<Response>(responseStream);
            }catch(Exception ex){
                LogError("File name :"+fileName+" "+ex.Message);
            }

            return response;
        }
        public Response Update<T>(Object domainObject)
        {
            // arrange the sales header
            T ob = (T)domainObject;

            string fileName = GetAttribute(typeof(T));
            foreach (var propertyInfo in ob.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(ob, null) == null)
                    {
                        propertyInfo.SetValue(ob, @"", null);
                    }
                }
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            ser.WriteObject(mem, javaScriptSerializer.Serialize(ob));
            string data = System.Text.Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
            
            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/json";
            webClient.Headers["PhysicalFileName"] = fileName;
            webClient.Headers["SecretKey"] = secretKey;
            webClient.Encoding = System.Text.Encoding.UTF8;
            // send the object to the web service
            try
            {
                string responseStream = webClient.UploadString(serviceUrl + "Update", "POST", data);
                Response response = javaScriptSerializer.Deserialize<Response>(responseStream);
            }
            catch (Exception ex)
            {
                LogError("File Name : "+fileName + " "+ex.Message);
            }

            return response;
        }
        public List<T> Get<T>(Object obj) where T : class
        {
            Response response = GetResponse<T>(obj, "Select");
            List<T> list = response.Data!=null ?javaScriptSerializer.Deserialize<List<T>>(response.Data) : new List<T>();
            return list;
        }
        public T Find<T>(Object obj) where T : class
        {
            Response response = GetResponse<T>(obj, "Find");
            T item= response != null && response.Data != null ? javaScriptSerializer.Deserialize<T>(response.Data) : default(T);
            return item;
        }
        public T FindLast<T>(Object obj) where T : class
        {
            List<T> list = Get<T>(obj);
            return list.LastOrDefault();
        }
        public T FindFirst<T>(Object obj) where T : class
        {
            List<T> list = Get<T>(obj);
            return list.FirstOrDefault();
        }
        public List<T> GetAll<T>() where T : class
        {
            string fileName = GetAttribute(typeof(T));
            string url = serviceUrl + "SelectAll?fileName="+fileName;
            List<T> tempList = new List<T>();
            List<T> result = new List<T>();
            try
            {
                request = WebRequest.Create(url);
                request.Headers.Add("SecretKey:" + secretKey);
                ws = request.GetResponse();
                response = (Response)jsonSerializer.ReadObject(ws.GetResponseStream());
                tempList =  javaScriptSerializer.Deserialize<List<T>>(response.Data);
            }
            catch (Exception e)
            {
                LogError("File Name - "+fileName +" "+ e.Message);
            }

            foreach ( T obj in tempList)
            {
                Type firstType = obj.GetType();

                foreach (PropertyInfo propertyInfo in firstType.GetProperties())
                {
                    // populating firld name list
                    DBFieldAttribute dbField = null;
                    object[] attrs = propertyInfo.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    }
                    
                    if (dbField != null && (propertyInfo.PropertyType == typeof(string)) && propertyInfo.GetValue(obj, null)!=null)
                    {
                        propertyInfo.SetValue(obj, propertyInfo.GetValue(obj, null).ToString().Trim(), null);
                    }        
                }

                result.Add(obj);
            }


            return result;
        }
        public bool Delete<T>(Object obj) where T : class
        {
            Response response = GetResponse<T>(obj, "Delete");
            return response.ID != 500;
        }
        public T Last<T>(Object obj) where T : class
        {
            Response response = GetResponse<T>(obj, "Delete");
            T item = response != null ? javaScriptSerializer.Deserialize<T>(response.Data) : default(T);
            return item;
        }
        public Response GetResponse<T>(Object domainObject, string url)
        {
            T ob = (T)domainObject;
            string fileName = GetAttribute(typeof(T));
            foreach (var propertyInfo in ob.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(ob, null) == null)
                    {
                        propertyInfo.SetValue(ob, @"", null);
                    }
                }
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            ser.WriteObject(mem, javaScriptSerializer.Serialize(ob));
            string data = System.Text.Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
            
            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/json";
            webClient.Headers["PhysicalFileName"] = fileName;
            webClient.Headers["SecretKey"] = secretKey;

            webClient.Encoding = System.Text.Encoding.UTF8;
            // send the object to the web service
            Response response=new Response();
            try { 
            string responseStream = webClient.UploadString(serviceUrl + url, "POST", data);
                response = javaScriptSerializer.Deserialize<Response>(responseStream);
            }
            catch (Exception ex)
            {
                LogError("File Name:"+fileName+" "+ex.Message);
            }
            return response;
        }

        public void LogError(string ex)
        {
            try
            {
                string PATH = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"].ToString());
                using (StreamWriter writer = new StreamWriter(PATH, true))
                {
                    writer.WriteLine("\nMessage : " + ex);
                }

            }
            catch (Exception exception)
            {
            }
        }
    }
}