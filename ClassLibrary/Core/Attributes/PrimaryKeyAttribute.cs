using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        private bool isPrimary;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fieldName">name of the field that the property will be mapped to</param>
        public PrimaryKeyAttribute()
        {
            isPrimary = true;
        }

        public bool IsPrimary
        {
            get { return isPrimary; }
        }
    }
}
