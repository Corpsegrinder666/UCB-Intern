using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    /// <summary>
    /// This class is just a wrapper to use when searching subject with Contract Code
    /// </summary>
    public class Subject
    {
        public PersonalData Person { get; set; }
        public Institution Institution { get; set; }
    }
}