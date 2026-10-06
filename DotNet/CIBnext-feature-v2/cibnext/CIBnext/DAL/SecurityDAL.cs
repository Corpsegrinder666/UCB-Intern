using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CIBnext.DAO;
using NLog;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using CIBnext.LIB;
using System.Data.Common;

namespace CIBnext.DAL
{
    public class SecurityDAL
    {
        Logger _logger = LogManager.GetLogger("SecurityDAL");

        private const string SP_LAND_BUILDING_INSERT = "PKG_SECURITY.SP_LAND_BUILDING_INSERT";
        private const string SP_FLAT_INSERT = "PKG_SECURITY.SP_FLAT_INSERT";
        private const string SP_MACHINERY_INSERT = "PKG_SECURITY.SP_MACHINERY_INSERT";
        private const string SP_MORTGAGE_INSERT = "PKG_SECURITY.SP_MORTGAGE_INSERT";
        private const string SP_HYPOTHECATION_INSERT = "PKG_SECURITY.SP_HYPOTHECATION_INSERT";
        private const string SP_SECURITY_LINK_INSERT = "PKG_SECURITY.SP_SECURITY_LINK_INSERT";
        private const string SP_SUBJECT_LINK_INSERT = "PKG_SECURITY.SP_SUBJECT_LINK_INSERT";
        private const string SP_LAND_BUILDING_GET = "PKG_SECURITY.SP_LAND_BUILDING_GET";
        private const string SP_FLAT_GET = "PKG_SECURITY.SP_FLAT_GET";
        private const string SP_MACHINERY_GET = "PKG_SECURITY.SP_MACHINERY_GET";
        private const string SP_MORTGAGE_GET_FILTERED = "PKG_SECURITY.SP_MORTGAGE_GET_FILTERED";
        private const string SP_HYPOTHECATION_GET_FILTERED = "PKG_SECURITY.SP_HYPOTHECATION_GET_FILTERED";

        private IUnitOfWork unitOfWork = null;

        public SecurityDAL()
        {

        }

        public SecurityDAL(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public string Message { get; private set; }

        internal async Task<string> InsertLandBuilding(LandBuilding landBuilding)
        {
            OracleConnection con = null;
            IDbTransaction tran = null;
            if (this.unitOfWork != null)
            {
                con = unitOfWork.Connection as OracleConnection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(WebConfig.UcibConnectionString);
            try
            {
                OracleDynamicParameters dp = new OracleDynamicParameters();
                dp.Add("IN_RECORD_TYPE", landBuilding.RecordType);
                dp.Add("IN_FI_CODE", landBuilding.FiCode);
                dp.Add("IN_BRANCH_CODE", landBuilding.BranchCode);
                dp.Add("IN_SECURITY_VALUE_CODE", landBuilding.SecurityValueCode);
                dp.Add("IN_SECURITY_CATEGORY", landBuilding.SecurityCategory);
                dp.Add("IN_FI_SECURITY_CODE", landBuilding.FiSecurityCode);
                dp.Add("IN_TYPE_DEED", landBuilding.TypeDeed);
                dp.Add("IN_TITLE_DEED_POA_NO", landBuilding.TitleDeedPoaNo);
                dp.Add("IN_DATE_REGISTRATION", landBuilding.DateRegistration.ToString("ddMMyyyy"));
                dp.Add("IN_NAME_SUB_REGISTRY_OFFICE", landBuilding.NameSubRegistryOffice);
                dp.Add("IN_DISTRICT", landBuilding.District);
                dp.Add("IN_THANA", landBuilding.Thana);
                dp.Add("IN_MOUZA", landBuilding.Mouza);
                dp.Add("IN_AREA_LAND", landBuilding.AreaLand);
                dp.Add("IN_PLOT_NO", landBuilding.PlotNo);
                dp.Add("IN_HOLDING_NO", landBuilding.HoldingNo);
                dp.Add("IN_ADDRESS", landBuilding.Address);
                dp.Add("IN_JOTE_NO", landBuilding.JoteNo);
                dp.Add("IN_DAG_NO_CS", landBuilding.DagNoCs);
                dp.Add("IN_DAG_NO_SA", landBuilding.DagNoSa);
                dp.Add("IN_DAG_NO_RS", landBuilding.DagNoRs);
                dp.Add("IN_DAG_NO_BS", landBuilding.DagNoBs);
                dp.Add("IN_DAG_NO_CITY_JORIP", landBuilding.DagNoCityJorip);
                dp.Add("IN_KHATIAN_NO_CS", landBuilding.KhatianNoCs);
                dp.Add("IN_KHATIAN_NO_SA", landBuilding.KhatianNoSa);
                dp.Add("IN_KHATIAN_NO_RS", landBuilding.KhatianNoRs);
                dp.Add("IN_KHATIAN_NO_BS", landBuilding.KhatianNoBs);
                dp.Add("IN_KHATIAN_NO_CITY_JORIP", landBuilding.KhatianNoCityJorip);
                dp.Add("IN_MUTATION_KHATIAN_NO", landBuilding.MutationKhatianNo);
                dp.Add("IN_LEASEHOLD_PROPERTY", landBuilding.LeaseholdProperty);
                dp.Add("IN_BUILDING_EXISTS_IN_LAND", landBuilding.BuildingExistsInLand);
                dp.Add("IN_SIZE_AREA_BUILDING", landBuilding.SizeAreaBuilding);
                dp.Add("IN_NUMBER_FLOOR", landBuilding.NumberFloor);
                dp.Add("IN_MAKE_BY", landBuilding.MakeBy);
                dp.Add("IN_MAKE_DATE", landBuilding.MakeDate.ToString("dd-MMM-yyyy"));
                dp.Add("IN_CHECK_BY", (string)null);
                dp.Add("IN_CHECK_DATE", (DateTime?)null);
                dp.Add("IN_RECORD_STATUS", landBuilding.RecordStatus);
                dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                string code = await con.QueryFirstAsync<string>(SP_LAND_BUILDING_INSERT, dp, transaction: tran, commandType: CommandType.StoredProcedure);
                return code;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                if (unitOfWork != null) throw;
            }
            return null;
        }

        internal async Task<string> InsertFlat(Flat flat)
        {
            IDbConnection con = null;
            IDbTransaction tran = null;
            if (unitOfWork != null)
            {
                con = unitOfWork.Connection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(WebConfig.UcibConnectionString);
            try
            {
                OracleDynamicParameters dp = new OracleDynamicParameters();
                dp.Add("IN_RECORD_TYPE", flat.RecordType);
                dp.Add("IN_FI_CODE", flat.FiCode);
                dp.Add("IN_BRANCH_CODE", flat.BranchCode);
                dp.Add("IN_SECURITY_VALUE_CODE", flat.SecurityValueCode);
                dp.Add("IN_SECURITY_CATEGORY", flat.SecurityCategory);
                dp.Add("IN_FI_SECURITY_CODE", flat.FiSecurityCode);
                dp.Add("IN_TYPE_DEED", flat.TypeDeed);
                dp.Add("IN_TITLE_DEED_POA_NO", flat.TitleDeedPoaNo);
                dp.Add("IN_DATE_REGISTRATION", flat.DateRegistration.ToString("ddMMyyyy"));
                dp.Add("IN_NAME_SUB_REGISTRY_OFFICE", flat.NameSubRegistryOffice);
                dp.Add("IN_DISTRICT", flat.District);
                dp.Add("IN_THANA", flat.Thana);
                dp.Add("IN_MOUZA", flat.Mouza);
                dp.Add("IN_TRIPARTITE_AGREEMENT", flat.TripartiteAgreement);
                dp.Add("IN_AREA_FLAT", flat.AreaFlat);
                dp.Add("IN_UNDEMARCATED_LAND_AREA", flat.UndemarcatedTotalLandArea);
                dp.Add("IN_APARTMENT_FLAT_NO", flat.ApartmentFlatNo);
                dp.Add("IN_FLOOR_NO", flat.FloorNo);
                dp.Add("IN_LOCATION_FLAT", flat.LocationFlat);
                dp.Add("IN_CITY_CORPRATION_HOLDING_NO", flat.CityCorprationHoldingNo);
                dp.Add("IN_NAME_BUILDING", flat.NameBuilding);
                dp.Add("IN_NAME_PROJECT", flat.NameProject);
                dp.Add("IN_NAME_DEVELOPER", flat.NameDeveloper);
                dp.Add("IN_REHAB_MEMBER_NO_DEVELOPER", flat.RehabMemberNoDeveloper);
                dp.Add("IN_ADDRESS", flat.Address);
                dp.Add("IN_JOTE_NO", flat.JoteNo);
                dp.Add("IN_DAG_NO_CS", flat.DagNoCs);
                dp.Add("IN_DAG_NO_SA", flat.DagNoSa);
                dp.Add("IN_DAG_NO_RS", flat.DagNoRs);
                dp.Add("IN_DAG_NO_BS", flat.DagNoBs);
                dp.Add("IN_DAG_NO_CITY_JORIP", flat.DagNoCityJorip);
                dp.Add("IN_KHATIAN_NO_CS", flat.KhatianNoCs);
                dp.Add("IN_KHATIAN_NO_SA", flat.KhatianNoSa);
                dp.Add("IN_KHATIAN_NO_RS", flat.KhatianNoRs);
                dp.Add("IN_KHATIAN_NO_BS", flat.KhatianNoBs);
                dp.Add("IN_KHATIAN_NO_CITY_JORIP", flat.KhatianNoCityJorip);
                dp.Add("IN_MUTATION_KHATIAN_NO", flat.MutationKhatianNo);
                dp.Add("IN_LEASEHOLD_PROPERTY", flat.LeaseholdProperty);
                dp.Add("IN_MAKE_BY", flat.MakeBy);
                dp.Add("IN_MAKE_DATE", flat.MakeDate.ToString("dd-MMM-yyyy"));
                dp.Add("IN_CHECK_BY", (string)null);
                dp.Add("IN_CHECK_DATE", (DateTime?)null);
                dp.Add("IN_RECORD_STATUS", flat.RecordStatus);
                dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                string code = await con.QueryFirstAsync<string>(SP_FLAT_INSERT, dp, transaction: tran, commandType: CommandType.StoredProcedure);
                return code;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                if (unitOfWork != null) throw;
            }
            return null;
        }

        internal async Task<string> InsertMachinery(Machinery machinery)
        {
            IDbConnection con = null;
            IDbTransaction tran = null;
            if (unitOfWork != null)
            {
                con = unitOfWork.Connection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(WebConfig.UcibConnectionString);
            try
            {
                OracleDynamicParameters dp = new OracleDynamicParameters();
                dp.Add("IN_RECORD_TYPE", machinery.RecordType);
                dp.Add("IN_FI_CODE", machinery.FiCode);
                dp.Add("IN_BRANCH_CODE", machinery.BranchCode);
                dp.Add("IN_SECURITY_VALUE_CODE", machinery.SecurityValueCode);
                dp.Add("IN_SECURITY_CATEGORY", machinery.SecurityCategory);
                dp.Add("IN_FI_SECURITY_CODE", machinery.FiSecurityCode);
                dp.Add("IN_NAME_MACHINERY", machinery.NameMachinery);
                dp.Add("IN_NAME_FACTORY", machinery.NameFactory);
                dp.Add("IN_ADDRESS_FACTORY", machinery.AddressFactory);
                dp.Add("IN_MFG_CO_BRAND_NAME", machinery.MfgCoBrandName);
                dp.Add("IN_MFG_COUNTRY", machinery.MfgCountry);
                dp.Add("IN_MFG_YEAR", machinery.MfgYear);
                dp.Add("IN_MODEL_NO", machinery.ModelNo);
                dp.Add("IN_NO_UNIT", machinery.NoUnit);
                dp.Add("IN_LC_NO", machinery.LcNo);
                dp.Add("IN_LC_DATE", machinery.LcDate.ToString("ddMMyyyy"));
                dp.Add("IN_VALUE_LC", machinery.ValueLc);
                dp.Add("IN_LADING_AIR_WAY_BILL_NO", machinery.LadingAirWayBillNo);
                dp.Add("IN_PRESENT_VALUE", machinery.PresentValue);
                dp.Add("IN_BOOK_INVOICE_VALUE", machinery.BookInvoiceValue);
                dp.Add("IN_MAKE_BY", machinery.MakeBy);
                dp.Add("IN_MAKE_DATE", machinery.MakeDate.ToString("dd-MMM-yyyy"));
                dp.Add("IN_CHECK_BY", (string)null);
                dp.Add("IN_CHECK_DATE", (DateTime?)null);
                dp.Add("IN_RECORD_STATUS", machinery.RecordStatus);
                dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                string code = await con.QueryFirstAsync<string>(SP_MACHINERY_INSERT, dp, transaction: tran, commandType: CommandType.StoredProcedure);
                return code;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                if (unitOfWork != null) throw;
            }
            return null;
        }

        internal async Task<SecurityPayload> SelectPayload(string fiSecurityCode)
        {
            SecurityPayload sp = null;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_FI_SECURITY_CODE", fiSecurityCode);
                    dp.Add("PERSON_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    dp.Add("INSTITUTION_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    dp.Add("LAND_BUILDING_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    dp.Add("FLAT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    dp.Add("MACHINERY_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    dp.Add("MORTGAGE_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    dp.Add("HYPOTHECATION_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                    var multi = await con.QueryMultipleAsync("PKG_SECURITY.SP_SECURITY_PAYLOAD_GET", dp, commandType: CommandType.StoredProcedure);

                    sp = new SecurityPayload
                    {
                        Persons = multi.Read<PersonalData, Address, PersonalData>((person, address) =>
                            { person.PermanentAddress = address; return person; }, splitOn: "RecordStatus,Street").ToArray(),
                        Institutions = multi.Read<Institution, Address, Institution>((institution, address) => 
                            { institution.BusinessAddress = address; return institution; }, splitOn: "RecordStatus,Street").ToArray(),
                        LandBuildings = multi.Read<LandBuilding>().ToArray(),
                        Flats = multi.Read<Flat>().ToArray(),
                        Machineries = multi.Read<Machinery>().ToArray(),
                        Mortgages = multi.Read<Mortgage>().ToArray(),
                        Hypothecations = multi.Read<Hypothecation>().ToArray()
                    };
                }
            }
            catch (Exception ex)
            {
                sp = null;
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return sp;
        }

        internal async Task<List<Machinery>> SelectPagedMachineries(int pageIndex, int pageSize, string branchCode, string filterString, string filterBy)
        {
            List<Machinery> list = new List<Machinery>();
            try
            {
                int rowEnd = pageIndex * pageSize;
                int rowStart = (rowEnd - pageSize) + 1;
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_BRANCH_CODE", branchCode);
                    dp.Add("IN_FI_SECURITY_CODE", filterBy.Equals("SECURITY_CODE")? filterString : (string)null);
                    dp.Add("IN_SUBJECT_NAME", filterBy.Equals("SUBJECT_NAME")? filterString : (string)null);
                    dp.Add("IN_ROW_START", rowStart);
                    dp.Add("IN_ROW_END", rowEnd);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                    list = (await con.QueryAsync<Machinery>(SP_MACHINERY_GET, dp, commandType: CommandType.StoredProcedure)).AsList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return list;
        }

        internal async Task<List<Flat>> SelectPagedFlats(int pageIndex, int pageSize, string branchCode, string filterString, string filterBy)
        {
            List<Flat> list = new List<Flat>();
            try
            {
                int rowEnd = pageIndex * pageSize;
                int rowStart = (rowEnd - pageSize) + 1;
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_BRANCH_CODE", branchCode);
                    dp.Add("IN_FI_SECURITY_CODE", filterBy.Equals("SECURITY_CODE")? filterString : (string)null);
                    dp.Add("IN_SUBJECT_NAME", filterBy.Equals("SUBJECT_NAME")? filterString : (string)null);
                    dp.Add("IN_ROW_START", rowStart);
                    dp.Add("IN_ROW_END", rowEnd);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                    list = (await con.QueryAsync<Flat>(SP_FLAT_GET, dp, commandType: CommandType.StoredProcedure)).AsList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return list;
        }

        internal async Task<List<LandBuilding>> SelectPagedLandBuildings(int pageIndex, int pageSize, string branchCode, string filterString, string filterBy)
        {
            List<LandBuilding> list = new List<LandBuilding>();
            try
            {
                int rowEnd = pageIndex * pageSize;
                int rowStart = (rowEnd - pageSize) + 1;
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_BRANCH_CODE", branchCode);
                    dp.Add("IN_FI_SECURITY_CODE", filterBy.Equals("SECURITY_CODE")? filterString : (string)null);
                    dp.Add("IN_SUBJECT_NAME", filterBy.Equals("SUBJECT_NAME") ? filterString : (string)null);
                    dp.Add("IN_ROW_START", rowStart);
                    dp.Add("IN_ROW_END", rowEnd);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                    list = (await con.QueryAsync<LandBuilding>(SP_LAND_BUILDING_GET, dp, commandType: CommandType.StoredProcedure)).AsList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return list;
        }

        internal async Task<string> InsertMortgage(Mortgage mortgage)
        {
            IDbConnection con = null;
            IDbTransaction tran = null;
            if (unitOfWork != null)
            {
                con = unitOfWork.Connection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(WebConfig.UcibConnectionString);

            try
            {
                OracleDynamicParameters dp = new OracleDynamicParameters();
                dp.Add("IN_RECORD_TYPE", mortgage.RecordType);
                dp.Add("IN_FI_CODE", mortgage.FiCode);
                dp.Add("IN_BRANCH_CODE", mortgage.BranchCode);
                dp.Add("IN_FI_MORTGAGE_CODE", mortgage.FiMortgageCode);
                dp.Add("IN_MORTGAGE_TYPE", mortgage.MortgageType);
                dp.Add("IN_MORTGAGE_DEED_NO", mortgage.MortgageDeedNo);
                dp.Add("IN_MORTGAGE_DATE", mortgage.MortgageDate.ToString("ddMMyyyy"));
                dp.Add("IN_MORTGAGE_VALUE", mortgage.MortgageValue);
                dp.Add("IN_MARKET_VALUE", mortgage.MarketValue);
                dp.Add("IN_RJSC_FILLING_NO", mortgage.RjscFillingNo);
                dp.Add("IN_RJSC_FILLING_DATE", mortgage.RjscFillingDate == null ? (string)null : mortgage.RjscFillingDate.Value.ToString("ddMMyyyy"));
                dp.Add("IN_RANKING_CHARGE", mortgage.RankingCharge);
                dp.Add("IN_PARI_PASSU_CHARGE", mortgage.PariPassuCharge);
                dp.Add("IN_RIGPA_NO", mortgage.RigpaNo);
                dp.Add("IN_RIGPA_DATE", mortgage.RigpaDate == null ? (string)null : mortgage.RigpaDate.Value.ToString("ddMMyyyy"));
                dp.Add("IN_MORTGAGE_PHASE", mortgage.MortgagePhase);
                dp.Add("IN_SECURITY_CATEGORY", mortgage.SecurityCategory);
                dp.Add("IN_MORTGAGED_LAND_AREA", mortgage.MortgagedLandArea);
                dp.Add("IN_MAKE_BY", mortgage.MakeBy);
                dp.Add("IN_MAKE_DATE", mortgage.MakeDate.ToString("dd-MMM-yyyy"));
                dp.Add("IN_CHECK_BY", (string)null);
                dp.Add("IN_CHECK_DATE", (DateTime?)null);
                dp.Add("IN_RECORD_STATUS", mortgage.RecordStatus);
                dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                string code = await con.QueryFirstAsync<string>(SP_MORTGAGE_INSERT, dp, transaction: tran, commandType: CommandType.StoredProcedure);
                return code;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                if (unitOfWork != null) throw;
            }
            return null;
        }

        internal async Task<List<Hypothecation>> SelectFilteredHypothecations(string branchCode, string fiHypothecationCode, DateTime? hypothecationDate, string rjscFillingNo, DateTime? rjscFillingDate)
        {
            List<Hypothecation> hypos = null;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_BRANCH_CODE", string.IsNullOrEmpty(branchCode) ? (string)null : branchCode);
                    dp.Add("IN_FI_HYPOTHECATION_CODE", string.IsNullOrEmpty(fiHypothecationCode) ? (string)null : fiHypothecationCode);
                    dp.Add("IN_DATE_HYPOTHECATION", hypothecationDate == null ? (string)null : hypothecationDate.Value.ToString("ddMMyyyy"));
                    dp.Add("IN_RJSC_FILLING_NO", string.IsNullOrEmpty(rjscFillingNo) ? (string)null : rjscFillingNo);
                    dp.Add("IN_RJSC_FILLING_DATE", rjscFillingDate == null ? (string)null : rjscFillingDate.Value.ToString("ddMMyyyy"));
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                    hypos = (await con.QueryAsync<Hypothecation>(SP_HYPOTHECATION_GET_FILTERED, dp, commandType: CommandType.StoredProcedure)).AsList();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                return null;
            }
            return hypos;
        }

        internal async Task<List<Mortgage>> SelectFilteredMortgages(string branchCode, string fiMortgageCode, string mortgageDeedNo, DateTime? mortgageDate, decimal? mortgageValue)
        {
            List<Mortgage> mortgages = null;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_BRANCH_CODE", string.IsNullOrEmpty(branchCode) ? (string)null : branchCode);
                    dp.Add("IN_FI_MORTGAGE_CODE", string.IsNullOrEmpty(fiMortgageCode) ? (string)null : fiMortgageCode);
                    dp.Add("IN_MORTGAGE_DEED_NO", string.IsNullOrEmpty(mortgageDeedNo) ? (string)null : mortgageDeedNo);
                    dp.Add("IN_MORTGAGE_DATE", mortgageDate == null ? (string)null : mortgageDate.Value.ToString("ddMMyyyy"));
                    dp.Add("IN_MORTGAGE_VALUE", mortgageValue);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    mortgages = (await con.QueryAsync<Mortgage>(SP_MORTGAGE_GET_FILTERED, dp, commandType: CommandType.StoredProcedure)).AsList();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                return null;
            }
            return mortgages;
        }

        internal async Task<string> InsertHypothecation(Hypothecation hypothecation)
        {
            IDbConnection con = null;
            IDbTransaction tran = null;
            if (unitOfWork != null)
            {
                con = unitOfWork.Connection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(WebConfig.UcibConnectionString);
            try
            {
                OracleDynamicParameters dp = new OracleDynamicParameters();
                dp.Add("IN_RECORD_TYPE", hypothecation.RecordType);
                dp.Add("IN_FI_CODE", hypothecation.FiCode);
                dp.Add("IN_BRANCH_CODE", hypothecation.BranchCode);
                dp.Add("IN_FI_HYPOTHECATION_CODE", hypothecation.FiHypothecationCode);
                dp.Add("IN_DATE_HYPOTHECATION", hypothecation.DateHypothecation == null ? (string)null : hypothecation.DateHypothecation.Value.ToString("ddMMyyyy"));
                dp.Add("IN_RJSC_FILLING_NO", hypothecation.RjscFillingNo);
                dp.Add("IN_RJSC_FILLING_DATE", hypothecation.RjscFillingDate == null ? (string)null : hypothecation.RjscFillingDate.Value.ToString("ddMMyyyy"));
                dp.Add("IN_RANKING_CHARGE", hypothecation.RankingCharge);
                dp.Add("IN_PARI_PASSU_CHARGE", hypothecation.PariPassuCharge);
                dp.Add("IN_HYPOTHECATION_PHASE", hypothecation.HypothecationPhase);
                dp.Add("IN_MAKE_BY", hypothecation.MakeBy);
                dp.Add("IN_MAKE_DATE", hypothecation.MakeDate.ToString("dd-MMM-yyyy"));
                dp.Add("IN_CHECK_BY", (string)null);
                dp.Add("IN_CHECK_DATE", (DateTime?)null);
                dp.Add("IN_RECORD_STATUS", hypothecation.RecordStatus);
                dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                string code = await con.QueryFirstAsync<string>(SP_HYPOTHECATION_INSERT, dp, transaction: tran, commandType: CommandType.StoredProcedure);
                return code;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                if (unitOfWork != null) throw;
            }
            return null;
        }

        internal async Task<bool> InsertSecurityLink(string fiSecurityCode, string fiMortHypoCode, string branchCode, string linkType, string maker)
        {
            IDbConnection con = null;
            IDbTransaction tran = null;

            if (unitOfWork != null)
            {
                con = unitOfWork.Connection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(WebConfig.UcibConnectionString);
            try
            {
                OracleDynamicParameters dp = new OracleDynamicParameters();
                dp.Add("IN_RECORD_TYPE", "L");
                dp.Add("IN_FI_CODE", "046");
                dp.Add("IN_BRANCH_CODE", branchCode);
                dp.Add("IN_LINK_TYPE", linkType);
                dp.Add("IN_FI_SECURITY_CODE", fiSecurityCode);
                dp.Add("IN_FI_MORT_HYPO_CODE", fiMortHypoCode);
                dp.Add("IN_MAKE_BY", maker);
                dp.Add("IN_MAKE_DATE", DateTime.Now.ToString("dd-MMM-yyyy"));
                dp.Add("IN_CHECK_BY", (string)null);
                dp.Add("IN_CHECK_DATE", (DateTime?)null);
                dp.Add("IN_RECORD_STATUS", "C");
                await con.ExecuteAsync(SP_SECURITY_LINK_INSERT, dp, transaction: tran, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                if (unitOfWork != null) throw;
                return false;
            }
            return true;
        }

        internal async Task<bool> InsertSubjectLink(string cibBranchCode, string linkType, string fISubjectCode, string fiSecMortHypCode, string role, string maker)
        {
            IDbConnection con = null;
            IDbTransaction tran = null;
            if (unitOfWork != null)
            {
                con = unitOfWork.Connection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(WebConfig.UcibConnectionString);

            try
            {
                OracleDynamicParameters dp = new OracleDynamicParameters();
                dp.Add("IN_RECORD_TYPE", "R");
                dp.Add("IN_FI_CODE", "046");
                dp.Add("IN_BRANCH_CODE", cibBranchCode);
                dp.Add("IN_LINK_TYPE", linkType);
                dp.Add("IN_FI_SUBJECT_CODE", fISubjectCode);
                dp.Add("IN_FI_SEC_MORT_HYPO_CODE", fiSecMortHypCode);
                dp.Add("IN_ROLE", role);
                dp.Add("IN_MAKE_BY", maker);
                dp.Add("IN_MAKE_DATE", DateTime.Now.ToString("dd-MMM-yyyy"));
                dp.Add("IN_CHECK_BY", (string)null);
                dp.Add("IN_CHECK_DATE", (DateTime?)null);
                dp.Add("IN_RECORD_STATUS", "C");

                await con.ExecuteAsync(SP_SUBJECT_LINK_INSERT, dp, transaction: tran, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                if (unitOfWork != null) throw;
                return false;
            }
            return true;
        }
    }
}