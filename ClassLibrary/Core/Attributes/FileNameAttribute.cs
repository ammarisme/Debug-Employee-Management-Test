using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.Core.Attributes
{ 
    public class FileNameAttribute : Attribute
    {
        private string mFileName;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fieldName">name of the field that the property will be mapped to</param>
        public FileNameAttribute(string fileName)
        {
            mFileName= fileName;
        }

        public string FileName
        {
            get { return mFileName; }
        }
    }
}
