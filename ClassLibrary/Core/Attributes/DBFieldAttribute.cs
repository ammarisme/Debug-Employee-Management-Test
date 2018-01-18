using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DBFieldAttribute : Attribute
    {
        private string mFieldName;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fieldName">name of the field that the property will be mapped to</param>
        public DBFieldAttribute(string fieldName)
        {
            mFieldName = fieldName;
        }

        public string FieldName
        {
            get { return mFieldName; }
        }
    }
}