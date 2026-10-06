//using CIBnext.ContractUI;
using CIBnext.LIB;

using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CIBnext.DAL
{
    public class FileDAL
    {
        private Logger _logger = LogManager.GetLogger("FileDAL");

        public string[] GetSubjectFileData(DateTime reportingPeriod)
        {
            List<string> lines = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_FILE.SP_GET_SUBJECT_FILE_DATA";
                    com.Parameters.Add("IN_REPORTING_PERIOD", reportingPeriod);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                            lines.Add(dr.GetString(0));
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return lines.ToArray();
        }

        public string[] GetSecuritySubjectFileData()
        {
            List<string> lines = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_FILE.SP_GET_SEC_SUBJECT_FILE_DATA";
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                            lines.Add(dr.GetString(0));
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return lines.ToArray();
        }

        public string[] GetSecurityFileData()
        {
            List<string> lines = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_FILE.SP_GET_SECURITY_FILE_DATA";
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                            lines.Add(dr.GetString(0));
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return lines.ToArray();
        }

        public string[] GetContractFileData(DateTime reportingPeriod)
        {
            List<string> lines = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_FILE.SP_GET_CONTRACT_FILE_DATA";
                    com.Parameters.Add("IN_REPORTING_PERIOD", reportingPeriod);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                            lines.Add(dr.GetString(0));
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return lines.ToArray();
        }

        public List<DAO.ContractLink> GetLinksForContracts(string[] contracts)
        {
            List<DAO.ContractLink> lines = new List<DAO.ContractLink>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    var paramNames = contracts.Select((id, index) => $":code{index}").ToArray();
                    var inClause = string.Join(", ", paramNames);

                    com.CommandText = $"SELECT RECORD_TYPE,FI_CODE,BRANCH_CODE,TYPE_OF_LINK,FI_PRIMARY_CODE,FI_SECONDARY_CODE,FI_CONTRACT_CODE,MAKE_BY,MAKE_DATE,RECORD_STATUS FROM contract_links WHERE record_status not in ('D','E','R') and fi_contract_code IN ({inClause})";
                    com.BindByName = true;

                    for (int i = 0; i < contracts.Length; i++)
                    {
                        com.Parameters.Add(new OracleParameter($"code{i}", OracleDbType.Varchar2)).Value = contracts[i];
                    }

                    using (OracleDataReader dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lines.Add(FillContractLinkRecord(dr));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return lines;
        }

        private DAO.ContractLink FillContractLinkRecord(IDataRecord dr)
        {
            DAO.ContractLink contract = new DAO.ContractLink();
            var ord = 0;
            if (dr.TryGetOrdinal("RECORD_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.RecordType = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FICode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                contract.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("TYPE_OF_LINK", out ord) && !dr.IsDBNull(ord))
                contract.TypeOfLink = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_PRIMARY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIPrimaryCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SECONDARY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FISecondaryCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.RecordStatus = dr.GetString(ord);
            return contract;
        }

        public void UpdateLinksAsReportForContracts(string[] contracts)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    var paramNames = contracts.Select((id, index) => $":code{index}").ToArray();
                    var inClause = string.Join(", ", paramNames);

                    com.CommandText = $"update contract_links set record_status = 'R' WHERE record_status not in ('D','E','R') and fi_contract_code IN ({inClause})";
                    com.BindByName = true;

                    for (int i = 0; i < contracts.Length; i++)
                    {
                        com.Parameters.Add(new OracleParameter($"code{i}", OracleDbType.Varchar2)).Value = contracts[i];
                    }

                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }
    }
}