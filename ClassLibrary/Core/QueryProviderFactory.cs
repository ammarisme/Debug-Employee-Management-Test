using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary.Core.QueryProviders;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Linq.Expressions;
using System.IO;

namespace ClassLibrary.Core
{
    public class QueryProviderFactory
    {
        public QueryProviderFactory()
        {

        }
        
        /// <summary>
        /// mssql,db2
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        public iQueryProvider GetQueryProvider(string queryType)
        {
            queryType = queryType.ToLower();
            switch (queryType)
            {
                case "db2":
                    return new DB2Operations();
                case "mssql":
                    return new MsSqlQueryProvider();
                default:
                    return null;
            }
        }       
    }
    }
