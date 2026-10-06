using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using CIBnext.DAO;
using CIBnext.LIB;
using System.Data;

namespace CIBnext.DAL
{
    public class ContractDAL
    {
        private Logger _logger = LogManager.GetLogger("ContractDAL");
        private readonly IConfiguration _configuration;

        public ContractDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool InsertContract(InstalmentContract contract)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_CONTRACT.SP_INSTALMENT_CONTRACT_INSERT";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("IN_RECORD_TYPE", contract.RecordType);
                    com.Parameters.Add("IN_FI_CODE", contract.FICode);
                    com.Parameters.Add("IN_BRANCH_CODE", contract.BranchCode);
                    com.Parameters.Add("IN_FI_SUBJECT_CODE", contract.FISubjectCode);
                    com.Parameters.Add("IN_FI_CONTRACT_CODE", contract.FIContractCode);
                    com.Parameters.Add("IN_CONTRACT_TYPE", contract.ContractType);
                    com.Parameters.Add("IN_CONTRACT_PHASE", contract.ContractPhase);
                    com.Parameters.Add("IN_CONTRACT_STATUS", contract.ContractStatus);
                    com.Parameters.Add("IN_CURRENCY_CODE", contract.CurrencyCode);
                    if (contract.StartingDate == default(DateTime))
                        com.Parameters.Add("IN_STARTING_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_STARTING_DATE", contract.StartingDate.ToString("dd MMM yyyy"));
                    if (contract.RequestDate == default(DateTime))
                        com.Parameters.Add("IN_REQUEST_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_REQUEST_DATE", contract.RequestDate.ToString("dd MMM yyyy"));
                    if (contract.PlannedEndDate == default(DateTime))
                        com.Parameters.Add("IN_PLANNED_END_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_PLANNED_END_DATE", contract.PlannedEndDate.ToString("dd MMM yyyy"));
                    if (contract.ActualEndDate == default(DateTime))
                        com.Parameters.Add("IN_ACTUAL_END_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_ACTUAL_END_DATE", contract.ActualEndDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_DEFAULTER_STATUS", contract.DefaulterStatus);
                    com.Parameters.Add("IN_LAST_PAYMENT_DATE",
                        contract.DateOfLastPayment is DateTime dt
                        ? dt.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_SUBSIDIZED_CREDIT", contract.FlagSubsidizedCredit);
                    com.Parameters.Add("IN_PRE_FINANCE_LOAN", contract.FlagPreFinanceOfLoan);
                    com.Parameters.Add("IN_REORGANIZED_CREDIT", contract.CodeReorganizedCredit);
                    com.Parameters.Add("IN_THIRD_PARTY_GUARANTEE_TYPE", contract.ThirdPartyGuaranteeType);
                    com.Parameters.Add("IN_SECURITY_TYPE", contract.SecurityType);
                    com.Parameters.Add("IN_AMOUNT_GURANTEED_3RD_PARTY", contract.AmountGuaranteedByThirdParty);
                    com.Parameters.Add("IN_AMOUNT_GUARANTEED_SECURITY", contract.AmountGuaranteedBySecurityType);
                    com.Parameters.Add("IN_QUALIFICATION_JUDGEMENT", contract.QualitativeJudgement);
                    com.Parameters.Add("IN_SANCTION_LIMIT", contract.SanctionLimit);
                    com.Parameters.Add("IN_TOTAL_DISBURSED_AMOUNT", contract.TotalDisbursedAmount);
                    com.Parameters.Add("IN_TOTAL_OUTSTANDING_AMOUNT", contract.TotalOutstandingAmount);
                    com.Parameters.Add("IN_TOTAL_NUMBER_INSTALMENTS", contract.TotalNumberOfInstallments);
                    com.Parameters.Add("IN_PERIODICITY_PAYMENT", contract.PeriodicityOfPayment);
                    com.Parameters.Add("IN_METHOD_PAYMENT", contract.MethodOfPayment);
                    com.Parameters.Add("IN_INSTALMENT_AMOUNT", contract.InstalmentAmount);
                    com.Parameters.Add("IN_EXPIRATION_DATE_NEXT_INST",
                        contract.ExpirationDateOfNextInstallment is DateTime dte
                        ? dte.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_AMOUNT_NEXT_EXPIRING_INST", contract.AmountOfNextExpiringInstallment);
                    com.Parameters.Add("IN_NO_OF_REMAINING_INSTALMENT", contract.NumberOfRemainingInstallments);
                    com.Parameters.Add("IN_REMAINING_AMOUNT", contract.RemainingAmount);
                    com.Parameters.Add("IN_NO_OF_OVERDUE_INSTALMENT", contract.NumberOfOverdueInstallment);
                    com.Parameters.Add("IN_OVERDUE_AMOUNT", contract.OverdueAmount);
                    com.Parameters.Add("IN_NO_OF_DAYS_PAYMENT_DELAY", contract.NumberOfDaysOfPaymentDelay);
                    com.Parameters.Add("IN_TYPE_OF_LEASED_GOOD", contract.TypeOfLeasedGood);
                    com.Parameters.Add("IN_VALUE_OF_LEASED_GOOD", contract.ValueOfLeasedGood);
                    com.Parameters.Add("IN_REGISTRATION_NUMBER", contract.RegistrationNumber);
                    if (contract.DateOfManufacturing == default(DateTime))
                        com.Parameters.Add("IN_DATE_OF_MANUFACTURING", DBNull.Value);
                    else
                        com.Parameters.Add("IN_DATE_OF_MANUFACTURING", contract.DateOfManufacturing.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_DUE_FOR_RECOVERY", contract.DueForRecovery);
                    com.Parameters.Add("IN_RECOVERY_DURING_REPORTING", contract.RecoveryDuringTheReportingPeriod);
                    com.Parameters.Add("IN_CUMULATIVE_RECOVERY", contract.CumulativeRecovery);
                    com.Parameters.Add("IN_DATE_OF_LAW_SUIT",
                        contract.DateOfLawSuit is DateTime dtl
                        ? dtl.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_DATE_OF_CLASSIFICATION",
                        contract.DateOfClassification is DateTime dtc
                        ? dtc.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_NO_TIMES_RESCHEDULING", contract.NumberOfTimesRescheduling);
                    if (contract.DateOfLastRescheduling == default(DateTime))
                        com.Parameters.Add("IN_DATE_LAST_RESCHEDULING", DBNull.Value);
                    else
                        com.Parameters.Add("IN_DATE_LAST_RESCHEDULING", contract.DateOfLastRescheduling.ToString("dd MMM yyyy"));

                    com.Parameters.Add("IN_ECONOMIC_PURPOSE_CODE", contract.EconomicPurposeCode);
                    com.Parameters.Add("IN_SME", contract.SME);
                    com.Parameters.Add("IN_ENTERPRISE_TYPE", contract.EnterpriseType);
                    com.Parameters.Add("IN_MAKE_BY", contract.MakeBy);
                    com.Parameters.Add("IN_MAKE_DATE", contract.MakeDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_UBS_ACCOUNT_NO", contract.UbsAccountNo);
                    com.Parameters.Add("IN_RECORD_STATUS", contract.RecordStatus);

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }

        public bool InsertContract(DAO.NonInstalmentContract contract)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_CONTRACT.SP_NON_INSTL_CONTRACT_INSERT";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("IN_RECORD_TYPE", contract.RecordType);
                    com.Parameters.Add("IN_FI_CODE", contract.FICode);
                    com.Parameters.Add("IN_BRANCH_CODE", contract.BranchCode);
                    com.Parameters.Add("IN_FI_SUBJECT_CODE", contract.FISubjectCode);
                    com.Parameters.Add("IN_FI_CONTRACT_CODE", contract.FIContractCode);
                    com.Parameters.Add("IN_CONTRACT_TYPE", contract.ContractType);
                    com.Parameters.Add("IN_CONTRACT_PHASE", contract.ContractPhase);
                    com.Parameters.Add("IN_CONTRACT_STATUS", contract.ContractStatus);
                    com.Parameters.Add("IN_CURRENCY_CODE", contract.CurrencyCode);
                    if (contract.StartingDate == default(DateTime))
                        com.Parameters.Add("IN_STARTING_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_STARTING_DATE", contract.StartingDate.ToString("dd MMM yyyy"));
                    if (contract.RequestDate == default(DateTime))
                        com.Parameters.Add("IN_REQUEST_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_REQUEST_DATE", contract.RequestDate.ToString("dd MMM yyyy"));
                    if (contract.PlannedEndDate == default(DateTime))
                        com.Parameters.Add("IN_PLANNED_END_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_PLANNED_END_DATE", contract.PlannedEndDate.ToString("dd MMM yyyy"));
                    if (contract.ActualEndDate == default(DateTime))
                        com.Parameters.Add("IN_ACTUAL_END_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_ACTUAL_END_DATE", contract.ActualEndDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_DEFAULTER_STATUS", contract.DefaulterStatus);
                    com.Parameters.Add("IN_DATE_OF_LAST_PAYMENT",
                        contract.DateOfLastPayment is DateTime dt
                        ? dt.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_SUBSIDIZED_CREDIT", contract.FlagSubsidizedCredit);
                    com.Parameters.Add("IN_PRE_FINANCE_LOAN", contract.FlagPreFinanceOfLoan);
                    com.Parameters.Add("IN_REORGANIZED_CREDIT", contract.CodeReorganizedCredit);
                    com.Parameters.Add("IN_THIRD_PARTY_GUARANTEE_TYPE", contract.ThirdPartyGuaranteeType);
                    com.Parameters.Add("IN_SECURITY_TYPE", contract.SecurityType);
                    com.Parameters.Add("IN_AMOUNT_GUARANTEED_3RD_PARTY", contract.AmountGuaranteedByThirdParty);
                    com.Parameters.Add("IN_AMOUNT_GUARANTEED_SECURITY", contract.AmountGuaranteedBySecurityType);
                    com.Parameters.Add("IN_QUALITATIVE_JUDGEMENT", contract.QualitativeJudgement);
                    com.Parameters.Add("IN_SANCTION_LIMIT", contract.SanctionLimit);
                    com.Parameters.Add("IN_TOTAL_OUTSTANDING_AMOUNT", contract.TotalOutstandingAmount);
                    com.Parameters.Add("IN_NO_OF_DAYS_PAYMENT_DELAY", contract.NumberOfDaysOfPaymentDelay);
                    com.Parameters.Add("IN_DUE_FOR_RECOVERY", contract.DueForRecovery);
                    com.Parameters.Add("IN_RECOVERY_DURING_REPORTING", contract.RecoveryDuringTheReportingPeriod);
                    com.Parameters.Add("IN_CUMULATIVE_RECOVERY", contract.CumulativeRecovery);
                    com.Parameters.Add("IN_DATE_OF_LAW_SUIT",
                        contract.DateOfLawSuit is DateTime dtl
                        ? dtl.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_DATE_OF_CLASSIFICATION",
                        contract.DateOfClassification is DateTime dtc
                        ? dtc.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_NO_OF_TIMES_RESCHEDULING", contract.NumberOfTimesRescheduling);
                    if (contract.DateOfLastRescheduling == default(DateTime))
                        com.Parameters.Add("IN_DATE_LAST_RESCHEDULING", DBNull.Value);
                    else
                        com.Parameters.Add("IN_DATE_LAST_RESCHEDULING", contract.DateOfLastRescheduling.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_ECONOMIC_PURPOSE_CODE", contract.EconomicPurposeCode);
                    com.Parameters.Add("IN_SME", contract.SME);
                    com.Parameters.Add("IN_ENTERPRISE_TYPE", contract.EnterpriseType);
                    com.Parameters.Add("IN_MAKE_BY", contract.MakeBy);
                    com.Parameters.Add("IN_MAKE_DATE", contract.MakeDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_UBS_ACCOUNT_NO", contract.UbsAccountNo);
                    com.Parameters.Add("IN_RECORD_STATUS", contract.RecordStatus);
                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }

        public bool InsertContract(CardContract contract)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_CARD_CONTRACT_INSERT";
                    com.Parameters.Add("IN_RECORD_TYPE", contract.RecordType);
                    com.Parameters.Add("IN_FI_CODE", contract.FICode);
                    com.Parameters.Add("IN_BRANCH_CODE", contract.BranchCode);
                    com.Parameters.Add("IN_FI_SUBJECT_CODE", contract.FISubjectCode);
                    com.Parameters.Add("IN_FI_CONTRACT_CODE", contract.FIContractCode);
                    com.Parameters.Add("IN_CONTRACT_TYPE", contract.ContractType);
                    com.Parameters.Add("IN_CONTRACT_PHASE", contract.ContractPhase);
                    com.Parameters.Add("IN_CONTRACT_STATUS", contract.ContractStatus);
                    com.Parameters.Add("IN_CURRENCY_CODE", contract.CurrencyCode);
                    if (contract.StartingDate == default(DateTime))
                        com.Parameters.Add("IN_STARTING_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_STARTING_DATE", contract.StartingDate.ToString("dd MMM yyyy"));
                    if (contract.RequestDate == default(DateTime))
                        com.Parameters.Add("IN_REQUEST_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_REQUEST_DATE", contract.RequestDate.ToString("dd MMM yyyy"));
                    if (contract.PlannedEndDate == default(DateTime))
                        com.Parameters.Add("IN_PLANNED_END_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_PLANNED_END_DATE", contract.PlannedEndDate.ToString("dd MMM yyyy"));
                    if (contract.ActualEndDate == default(DateTime))
                        com.Parameters.Add("IN_ACTUAL_END_DATE", default(DateTime));
                    else
                        com.Parameters.Add("IN_ACTUAL_END_DATE", contract.ActualEndDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_DEFAULTER_STATUS", contract.DefaulterStatus);
                    com.Parameters.Add("IN_DATE_LAST_PAYMENT",
                        contract.DateOfLastPayment is DateTime dt
                        ? dt.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_SUBSIDIZED_CREDIT", contract.FlagSubsidizedCredit);
                    com.Parameters.Add("IN_PRE_FINANCE_OF_LOAN", contract.FlagPreFinanceOfLoan);
                    com.Parameters.Add("IN_REORGANIZED_CREDIT", contract.CodeReorganizedCredit);
                    com.Parameters.Add("IN_THIRD_PARTY_GUARANTEE_TYPE", contract.ThirdPartyGuaranteeType);
                    com.Parameters.Add("IN_SECURITY_TYPE", contract.SecurityType);
                    com.Parameters.Add("IN_AMOUNT_GUARANTEED_3RD_PARTY", contract.AmountGuaranteedByThirdParty);
                    com.Parameters.Add("IN_AMOUNT_GUARANTEED_SECURITY", contract.AmountGuaranteedBySecurityType);
                    com.Parameters.Add("IN_QUALITATIVE_JUDGEMENT", contract.QualitativeJudgement);
                    com.Parameters.Add("IN_PERIODICITY_PAYMENT", contract.PeriodicityOfPayment);
                    com.Parameters.Add("IN_METHOD_OF_PAYMENT", contract.MethodOfPayment);
                    com.Parameters.Add("IN_INSTALMENT_AMOUNT", contract.InstalmentAmount);
                    com.Parameters.Add("IN_CREDIT_LIMIT", contract.CreditLimit);
                    com.Parameters.Add("IN_TOTAL_OUTSTANDING_AMOUNT", contract.TotalOutstandingAmount);
                    com.Parameters.Add("IN_EXPIRATION_DATE_NEXT_INST",
                        contract.ExpirationDateOfNextInstallment is DateTime dte
                        ? dte.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_REMAINING_AMOUNT", contract.RemainingAmount);
                    com.Parameters.Add("IN_NUMBER_OF_OVERDUE", contract.NumberOfOverdueInstallment);
                    com.Parameters.Add("IN_OVERDUE_AMOUNT", contract.OverdueAmount);
                    if (contract.DateOfLastCharge == default(DateTime))
                        com.Parameters.Add("IN_DATE_OF_LAST_CHARGE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_DATE_OF_LAST_CHARGE", contract.DateOfLastCharge.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_TYPE_OF_INSTALMENT", contract.TypeOfInstallment);
                    com.Parameters.Add("IN_NO_OF_DAYS_PAYMENT_DELAY", contract.NumberOfDaysOfPaymentDelay);
                    com.Parameters.Add("IN_DUE_FOR_RECOVERY", contract.DueForRecovery);
                    com.Parameters.Add("IN_RECOVERY_DURING_REPORTING", contract.RecoveryDuringTheReportingPeriod);
                    com.Parameters.Add("IN_CUMULATIVE_RECOVERY", contract.CumulativeRecovery);
                    com.Parameters.Add("IN_DATE_OF_LAW_SUIT",
                        contract.DateOfLawSuit is DateTime dtl
                        ? dtl.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.Parameters.Add("IN_ECONOMIC_PURPOSE_CODE", contract.EconomicPurposeCode);
                    com.Parameters.Add("IN_MAKE_BY", contract.MakeBy);
                    com.Parameters.Add("IN_MAKE_DATE", contract.MakeDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_RECORD_STATUS", contract.RecordStatus);
                    com.Parameters.Add("IN_DATE_OF_CLASSIFICATION",
                        contract.DateOfClassification is DateTime dtc
                        ? dtc.ToString("dd MMM yyyy")
                        : (object)DBNull.Value);
                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }

        public bool InsertContractLink(ContractLink link)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_CONTRACT_LINK_INSERT";
                    com.Parameters.Add("RECORD_TYPE_IN", link.RecordType);
                    com.Parameters.Add("FI_CODE_IN", link.FICode);
                    com.Parameters.Add("BRANCH_CODE_IN", link.BranchCode);
                    com.Parameters.Add("TYPE_OF_LINK_IN", link.TypeOfLink);
                    com.Parameters.Add("FI_PRIMARY_CODE_IN", string.IsNullOrEmpty(link.FIPrimaryCode) ? (object)DBNull.Value : (object)link.FIPrimaryCode);
                    com.Parameters.Add("FI_SECONDARY_CODE_IN", link.FISecondaryCode);
                    com.Parameters.Add("FI_CONTRACT_CODE_IN", link.FIContractCode);
                    com.Parameters.Add("MAKE_BY_IN", link.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", link.MakeDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("RECORD_STATUS_IN", link.RecordStatus);

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }

        public InstalmentContract SelectInstalmentContract(string contractCode)
        {
            InstalmentContract contract = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_INSTALMENT_CONTRACT";
                    com.Parameters.Add("CONTRACT_CODE", contractCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                            contract = FillInstalmentContractRecord(dr);

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return contract;
        }

        private InstalmentContract FillInstalmentContractRecord(IDataRecord dr)
        {
            InstalmentContract contract = new InstalmentContract();
            var ord = 0;
            if (dr.TryGetOrdinal("RECORD_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.RecordType = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FICode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                contract.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BORROWER_NAME", out ord) && !dr.IsDBNull(ord))
                contract.BorrowerName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("UBS_ACCOUNT_NO", out ord) && !dr.IsDBNull(ord))
                contract.UbsAccountNo = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ContractType = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_PHASE", out ord) && !dr.IsDBNull(ord))
                contract.ContractPhase = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.ContractStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CURRENCY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.CurrencyCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("STARTING_DATE", out ord) && !dr.IsDBNull(ord))
                contract.StartingDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("REQUEST_DATE", out ord) && !dr.IsDBNull(ord))
                contract.RequestDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("PLANNED_END_DATE", out ord) && !dr.IsDBNull(ord))
                contract.PlannedEndDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("ACTUAL_END_DATE", out ord) && !dr.IsDBNull(ord))
                contract.ActualEndDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("DEFAULTER_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.DefaulterStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("LAST_PAYMENT_DATE", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLastPayment = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("SUBSIDIZED_CREDIT", out ord) && !dr.IsDBNull(ord))
                contract.FlagSubsidizedCredit = dr.GetString(ord);
            if (dr.TryGetOrdinal("PRE_FINANCE_LOAN", out ord) && !dr.IsDBNull(ord))
                contract.FlagPreFinanceOfLoan = dr.GetString(ord);
            if (dr.TryGetOrdinal("REORGANIZED_CREDIT", out ord) && !dr.IsDBNull(ord))
                contract.CodeReorganizedCredit = dr.GetString(ord);
            if (dr.TryGetOrdinal("THIRD_PARTY_GUARANTEE_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ThirdPartyGuaranteeType = dr.GetString(ord);
            if (dr.TryGetOrdinal("SECURITY_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.SecurityType = dr.GetString(ord);
            if (dr.TryGetOrdinal("AMOUNT_GURANTEED_THIRD_PARTY", out ord) && !dr.IsDBNull(ord))
                contract.AmountGuaranteedByThirdParty = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("AMOUNT_GUARANTEED_SECURITY", out ord) && !dr.IsDBNull(ord))
                contract.AmountGuaranteedBySecurityType = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("QUALIFICATION_JUDGEMENT", out ord) && !dr.IsDBNull(ord))
                contract.QualitativeJudgement = dr.GetString(ord);
            if (dr.TryGetOrdinal("SANCTION_LIMIT", out ord) && !dr.IsDBNull(ord))
                contract.SanctionLimit = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_DISBURSED_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalDisbursedAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_OUTSTANDING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalOutstandingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_NUMBER_INSTALMENTS", out ord) && !dr.IsDBNull(ord))
                contract.TotalNumberOfInstallments = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("PERIODICITY_PAYMENT", out ord) && !dr.IsDBNull(ord))
                contract.PeriodicityOfPayment = dr.GetString(ord);
            if (dr.TryGetOrdinal("METHOD_PAYMENT", out ord) && !dr.IsDBNull(ord))
                contract.MethodOfPayment = dr.GetString(ord);
            if (dr.TryGetOrdinal("INSTALMENT_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.InstalmentAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("EXPIRATION_DATE_NEXT_INST", out ord) && !dr.IsDBNull(ord))
                contract.ExpirationDateOfNextInstallment = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("AMOUNT_NEXT_EXPIRING_INST", out ord) && !dr.IsDBNull(ord))
                contract.AmountOfNextExpiringInstallment = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("NO_OF_REMAINING_INSTALMENT", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfRemainingInstallments = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("REMAINING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.RemainingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("NO_OF_OVERDUE_INSTALMENT", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfOverdueInstallment = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("OVERDUE_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.OverdueAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("NO_OF_DAYS_PAYMENT_DELAY", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfDaysOfPaymentDelay = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("TYPE_OF_LEASED_GOOD", out ord) && !dr.IsDBNull(ord))
                contract.TypeOfLeasedGood = dr.GetString(ord);
            if (dr.TryGetOrdinal("VALUE_OF_LEASED_GOOD", out ord) && !dr.IsDBNull(ord))
                contract.ValueOfLeasedGood = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("REGISTRATION_NUMBER", out ord) && !dr.IsDBNull(ord))
                contract.RegistrationNumber = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_OF_MANUFACTURING", out ord) && !dr.IsDBNull(ord))
                contract.DateOfManufacturing = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("DUE_FOR_RECOVERY", out ord) && !dr.IsDBNull(ord))
                contract.DueForRecovery = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("RECOVERY_DURING_REPORTING", out ord) && !dr.IsDBNull(ord))
                contract.RecoveryDuringTheReportingPeriod = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("CUMULATIVE_RECOVERY", out ord) && !dr.IsDBNull(ord))
                contract.CumulativeRecovery = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("DATE_OF_LAW_SUIT", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLawSuit = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("DATE_OF_CLASSIFICATION", out ord) && !dr.IsDBNull(ord))
                contract.DateOfClassification = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("NO_TIMES_RESCHEDULING", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfTimesRescheduling = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_LAST_RESCHEDULING", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLastRescheduling = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("ECONOMIC_PURPOSE_CODE", out ord) && !dr.IsDBNull(ord))
                contract.EconomicPurposeCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("SME", out ord) && !dr.IsDBNull(ord))
                contract.SME = dr.GetString(ord);
            if (dr.TryGetOrdinal("ENTERPRISE_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.EnterpriseType = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                contract.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                contract.MakeDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.RecordStatus = dr.GetString(ord);
            return contract;
        }

        public bool IsInInstalmentContract(string contractCode, DateTime reportingPerioddate)
        {
            string FIContractCode = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_INSTALMENT_CONTRACT_REPORT";
                    com.Parameters.Add("CONTRACT_CODE", contractCode);
                    com.Parameters.Add("REPORT_PERIOD", reportingPerioddate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            var ord = 0;
                            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                                FIContractCode = dr.GetString(ord);
                        }
                            
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            if (FIContractCode != null)
                return true;
            else
                return false;
        }
        public bool IsInNonInstalmentContract(string contractCode, DateTime reportingPerioddate)
        {
            string FIContractCode = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_NONINSTALMENT_CONTRACT_REPORT";
                    com.Parameters.Add("CONTRACT_CODE", contractCode);
                    com.Parameters.Add("REPORT_PERIOD", reportingPerioddate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            var ord = 0;
                            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                                FIContractCode = dr.GetString(ord);
                        }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            if (FIContractCode != null)
                return true;
            else
                return false;
        }

        public List<InstalmentContract> SelectInstalmentContractsWithFilters(string subjectCode, string cibBranchCode, string contractCode, string contractPhase, string ubsAcNo)
        {
            List<InstalmentContract> list = new List<InstalmentContract>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_INST_CONTRACTS_FILTERED";
                    com.Parameters.Add("SUBJECT_CODE_IN", string.IsNullOrEmpty(subjectCode) ? (object)DBNull.Value : (object)subjectCode);
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(cibBranchCode) ? (object)DBNull.Value : (object)cibBranchCode);
                    com.Parameters.Add("CONTRACT_CODE_IN", string.IsNullOrEmpty(contractCode) ? (object)DBNull.Value : (object)contractCode);
                    com.Parameters.Add("CONTRACT_PHASE_IN", string.IsNullOrEmpty(contractPhase) ? (object)DBNull.Value : (object)contractPhase);
                    com.Parameters.Add("UBS_AC_NO_IN", string.IsNullOrEmpty(ubsAcNo) ? (object)DBNull.Value : (object)ubsAcNo);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillInstalmentContractShortRecord(dr));

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private InstalmentContract FillInstalmentContractShortRecord(OracleDataReader dr)
        {
            InstalmentContract contract = new InstalmentContract();
            var ord = 0;

            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                contract.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BORROWER_NAME", out ord) && !dr.IsDBNull(ord))
                contract.BorrowerName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ContractType = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_PHASE", out ord) && !dr.IsDBNull(ord))
                contract.ContractPhase = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.ContractStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CURRENCY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.CurrencyCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("DEFAULTER_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.DefaulterStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("SANCTION_LIMIT", out ord) && !dr.IsDBNull(ord))
                contract.SanctionLimit = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_DISBURSED_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalDisbursedAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_OUTSTANDING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalOutstandingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_NUMBER_INSTALMENTS", out ord) && !dr.IsDBNull(ord))
                contract.TotalNumberOfInstallments = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("PERIODICITY_PAYMENT", out ord) && !dr.IsDBNull(ord))
                contract.PeriodicityOfPayment = dr.GetString(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.RecordStatus = dr.GetString(ord);
            return contract;
        }

        public NonInstalmentContract SelectNonInstalmentContract(string contractCode)
        {
            NonInstalmentContract contract = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_NON_INSTALMENT_CONTRACT";
                    com.Parameters.Add("CONTRACT_CODE", contractCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                            contract = FillNonInstalmentContractRecord(dr);

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return contract;
        }

        private NonInstalmentContract FillNonInstalmentContractRecord(OracleDataReader dr)
        {
            NonInstalmentContract contract = new NonInstalmentContract();
            var ord = 0;
            if (dr.TryGetOrdinal("RECORD_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.RecordType = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FICode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                contract.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BORROWER_NAME", out ord) && !dr.IsDBNull(ord))
                contract.BorrowerName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("UBS_ACCOUNT_NO", out ord) && !dr.IsDBNull(ord))
                contract.UbsAccountNo = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ContractType = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_PHASE", out ord) && !dr.IsDBNull(ord))
                contract.ContractPhase = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.ContractStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CURRENCY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.CurrencyCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("STARTING_DATE", out ord) && !dr.IsDBNull(ord))
                contract.StartingDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("REQUEST_DATE", out ord) && !dr.IsDBNull(ord))
                contract.RequestDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("PLANNED_END_DATE", out ord) && !dr.IsDBNull(ord))
                contract.PlannedEndDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("ACTUAL_END_DATE", out ord) && !dr.IsDBNull(ord))
                contract.ActualEndDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("DEFAULTER_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.DefaulterStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_OF_LAST_PAYMENT", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLastPayment = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("SUBSIDIZED_CREDIT", out ord) && !dr.IsDBNull(ord))
                contract.FlagSubsidizedCredit = dr.GetString(ord);
            if (dr.TryGetOrdinal("PRE_FINANCE_LOAN", out ord) && !dr.IsDBNull(ord))
                contract.FlagPreFinanceOfLoan = dr.GetString(ord);
            if (dr.TryGetOrdinal("REORGANIZED_CREDIT", out ord) && !dr.IsDBNull(ord))
                contract.CodeReorganizedCredit = dr.GetString(ord);
            if (dr.TryGetOrdinal("THIRD_PARTY_GUARANTEE_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ThirdPartyGuaranteeType = dr.GetString(ord);
            if (dr.TryGetOrdinal("SECURITY_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.SecurityType = dr.GetString(ord);
            if (dr.TryGetOrdinal("AMOUNT_GUARANTEED_THIRD_PARTY", out ord) && !dr.IsDBNull(ord))
                contract.AmountGuaranteedByThirdParty = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("AMOUNT_GUARANTEED_SECURITY", out ord) && !dr.IsDBNull(ord))
                contract.AmountGuaranteedBySecurityType = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("QUALITATIVE_JUDGEMENT", out ord) && !dr.IsDBNull(ord))
                contract.QualitativeJudgement = dr.GetString(ord);
            if (dr.TryGetOrdinal("SANCTION_LIMIT", out ord) && !dr.IsDBNull(ord))
                contract.SanctionLimit = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_OUTSTANDING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalOutstandingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("NO_OF_DAYS_PAYMENT_DELAY", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfDaysOfPaymentDelay = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("DUE_FOR_RECOVERY", out ord) && !dr.IsDBNull(ord))
                contract.DueForRecovery = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("RECOVERY_DURING_REPORTING", out ord) && !dr.IsDBNull(ord))
                contract.RecoveryDuringTheReportingPeriod = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("CUMULATIVE_RECOVERY", out ord) && !dr.IsDBNull(ord))
                contract.CumulativeRecovery = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("DATE_OF_LAW_SUIT", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLawSuit = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("DATE_OF_CLASSIFICATION", out ord) && !dr.IsDBNull(ord))
                contract.DateOfClassification = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("NO_OF_TIMES_RESCHEDULING", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfTimesRescheduling = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_LAST_RESCHEDULING", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLastRescheduling = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("ECONOMIC_PURPOSE_CODE", out ord) && !dr.IsDBNull(ord))
                contract.EconomicPurposeCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("SME", out ord) && !dr.IsDBNull(ord))
                contract.SME = dr.GetString(ord);
            if (dr.TryGetOrdinal("ENTERPRISE_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.EnterpriseType = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                contract.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                contract.MakeDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.RecordStatus = dr.GetString(ord);
            return contract;
        }

        public List<NonInstalmentContract> SelectNonInstalmentContractsWithFilters(string subjectCode, string cibBranchCode, string contractCode, string contractPhase, string ubsAcNo)
        {
            List<NonInstalmentContract> list = new List<NonInstalmentContract>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_NON_INST_CONTRACTS_FLTR";
                    com.Parameters.Add("SUBJECT_CODE_IN", string.IsNullOrEmpty(subjectCode) ? (object)DBNull.Value : (object)subjectCode);
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(cibBranchCode) ? (object)DBNull.Value : (object)cibBranchCode);
                    com.Parameters.Add("CONTRACT_CODE_IN", string.IsNullOrEmpty(contractCode) ? (object)DBNull.Value : (object)contractCode);
                    com.Parameters.Add("CONTRACT_PHASE_IN", string.IsNullOrEmpty(contractPhase) ? (object)DBNull.Value : (object)contractPhase);
                    com.Parameters.Add("UBS_AC_NO_IN", string.IsNullOrEmpty(ubsAcNo) ? (object)DBNull.Value : (object)ubsAcNo);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillNonInstalmentContractShortRecord(dr));

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private NonInstalmentContract FillNonInstalmentContractShortRecord(OracleDataReader dr)
        {
            NonInstalmentContract contract = new NonInstalmentContract();
            var ord = 0;

            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                contract.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BORROWER_NAME", out ord) && !dr.IsDBNull(ord))
                contract.BorrowerName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ContractType = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_PHASE", out ord) && !dr.IsDBNull(ord))
                contract.ContractPhase = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.ContractStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CURRENCY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.CurrencyCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("DEFAULTER_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.DefaulterStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("SANCTION_LIMIT", out ord) && !dr.IsDBNull(ord))
                contract.SanctionLimit = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_OUTSTANDING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalOutstandingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.RecordStatus = dr.GetString(ord);
            return contract;
        }

        public CardContract SelectCardContract(string contractCode)
        {
            CardContract contract = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_CARD_CONTRACT";
                    com.Parameters.Add("CONTRACT_CODE", contractCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                            contract = FillCardContractRecord(dr);

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return contract;
        }

        private CardContract FillCardContractRecord(IDataRecord dr)
        {
            CardContract contract = new CardContract();
            var ord = 0;
            if (dr.TryGetOrdinal("RECORD_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.RecordType = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FICode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                contract.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BORROWER_NAME", out ord) && !dr.IsDBNull(ord))
                contract.BorrowerName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ContractType = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_PHASE", out ord) && !dr.IsDBNull(ord))
                contract.ContractPhase = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.ContractStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CURRENCY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.CurrencyCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("STARTING_DATE", out ord) && !dr.IsDBNull(ord))
                contract.StartingDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("REQUEST_DATE", out ord) && !dr.IsDBNull(ord))
                contract.RequestDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("PLANNED_END_DATE", out ord) && !dr.IsDBNull(ord))
                contract.PlannedEndDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("ACTUAL_END_DATE", out ord) && !dr.IsDBNull(ord))
                contract.ActualEndDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("DEFAULTER_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.DefaulterStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_LAST_PAYMENT", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLastPayment = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("SUBSIDIZED_CREDIT", out ord) && !dr.IsDBNull(ord))
                contract.FlagSubsidizedCredit = dr.GetString(ord);
            if (dr.TryGetOrdinal("PRE_FINANCE_OF_LOAN", out ord) && !dr.IsDBNull(ord))
                contract.FlagPreFinanceOfLoan = dr.GetString(ord);
            if (dr.TryGetOrdinal("REORGANIZED_CREDIT", out ord) && !dr.IsDBNull(ord))
                contract.CodeReorganizedCredit = dr.GetString(ord);
            if (dr.TryGetOrdinal("THIRD_PARTY_GUARANTEE_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ThirdPartyGuaranteeType = dr.GetString(ord);
            if (dr.TryGetOrdinal("SECURITY_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.SecurityType = dr.GetString(ord);
            if (dr.TryGetOrdinal("AMOUNT_GUARANTEED_THIRD_PARTY", out ord) && !dr.IsDBNull(ord))
                contract.AmountGuaranteedByThirdParty = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("AMOUNT_GUARANTEED_SECURITY", out ord) && !dr.IsDBNull(ord))
                contract.AmountGuaranteedBySecurityType = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("QUALITATIVE_JUDGEMENT", out ord) && !dr.IsDBNull(ord))
                contract.QualitativeJudgement = dr.GetString(ord);
            if (dr.TryGetOrdinal("PERIODICITY_PAYMENT", out ord) && !dr.IsDBNull(ord))
                contract.PeriodicityOfPayment = dr.GetString(ord);
            if (dr.TryGetOrdinal("METHOD_OF_PAYMENT", out ord) && !dr.IsDBNull(ord))
                contract.MethodOfPayment = dr.GetString(ord);
            if (dr.TryGetOrdinal("INSTALMENT_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.InstalmentAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("CREDIT_LIMIT", out ord) && !dr.IsDBNull(ord))
                contract.CreditLimit = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_OUTSTANDING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalOutstandingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("EXPIRATION_DATE_NEXT_INST", out ord) && !dr.IsDBNull(ord))
                contract.ExpirationDateOfNextInstallment = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("REMAINING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.RemainingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("NUMBER_OF_OVERDUE", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfOverdueInstallment = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("OVERDUE_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.OverdueAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("DATE_OF_LAST_CHARGE", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLastCharge = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("TYPE_OF_INSTALMENT", out ord) && !dr.IsDBNull(ord))
                contract.TypeOfInstallment = dr.GetString(ord);
            if (dr.TryGetOrdinal("NO_OF_DAYS_PAYMENT_DELAY", out ord) && !dr.IsDBNull(ord))
                contract.NumberOfDaysOfPaymentDelay = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("DUE_FOR_RECOVERY", out ord) && !dr.IsDBNull(ord))
                contract.DueForRecovery = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("RECOVERY_DURING_REPORTING", out ord) && !dr.IsDBNull(ord))
                contract.RecoveryDuringTheReportingPeriod = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("CUMULATIVE_RECOVERY", out ord) && !dr.IsDBNull(ord))
                contract.CumulativeRecovery = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("DATE_OF_LAW_SUIT", out ord) && !dr.IsDBNull(ord))
                contract.DateOfLawSuit = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("ECONOMIC_PURPOSE_CODE", out ord) && !dr.IsDBNull(ord))
                contract.EconomicPurposeCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                contract.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                contract.MakeDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.RecordStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_OF_CLASSIFICATION", out ord) && !dr.IsDBNull(ord))
                contract.DateOfClassification = dr.GetDateTime(ord);
            return contract;
        }

        public List<CardContract> SelectCardContractsWithFilters(string subjectCode, string cibBranchCode, string contractCode, string contractPhase)
        {
            List<CardContract> list = new List<CardContract>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_CARD_CONTRACTS_FILTERED";
                    com.Parameters.Add("SUBJECT_CODE_IN", string.IsNullOrEmpty(subjectCode) ? (object)DBNull.Value : (object)subjectCode);
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(cibBranchCode) ? (object)DBNull.Value : (object)cibBranchCode);
                    com.Parameters.Add("CONTRACT_CODE_IN", string.IsNullOrEmpty(contractCode) ? (object)DBNull.Value : (object)contractCode);
                    com.Parameters.Add("CONTRACT_PHASE_IN", string.IsNullOrEmpty(contractPhase) ? (object)DBNull.Value : (object)contractPhase);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillCardContractShortRecord(dr));

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private CardContract FillCardContractShortRecord(OracleDataReader dr)
        {
            CardContract contract = new CardContract();
            var ord = 0;

            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                contract.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BORROWER_NAME", out ord) && !dr.IsDBNull(ord))
                contract.BorrowerName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                contract.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_TYPE", out ord) && !dr.IsDBNull(ord))
                contract.ContractType = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_PHASE", out ord) && !dr.IsDBNull(ord))
                contract.ContractPhase = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.ContractStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CURRENCY_CODE", out ord) && !dr.IsDBNull(ord))
                contract.CurrencyCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("DEFAULTER_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.DefaulterStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CREDIT_LIMIT", out ord) && !dr.IsDBNull(ord))
                contract.CreditLimit = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TOTAL_OUTSTANDING_AMOUNT", out ord) && !dr.IsDBNull(ord))
                contract.TotalOutstandingAmount = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("TYPE_OF_INSTALMENT", out ord) && !dr.IsDBNull(ord))
                contract.TypeOfInstallment = dr.GetString(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                contract.RecordStatus = dr.GetString(ord);
            return contract;
        }

        public List<ContractLink> SelectContractLinksFiltered(string branchCode, string primaryCode, string primaryName, string secondaryCode, string secondaryName, string contractCode)
        {
            List<ContractLink> list = new List<ContractLink>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_CONTRACT_LINKS_FILTERED";
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(branchCode) ? (object)DBNull.Value : (object)branchCode);
                    com.Parameters.Add("PRIMARY_CODE_IN", string.IsNullOrEmpty(primaryCode) ? (object)DBNull.Value : (object)primaryCode);
                    com.Parameters.Add("PRIMARY_NAME_IN", string.IsNullOrEmpty(primaryName) ? (object)DBNull.Value : (object)primaryName);
                    com.Parameters.Add("SECONDARY_CODE_IN", string.IsNullOrEmpty(secondaryCode) ? (object)DBNull.Value : (object)secondaryCode);
                    com.Parameters.Add("SECONDARY_NAME_IN", string.IsNullOrEmpty(secondaryName) ? (object)DBNull.Value : (object)secondaryName);
                    com.Parameters.Add("CONTRACT_CODE_IN", string.IsNullOrEmpty(contractCode) ? (object)DBNull.Value : (object)contractCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(FillContractLinksRecord(dr));
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private ContractLink FillContractLinksRecord(IDataRecord dr)
        {
            ContractLink link = new ContractLink();
            var ord = 0;
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                link.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("TYPE_OF_LINK", out ord) && !dr.IsDBNull(ord))
                link.TypeOfLink = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_PRIMARY_CODE", out ord) && !dr.IsDBNull(ord))
                link.FIPrimaryCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("PRIMARY_NAME", out ord) && !dr.IsDBNull(ord))
                link.FIPrimaryName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SECONDARY_CODE", out ord) && !dr.IsDBNull(ord))
                link.FISecondaryCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("SECONDARY_NAME", out ord) && !dr.IsDBNull(ord))
                link.FISecondaryName = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                link.FIContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                link.RecordStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CONTRACT_CATEGORY", out ord) && !dr.IsDBNull(ord))
                link.ContractCategory = dr.GetString(ord);
            return link;
        }

        public bool FlagDeleteCardContracts(List<Contract> contracts)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_CONTRACT.SP_CARD_CONTRACT_FLAG_DELETE";
                        com.Parameters.Add("FI_CONTRACT_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var contract in contracts)
                            {
                                com.Parameters["FI_CONTRACT_CODE_IN"].Value = contract.FIContractCode;
                                com.Parameters["MAKE_BY_IN"].Value = contract.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = contract.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true; ;
        }

        public bool FlagDeleteInstalmentContracts(List<Contract> contracts)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_CONTRACT.SP_INST_CONTRACT_FLAG_DELETE";
                        com.Parameters.Add("FI_CONTRACT_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var contract in contracts)
                            {
                                com.Parameters["FI_CONTRACT_CODE_IN"].Value = contract.FIContractCode;
                                com.Parameters["MAKE_BY_IN"].Value = contract.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = contract.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true; ;
        }

        public bool FlagDeleteNonInstalmentContracts(List<Contract> contracts)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_CONTRACT.SP_N_INST_CONTRACT_FLAG_DELETE";
                        com.Parameters.Add("FI_CONTRACT_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var contract in contracts)
                            {
                                com.Parameters["FI_CONTRACT_CODE_IN"].Value = contract.FIContractCode;
                                com.Parameters["MAKE_BY_IN"].Value = contract.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = contract.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true; ;
        }

        public bool FlagDeleteContractLinks(List<ContractLink> links)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_CONTRACT.SP_CONTRACT_LINK_FLAG_DELETE";
                        com.Parameters.Add("FI_CONTRACT_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("FI_SECONDARY_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var link in links)
                            {
                                com.Parameters["FI_CONTRACT_CODE_IN"].Value = link.FIContractCode;
                                com.Parameters["FI_SECONDARY_CODE_IN"].Value = link.FISecondaryCode;
                                com.Parameters["MAKE_BY_IN"].Value = link.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = link.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true; ;
        }

        public List<ContractError> SelectInstalmentContractError(string cibBranchCode, string conCode, string subCode)
        {
            List<ContractError> list = new List<ContractError>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ERROR.SP_GET_INST_CONTRACT_ERROR";
                    com.Parameters.Add("P_BRANCH_CODE", cibBranchCode);
                    com.Parameters.Add("P_FI_SUBJECT_CODE", subCode);
                    com.Parameters.Add("P_FI_CONTRACT_CODE", conCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillContractErrorRecord(dr));

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private ContractError FillContractErrorRecord(IDataRecord dr)
        {
            ContractError error = new ContractError();
            var ord = 0;
            if (dr.TryGetOrdinal("VERSION_NO", out ord) && !dr.IsDBNull(ord))
                error.VersionNo = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("REPORTING_PERIOD", out ord) && !dr.IsDBNull(ord))
                error.ReportingPeriod = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("FI_CONTRACT_CODE", out ord) && !dr.IsDBNull(ord))
                error.FiContractCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_PRIMARY_CODE", out ord) && !dr.IsDBNull(ord))
                error.FiPrimaryCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_PRIMARY_NAME", out ord) && !dr.IsDBNull(ord))
                error.FiPrimaryName = dr.GetString(ord);
            if (dr.TryGetOrdinal("ERROR_CODE", out ord) && !dr.IsDBNull(ord))
                error.ErrorCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("ERROR_DESCRIPTION", out ord) && !dr.IsDBNull(ord))
                error.ErrorDescription = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                error.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                error.MakeDate = dr.GetDateTime(ord);
            return error;
        }

        public List<ContractError> SelectNonInstalmentContractError(string cibBranchCode, string conCode, string subCode)
        {
            List<ContractError> list = new List<ContractError>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ERROR.SP_GET_NON_INST_CONTRACT_ERROR";
                    com.Parameters.Add("P_BRANCH_CODE", cibBranchCode);
                    com.Parameters.Add("P_FI_SUBJECT_CODE", subCode);
                    com.Parameters.Add("P_FI_CONTRACT_CODE", conCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillContractErrorRecord(dr));

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        public List<ContractError> SelectCardContractError(string cibBranchCode, string conCode, string subCode)
        {
            List<ContractError> list = new List<ContractError>();
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ERROR.SP_GET_CARD_CONTRACT_ERROR";
                    com.Parameters.Add("P_BRANCH_CODE", cibBranchCode);
                    com.Parameters.Add("P_FI_SUBJECT_CODE", subCode);
                    com.Parameters.Add("P_FI_CONTRACT_CODE", conCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillContractErrorRecord(dr));

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        internal bool SelectNonInstalmentContractExists(string conCode)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "SELECT 1 FROM NON_INSTALMENT_CONTRACTS WHERE FI_CONTRACT_CODE = :P_FI_CONTRACT_CODE";
                    com.Parameters.Add("P_FI_CONTRACT_CODE", conCode);

                    using (OracleDataReader dr = com.ExecuteReader())
                        return dr.HasRows;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
        }

        internal bool SelectInstalmentContractExists(string conCode)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "SELECT 1 FROM INSTALMENT_CONTRACTS WHERE FI_CONTRACT_CODE = :P_FI_CONTRACT_CODE";
                    com.Parameters.Add("P_FI_CONTRACT_CODE", conCode);

                    using (OracleDataReader dr = com.ExecuteReader())
                        return dr.HasRows;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
        }
    }


}