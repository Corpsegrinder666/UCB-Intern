using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class JsonRecord
    {
        public bool Success { get; set; }
        public string[] Messages { get; set; }
        public object Payload { get; set; }
    }
}