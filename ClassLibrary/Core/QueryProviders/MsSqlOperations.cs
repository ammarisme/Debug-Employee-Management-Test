using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Linq.Expressions;
using System.IO;
using ClassLibrary.Core.Attributes;

namespace ClassLibrary.Core.QueryProviders
{
    /// <summary>
    /// Containing all the methods that do database operations
    /// </summary>
    public class MsSqlQueryProvider : iQueryProvider
    {
        private DBConnection dbConnection;
        private string library;
        Utilities utils;
        public MsSqlQueryProvider()
        {
            library = System.Configuration.ConfigurationSettings.AppSettings["LibraryName"].ToString();
            dbConnection = new DBConnection(@"DSN=ODBCMSSQL");
            utils = new Utilities();
        }

        public List<object> SelectAll(string file) 
        {
            DataAccessObject dataAccessObject = new DataAccessObject();
            dbConnection.cmd.CommandText = "SELECT * FROM ["+library+"].[dbo].[" + file+ "]";
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;
            
            List<object> list = new List<object>();
            DatabaseAccess dbAccess = new DatabaseAccess();

            object obj = dbAccess.DeserializeToDomain(file, "{}");

            try
            {
                using (dbConnection.dr = dbConnection.cmd.ExecuteReader())
                {
                    list = dataAccessObject.ReadCollection(dbConnection.dr, obj);
                }
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return new List<object>();
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return list;
        }

        public string Insert(string fileName, Object obj)
        {
            string query = "INSERT INTO " + library + "." + fileName + " ";
            string fields = "( ";
            string values = " VALUES ( ";
            string primaryKeyId= "ID";
            Type firstType = obj.GetType();
            decimal pkId = 0;

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                // populating firld name list
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute primary = null;
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    primary = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : primary;
                }

                decimal recCount;
                try
                {
                    dbConnection.cmd.CommandText = "SELECT COUNT(*) FROM " + library + "." + fileName;
                    recCount = decimal.Parse(dbConnection.cmd.ExecuteScalar().ToString());
                }
                catch (Exception ex)
                {
                    recCount = 0;
                }
                
                if (primary != null && dbField != null & recCount > 0)
                {
                    try
                    {
                        dbConnection.cmd.CommandText = "SELECT MAX(cast(" + dbField.FieldName + " as int)) FROM " + library + "." + fileName;
                        pkId = decimal.Parse(dbConnection.cmd.ExecuteScalar().ToString());
                        pkId++;
                    }
                    catch (Exception ex)
                    {
                        pkId = 0;
                    }

                }

                string value = (propertyInfo.PropertyType == typeof(decimal)) ? propertyInfo.GetValue(obj, null).ToString() : "'" + propertyInfo.GetValue(obj, null) + "'";
                if (dbField != null & primary == null && value != "0" && value != "" && value != "''")
                {
                    fields = fields == "( " ? (fields + dbField.FieldName) : (fields + "," + dbField.FieldName);
                    values = values == " VALUES ( " ? values + value : values + " , " + value;
                }
                else if (dbField != null & primary != null)
                {
                    primaryKeyId = dbField.FieldName;
                    fields = fields == "( " ? (fields + dbField.FieldName) : (fields + "," + dbField.FieldName);
                    string primaryKey = (propertyInfo.PropertyType == typeof(decimal)) ? pkId.ToString() : "'" + pkId.ToString() + "'";
                    values = values == " VALUES ( " ? values + primaryKey : values + " , " + primaryKey;
                }
            }

            query = "INSERT INTO [" + library + "].[dbo].[" + fileName + "] " + fields + " ) " + values + " )";

            int success = -1;
            try
            {
                dbConnection.cmd.CommandText = query;
                dbConnection.cmd.CommandType = System.Data.CommandType.Text;
                success = dbConnection.cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            utils.Logger(query);
            return (success == 1) ? pkId.ToString() : "-1";
        }

        public string InsertDefinition(string fileName, Object obj)
        {
            string query = "INSERT INTO " + library + "." + fileName + " ";
            string fields = "( ";
            string values = " VALUES ( ";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                // populating firld name list
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute primary = null;
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    primary = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : primary;
                }
                
                string value = (propertyInfo.PropertyType == typeof(decimal)) ? propertyInfo.GetValue(obj, null).ToString() : "'" + propertyInfo.GetValue(obj, null) + "'";
                if (dbField != null)
                {
                    fields = fields == "( " ? (fields + dbField.FieldName) : (fields + "," + dbField.FieldName);
                    values = values == " VALUES ( " ? values + value : values + " , " + value;
                }
            }

            query = "INSERT INTO " + library + "." + fileName + " " + fields + " ) " + values + " )";

            int success = -1;
            try
            {
                dbConnection.cmd.CommandText = query;
                dbConnection.cmd.CommandType = System.Data.CommandType.Text;
                success = dbConnection.cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            utils.Logger(query);
            return (success == 1) ? "1" : "Error - Query:" + query;
        }

        public string Update(string fileName, Object obj)
        {
            string update = "UPDATE " + library + "." + fileName;
            string setValues = "";
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                // populating firld name list
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute primaryKey = null;
                string value = (propertyInfo.PropertyType == typeof(decimal)) ? propertyInfo.GetValue(obj, null).ToString() : "'" + propertyInfo.GetValue(obj, null) + "'";

                // Go through each attribute of the property
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    primaryKey = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : primaryKey;
                }
                if (primaryKey != null && primaryKey.IsPrimary)
                {
                    whereCondition = dbField.FieldName + "=" + value;
                    continue;
                }
                if (dbField != null && value != "" && value != "0" & value != string.Empty && value != "''")
                {
                    string condition = dbField.FieldName + "=" + value;
                    condition = setValues == "" ? condition : " , " + condition;
                    setValues += condition;
                }

            }
            setValues = setValues != "" ? " SET " + setValues : setValues;
            whereCondition = whereCondition != "" ? " WHERE " + whereCondition : whereCondition;
            query = update + setValues + whereCondition;
            dbConnection.cmd.CommandText = query;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;


            int success = -1;
            try
            {
                success = dbConnection.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            utils.Logger(query);
            return "1";
        }

        public string UpdateAllFields(string fileName, Object obj)
        {
            string update = "UPDATE " + library + "." + fileName;
            string setValues = "";
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                // populating firld name list
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute primaryKey = null;
                string value = (propertyInfo.PropertyType == typeof(decimal)) ? propertyInfo.GetValue(obj, null).ToString() : "'" + propertyInfo.GetValue(obj, null) + "'";

                // Go through each attribute of the property
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    primaryKey = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : primaryKey;
                }
                if (primaryKey != null && primaryKey.IsPrimary)
                {
                    whereCondition = dbField.FieldName + "=" + value;
                    continue;
                }
                if (dbField != null)
                {
                    string updateValue = dbField.FieldName + "=" + value;
                    updateValue = setValues == "" ? updateValue : " , " + updateValue;
                    setValues += updateValue;
                }

            }
            setValues = setValues != "" ? " SET " + setValues : setValues;
            whereCondition = whereCondition != "" ? " WHERE " + whereCondition : whereCondition;
            query = update + setValues + whereCondition;
            dbConnection.cmd.CommandText = query;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;


            int success = -1;
            try
            {
                success = dbConnection.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            utils.Logger(query);
            return "1";
        }

        public List<object> Select(string fileName, Object obj) 
        {
            string select = "SELECT * FROM \"" + library + "\".\"" + fileName+'"';
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();
            List<object> list = new List<object>();
            DataAccessObject dataAccessObject = new DataAccessObject();
            
            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute pkField = null;
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                }
                if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && propertyInfo.GetValue(obj, null).ToString() != "0"))
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = '"'+dbField.FieldName + "\"=" + value+"";
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }
                else if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && pkField != null))
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }

            }
            whereCondition = whereCondition != "" ? " WHERE " + whereCondition : whereCondition;
            query = select + whereCondition;

            dbConnection.cmd.CommandText = query;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                using (dbConnection.dr = dbConnection.cmd.ExecuteReader())
                {
                    list = dataAccessObject.ReadCollection(dbConnection.dr, obj);
                }
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return new List<object>();
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return list;
        }

        public object SelectLast(string fileName, Object obj)
        {
            string select = "SELECT * FROM "+library+"."+fileName+" WHERE ";
            string castPk= "";
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();
            object result = new object();
            DataAccessObject dataAccessObject = new DataAccessObject();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute pkField = null;
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    pkField= (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute: pkField;
                }
                if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && propertyInfo.GetValue(obj, null).ToString() != "0"))
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }
                if (pkField != null)
                {
                    castPk = " CAST(" + dbField.FieldName + " AS INT) ";
                }

            }
            whereCondition = whereCondition != "" ? " WHERE " + whereCondition : whereCondition;
            query = " ( SELECT MAX(" + castPk + ") FROM " + library + "." + fileName + " "+whereCondition +") ";
            query = "SELECT * FROM " + library + "." + fileName+" WHERE "+castPk +"=" + query;
            dbConnection.cmd.CommandText = query;
            dbConnection.cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                using (dbConnection.dr = dbConnection.cmd.ExecuteReader())
                {
                    result = dataAccessObject.GetSingleOject(dbConnection.dr ,obj);
                }
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.RollBack();
                }
                return result;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            return result;
        }

        public string Delete(string fileName, Object obj)
        {
            string delete = "DELETE FROM " + library + "." + fileName + " WHERE ";
            string whereCondition = "";
            string query = "";
            Type firstType = obj.GetType();

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                object[] attrs = propertyInfo.GetCustomAttributes(true);
                DBFieldAttribute dbField = null;
                PrimaryKeyAttribute pkField = null;
                foreach (object attr in attrs)
                {
                    dbField = (attr as DBFieldAttribute) != null ? attr as DBFieldAttribute : dbField;
                    pkField = (attr as PrimaryKeyAttribute) != null ? attr as PrimaryKeyAttribute : pkField;
                }
                if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && propertyInfo.GetValue(obj, null).ToString() != "0" && propertyInfo.GetValue(obj, null).ToString() != "''") && pkField == null)
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }
                else if (dbField != null && (propertyInfo.GetValue(obj, null).ToString() != "" && propertyInfo.GetValue(obj, null).ToString() != "0" && propertyInfo.GetValue(obj, null).ToString() != "''") && pkField != null)
                {
                    string value = propertyInfo.PropertyType == typeof(string) ? "'" + propertyInfo.GetValue(obj, null).ToString() + "'" : propertyInfo.GetValue(obj, null).ToString();
                    string condition = dbField.FieldName + "=" + value;
                    condition = whereCondition == "" ? " " + condition : " AND " + condition;
                    whereCondition += condition;
                }

            }
            query = delete + whereCondition;

            int success = -1;
            try
            {
                dbConnection.cmd.CommandText = query;
                dbConnection.cmd.CommandType = System.Data.CommandType.Text;
                success = dbConnection.cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            utils.Logger(query);
            return (success >= 1) ? "1" : "Error - Query :" + query;
        }

        public string DeleteAll(string fileName)
        {
            string query = "DELETE FROM " + library + "." + fileName;

            int success = -1;
            try
            {
                dbConnection.cmd.CommandText = query;
                dbConnection.cmd.CommandType = System.Data.CommandType.Text;
                success = dbConnection.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.RollBack();
                return ex.Message + " - Query :" + query;
            }
            finally
            {
                if (dbConnection.con.State == System.Data.ConnectionState.Open)
                    dbConnection.Commit();
            }

            utils.Logger(query);
            return (success >= 1) ? "1" : "Error - Query :" + query;
        }
        
    }
    }
