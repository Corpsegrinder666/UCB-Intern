using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class SecurityPayload
    {
        public PersonalData[] Persons { get; set; }
        public Institution[] Institutions { get; set; }
        public LandBuilding[] LandBuildings { get; set; }
        public Flat[] Flats { get; set; }
        public Machinery[] Machineries { get; set; }
        public Mortgage[] Mortgages { get; set; }
        public Hypothecation[] Hypothecations { get; set; }
    }
}