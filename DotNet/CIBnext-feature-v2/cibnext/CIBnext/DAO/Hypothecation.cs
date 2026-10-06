using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Hypothecation
    {
        public string RecordType { get; set; }
        public string FiCode { get; set; }
        public string BranchCode { get; set; }
        public string FiHypothecationCode { get; set; }
        public DateTime? DateHypothecation { get; set; }
        public string RjscFillingNo { get; set; }
        public DateTime? RjscFillingDate { get; set; }
        public string RankingCharge { get; set; }
        public string PariPassuCharge { get; set; }
        public string HypothecationPhase { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string CheckBy { get; set; }
        public DateTime CheckDate { get; set; }
        public string RecordStatus { get; set; }
    }
}