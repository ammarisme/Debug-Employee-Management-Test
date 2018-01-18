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
    public interface iQueryProvider
    {
        List<object> SelectAll(string file);
        string Insert(string fileName, Object obj);
        string InsertDefinition(string fileName, Object obj);
        string Update(string fileName, Object obj);
        string UpdateAllFields(string fileName, Object obj);
        List<object> Select(string fileName, Object obj);
        object SelectLast(string fileName, Object obj);
        string Delete(string fileName, Object obj);
        string DeleteAll(string fileName);
    }
    }
