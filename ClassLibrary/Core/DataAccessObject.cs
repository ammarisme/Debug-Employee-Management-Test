

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Data.Odbc;
using ClassLibrary.Core.Attributes;

namespace ClassLibrary.Core
{
    public class DataAccessObject :IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbConnection"></param>
        /// <param name="readCommand"></param>
        /// <returns></returns>
        public List<object> ReadCollection(OdbcDataReader reader, object obj) 
        {
            List<object> collection = new List<object>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    object instance = (object) Activator.CreateInstance(obj.GetType());
                    
                     object item = GetItemFromReader(reader, instance);
                    collection.Add(item);
                }
            }

            return collection;

        }
        /// <summary>
        /// This method is used to Read Table Row From databse and assing its column values to 
        /// Relevant Domain object.
        /// </summary>
        /// <typeparam name="T">Generic Domain Object</typeparam>
        /// <param name="dbConnection">Opend Database Connection</param>
        /// <param name="readCommand">parameterized Sql Command</param>
        /// <returns>Generic Domain Object</returns>
        public object GetSingleOject(OdbcDataReader reader, object singleObject)
        {
            Type type = singleObject.GetType();
            

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    singleObject = GetItemFromReader(reader, singleObject);
                }
            }
            return singleObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public object GetItemFromReader(OdbcDataReader rdr, object item) 
        {
            Type type = item.GetType();
            
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Type type1 = property.ReflectedType;
                // for each property declared in the type provided check if the property is
                // decorated with the DBField attribute
                if (Attribute.IsDefined(property, typeof(DBFieldAttribute)))
                {
                    DBFieldAttribute attrib = (DBFieldAttribute)Attribute.GetCustomAttribute(property, typeof(DBFieldAttribute));

                    try
                    {

                        if (Convert.IsDBNull(rdr[attrib.FieldName])) // data in database is null, so do not set the value of the property
                            continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                   

                    if (property.PropertyType == rdr[attrib.FieldName].GetType()) // if the property and database field are the same
                        property.SetValue(item, rdr[attrib.FieldName], null); // set the value of property
                    else
                    {
                        try
                        {
                            property.SetValue(item, Convert.ChangeType(rdr[attrib.FieldName], property.PropertyType), null);
                        }
                        catch (Exception)
                        {
                            property.SetValue(item,rdr[attrib.FieldName].ToString().Trim(), null);
                        }
                        // change the type of the data in table to that of property and set the value of the property
                        
                        // for SQL attrib.FieldName canbe modified as ( "@" +attrib.FieldName)
                    }      
             
       
                }
            }

            return item;
        }


        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
