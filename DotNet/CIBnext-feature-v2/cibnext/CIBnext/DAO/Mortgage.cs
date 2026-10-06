using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Mortgage
    {
        public string RecordType { get; set; }
        public string FiCode { get; set; }
        public string BranchCode { get; set; }
        public string FiMortgageCode { get; set; }
        public string MortgageType { get; set; }
        public string MortgageDeedNo { get; set; }
        public DateTime MortgageDate { get; set; }
        public int MortgageValue { get; set; }
        public int MarketValue { get; set; }
        public string RjscFillingNo { get; set; }
        public DateTime? RjscFillingDate { get; set; }
        public string RankingCharge { get; set; }
        public string PariPassuCharge { get; set; }
        public string RigpaNo { get; set; }
        public DateTime? RigpaDate { get; set; }
        public string MortgagePhase { get; set; }
        public string SecurityCategory { get; set; }
        public string MortgagedLandArea { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string CheckBy { get; set; }
        public DateTime? CheckDate { get; set; }
        public string RecordStatus { get; set; }
    }
}