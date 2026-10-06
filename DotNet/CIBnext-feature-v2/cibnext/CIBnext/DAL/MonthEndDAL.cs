using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using NLog;
using System.Data;

namespace CIBnext.DAL
{
    public class MonthEndDAL
    {
        Logger _logger = LogManager.GetLogger("MonthEndDAL");

        internal bool UpdateSubjectsToLock()
        {
            try
            {
                using (OracleCommand com = new OracleCommand())
                using (com.Connection = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    com.Connection.Open();
                    com.CommandText = "PKG_MTH_END.SP_LOCK_SUBJECTS";
                    com.CommandType = CommandType.StoredProcedure;

                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        internal bool UpdateLinksToLock()
        {
            try
            {
                using (OracleCommand com = new OracleCommand())
                using (com.Connection = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    com.Connection.Open();
                    com.CommandText = "PKG_MTH_END.SP_LOCK_LINKS";
                    com.CommandType = CommandType.StoredProcedure;

                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        internal bool TruncateErrorTables()
        {
            try
            {
                using (OracleCommand com = new OracleCommand())
                using (com.Connection = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    com.Connection.Open();
                    com.CommandText = "PKG_MTH_END.SP_CLEAR_ERROR_LIST";
                    com.CommandType = CommandType.StoredProcedure;

                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        internal bool UpdateTMTAContractsToTerminate()
        {
            try
            {
                using (OracleCommand com = new OracleCommand())
                using (com.Connection = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    com.Connection.Open();
                    com.CommandText = "PKG_MTH_END.SP_TERMINATE_TMTA_CONTRACTS";
                    com.CommandType = CommandType.StoredProcedure;

                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }
    }
}