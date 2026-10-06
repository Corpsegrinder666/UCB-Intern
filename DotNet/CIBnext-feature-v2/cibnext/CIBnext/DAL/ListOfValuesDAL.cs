using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using CIBnext.BLL;

namespace CIBnext.DAL
{


    public class ListOfValuesDAL
    {
        private readonly IConfiguration _configuration;
        private Logger _logger = LogManager.GetLogger("ListOfValuesDAL");
        public ListOfValuesDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<KeyValuePair<string, string>> SelectListOfValues(LovTypes LovType)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = GetLovProcedureName(LovType);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            KeyValuePair<string, string> item = new KeyValuePair<string, string>(dr["CODE"].ToString(), dr["NAME"].ToString());
                            list.Add(item);
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private string GetLovProcedureName(LovTypes LovType)
        {
            if (LovType == LovTypes.THIRD_PARTY_GUARANTEE_TYPES)
                return "PKG_LOV.SP_3RD_PARTY_GUARANTEE_TYPES";
            else
                return "PKG_LOV.SP_" + Enum.GetName(typeof(LovTypes), LovType);
            //switch (LovType)
            //{
            //    case LovTypes.CONTRACT_PHASES:
            //        return "PKG_LOV.SP_CONTRACT_PHASES";
            //    case LovTypes.CONTRACT_STATUS:
            //        return "PKG_LOV.SP_CONTRACT_STATUS";
            //    case LovTypes.CONTRACT_TYPES_CARD:
            //    case LovTypes.CONTRACT_TYPES_INST:
            //    case LovTypes.CONTRACT_TYPES_NONINST:
            //        return "PKG_LOV.SP_CONTRACT_TYPES";
            //    case LovTypes.COUNTRIES:
            //        return "PKG_LOV.SP_COUNTRIES";
            //    case LovTypes.CURRENCIES:
            //        return "PKG_LOV.SP_CURRENCIES";
            //    case LovTypes.ECONOMIC_PURPOSE_CODES:
            //        return "PKG_LOV.SP_ECONOMIC_PURPOSE_CODES";
            //    case LovTypes.GENDER:
            //        return "PKG_LOV.SP_GENDER";
            //    case LovTypes.ID_TYPES:
            //        return "PKG_LOV.SP_ID_TYPES";
            //    case LovTypes.INSTALMENT_TYPES:
            //        return "PKG_LOV.SP_INSTALMENT_TYPES";
            //    case LovTypes.INSTITUTION_LEGAL_FORMS:
            //        return "PKG_LOV.SP_INSTITUTION_LEGAL_FORMS";
            //    case LovTypes.LEASED_GOOD_TYPES:
            //        return "PKG_LOV.SP_LEASED_GOOD_TYPES";
            //    case LovTypes.LINK_TYPES:
            //        return "PKG_LOV.SP_LINK_TYPES";
            //    case LovTypes.OWNER_TYPES:
            //        return "PKG_LOV.SP_OWNER_TYPES";
            //    case LovTypes.PAYMENT_METHODS:
            //        return "PKG_LOV.SP_PAYMENT_METHODS";
            //    case LovTypes.PERIODICITY_OF_PAYMENT:
            //        return "PKG_LOV.SP_PERIODICITY_OF_PAYMENT";
            //    case LovTypes.PRE_FINANCE_OF_LOAN:
            //        return "PKG_LOV.SP_PRE_FINANCE_OF_LOAN";
            //    case LovTypes.REORGANIZED_CREDIT:
            //        return "PKG_LOV.SP_REORGANIZED_CREDIT";
            //    case LovTypes.SECTOR_CODES:
            //        return "PKG_LOV.SP_SECTOR_CODES";
            //    case LovTypes.SECTOR_TYPES:
            //        return "PKG_LOV.SP_SECTOR_CODES";
            //    case LovTypes.SECURITY_TYPES:
            //        return "PKG_LOV.SP_SECURITY_TYPES";
            //    case LovTypes.SUBSIDIZED_CREDIT:
            //        return "PKG_LOV.SP_SUBSIDIZED_CREDIT";
            //    case LovTypes.THIRD_PARTY_GUARANTEE_TYPES:
            //        return "PKG_LOV.SP_3RD_PARTY_GUARANTEE_TYPES";
            //    default:
            //        return "";
            //}
        }

        internal List<string> SelectSROsByDistricts(string district)
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_LOV.SP_SUB_REGISTRY_OFFICES";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("IN_DISTRICT", district);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["SRO"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        internal List<string> SelectThanasByDistrict(string district)
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_LOV.SP_THANAS";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("IN_DISTRICT", district);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["THANA"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        internal List<string> SelectMouzasByDistrict(string district)
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_LOV.SP_MOUZAS";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("IN_DISTRICT", district);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["MOUZA"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        public List<string> SelectDistrictsByCountryCode(string countryCode)
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_LOV.SP_DISTRICTS";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("COUNTRY_CODE_IN", countryCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["DISTRICT_NAME"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }
    }
}