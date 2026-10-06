using CIBnext.DAO;
using CIBnext.LIB;
using Dapper;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CIBnext.DAL
{
    public class SubjectDAL
    {
        private Logger _logger = LogManager.GetLogger("SubjecDAL");
        private IUnitOfWork unitOfWork;
        private readonly IConfiguration _configuration;
       

        public string Message { get; private set; }

        public SubjectDAL(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public SubjectDAL(IConfiguration configuration) {
            _configuration = configuration;

        }

        public string InsertPersonalData(DAO.PersonalData personal)
        {
            IDbConnection con = null;
            IDbTransaction tran = null;
            if (unitOfWork != null)
            {
                con = unitOfWork.Connection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(_configuration.GetConnectionString("OracleConnection"));

            string fiSubCode = string.Empty;
            try
            {
                using (OracleCommand com = con.CreateCommand() as OracleCommand)
                {
                    if (tran == null) con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_PERSON_INSERT";
                    com.Parameters.Add("IN_RECORD_TYPE", personal.RecordType);
                    com.Parameters.Add("IN_FI_CODE", personal.FICode);
                    com.Parameters.Add("IN_BRANCH_CODE", personal.BranchCode);
                    com.Parameters.Add("IN_FI_SUBJECT_CODE", string.IsNullOrEmpty(personal.FISubjectCode) ? (object)DBNull.Value : personal.FISubjectCode);
                    com.Parameters.Add("IN_TITLE", personal.Title);
                    com.Parameters.Add("IN_NAME", personal.Name);
                    com.Parameters.Add("IN_FATHERS_TITLE", personal.FathersTitle);
                    com.Parameters.Add("IN_FATHERS_NAME", personal.FathersName ?? "");
                    com.Parameters.Add("IN_MOTHERS_TITLE", personal.MothersTitle);
                    com.Parameters.Add("IN_MOTHERS_NAME", personal.MothersName ?? "");
                    com.Parameters.Add("IN_SPOUSES_TITLE", personal.SpousesTitle);
                    com.Parameters.Add("IN_SPOUSES_NAME", personal.SpousesName);
                    if (personal.SectorType == 0)
                        com.Parameters.Add("IN_SECTOR_TYPE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_SECTOR_TYPE", personal.SectorType);
                    if (personal.SectorCode == 0)
                        com.Parameters.Add("IN_SECTOR_CODE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_SECTOR_CODE", personal.SectorCode);
                    com.Parameters.Add("IN_GENDER", personal.Gender);
                    com.Parameters.Add("IN_DATE_OF_BIRTH", personal.DateOfBirth.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_PLACE_OF_BIRTH", personal.PlaceOfBirth);
                    com.Parameters.Add("IN_COUNTRY_OF_BIRTH", personal.CountryOfBirth);
                    com.Parameters.Add("IN_NATIONAL_ID_NUMBER", personal.NationalIDNumber);
                    com.Parameters.Add("IN_NATIONAL_ID_AVAILABLE", personal.NationalIDAvailable);
                    com.Parameters.Add("IN_TIN", personal.TIN);
                    com.Parameters.Add("IN_PERMANENT_ADDRESS_STREET", personal.PermanentAddress.Street ?? "");
                    if (personal.PermanentAddress.PostalCode == 0)
                        com.Parameters.Add("IN_PERMANENT_ADDRESS_POSTAL_CD", DBNull.Value);
                    else
                        com.Parameters.Add("IN_PERMANENT_ADDRESS_POSTAL_CD", personal.PermanentAddress.PostalCode);
                    com.Parameters.Add("IN_PERMANENT_ADDRESS_DISTRICT", personal.PermanentAddress.District ?? "");
                    com.Parameters.Add("IN_PERMANENT_ADDRESS_COUNTRY", personal.PermanentAddress.CountryCode ?? "");

                    if (personal.PresentAddress != null)
                        com.Parameters.Add("IN_PRESENT_ADDRESS_STREET", personal.PresentAddress.Street);
                    else
                        com.Parameters.Add("IN_PRESENT_ADDRESS_STREET", DBNull.Value);
                    if (personal.PresentAddress == null || personal.PresentAddress.PostalCode == 0)
                        com.Parameters.Add("IN_PRESENT_ADDRESS_POSTAL_CODE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_PRESENT_ADDRESS_POSTAL_CODE", personal.PresentAddress.PostalCode);
                    if (personal.PresentAddress != null)
                        com.Parameters.Add("IN_PRESENT_ADDRESS_DISTRICT", personal.PresentAddress.District);
                    else
                        com.Parameters.Add("IN_PRESENT_ADDRESS_DISTRICT", DBNull.Value);
                    if (personal.PresentAddress != null)
                        com.Parameters.Add("IN_PRESENT_ADDRESS_COUNTRY", personal.PresentAddress.CountryCode);
                    else
                        com.Parameters.Add("IN_PRESENT_ADDRESS_COUNTRY", DBNull.Value);

                    if (personal.BusinessAddress != null)
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_STREET", personal.BusinessAddress.Street);
                    else
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_STREET", DBNull.Value);
                    if (personal.BusinessAddress == null || personal.BusinessAddress.PostalCode == 0)
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_POSTAL_CD", DBNull.Value);
                    else
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_POSTAL_CD", personal.BusinessAddress.PostalCode);
                    if (personal.BusinessAddress != null)
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_DISTRICT", personal.BusinessAddress.District);
                    else
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_DISTRICT", DBNull.Value);
                    if (personal.BusinessAddress != null)
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_COUNTRY", personal.BusinessAddress.CountryCode);
                    else
                        com.Parameters.Add("IN_BUSINESS_ADDRESS_COUNTRY", DBNull.Value);

                    com.Parameters.Add("IN_ID_TYPE", personal.IDType);
                    com.Parameters.Add("IN_ID_NUMBER", personal.IDNumber);
                    if (personal.IDIssueDate == default(DateTime))
                        com.Parameters.Add("IN_ID_ISSUE_DATE", DBNull.Value);
                    else
                        com.Parameters.Add("IN_ID_ISSUE_DATE", personal.IDIssueDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_ID_ISSUE_COUNTRY", personal.IDIssueCountryCode);
                    com.Parameters.Add("IN_PHONE_NUMBER", personal.PhoneNumber);
                    com.Parameters.Add("IN_MAKE_BY", personal.MakeBy);
                    com.Parameters.Add("IN_MAKE_DATE", personal.MakeDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("IN_RECORD_STATUS", personal.RecordStatus);
                    com.Parameters.Add("IN_CIF_NO", personal.CifNo != null ? personal.CifNo : (object)DBNull.Value);
                    com.Parameters.Add("IN_SMART_ID_NUMBER", personal.CifNo != null ? personal.SmartIDNumber : (object)DBNull.Value);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                            fiSubCode = dr.GetString(0);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex.ToString());
                if (unitOfWork != null)
                    throw;
            }
            finally
            {
                if (unitOfWork == null && con != null)
                    con.Dispose();
            }
            return fiSubCode;
        }

        public async Task<Tuple<PersonalData, Institution>> SelectSubjectByCifNo(string cifNo)
        {
            Tuple<PersonalData, Institution> tp = null;
            PersonalData person = null;
            Institution institution = null;
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("CIF_NO_IN", cifNo);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    var multip = await con.QueryMultipleAsync("PKG_SUBJECT.SP_GET_PERSON_BY_CIF_NO", dp, commandType: CommandType.StoredProcedure);
                    person = multip.Read<PersonalData, Address, Address, Address, PersonalData>((lperson, permanent, present, business) =>
                    {
                        lperson.PermanentAddress = permanent;
                        lperson.PresentAddress = present;
                        lperson.BusinessAddress = business;
                        return lperson;
                    }, splitOn: "Street,Street,Street").FirstOrDefault();
                    var multii = await con.QueryMultipleAsync("PKG_SUBJECT.SP_GET_INSTITUTION_BY_CIF_NO", dp, commandType: CommandType.StoredProcedure);
                    institution = multii.Read<Institution, Address, Address, Institution>((linstitution, business, factory) => {
                        linstitution.BusinessAddress = business;
                        linstitution.FactoryAddress = factory;
                        return linstitution;
                    }, splitOn: "Street,Street").FirstOrDefault();

                    if (person != null || institution != null)
                        tp = new Tuple<PersonalData, Institution>(person, institution);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return null;
            }
            return tp;
        }

        public string InsertInstitution(DAO.Institution institution)
        {
            OracleConnection con = null;
            IDbTransaction tran = null;
            if (unitOfWork != null)
            {
                con = unitOfWork.Connection as OracleConnection;
                tran = unitOfWork.Transaction;
            }
            else
                con = new OracleConnection(_configuration.GetConnectionString("OracleConnection"));

            string fiSubCode = string.Empty;
            try
            {
                using (OracleCommand com = con.CreateCommand())
                {
                    if (tran == null) con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_INSTITUTION_INSERT";
                    com.Parameters.Add("RECORD_TYPE_IN", institution.RecordType);
                    com.Parameters.Add("FI_CODE_IN", institution.FICode);
                    com.Parameters.Add("BRANCH_CODE_IN", institution.BranchCode);
                    com.Parameters.Add("FI_SUBJECT_CODE_IN", string.IsNullOrEmpty(institution.FISubjectCode) ? (object)DBNull.Value : (object)institution.FISubjectCode);
                    com.Parameters.Add("TITLE_IN", institution.Title);
                    com.Parameters.Add("TRADE_NAME_IN", institution.TradeName);
                    com.Parameters.Add("SECTOR_TYPE_IN", institution.SectorType == 0 ? (object)DBNull.Value : (object)institution.SectorType);
                    com.Parameters.Add("SECTOR_CODE_IN", institution.SectorCode == 0 ? (object)DBNull.Value : (object)institution.SectorCode);
                    com.Parameters.Add("LEGAL_FORM_IN", institution.LegalForm);
                    com.Parameters.Add("REGISTRATION_NUMBER_RJSC_IN", institution.RegistrationNumberRJSC);
                    if (institution.RegistrationDateRJSC == default(DateTime))
                        com.Parameters.Add("REGISTRATION_DATE_RJSC_IN", DBNull.Value);
                    else
                        com.Parameters.Add("REGISTRATION_DATE_RJSC_IN", institution.RegistrationDateRJSC.ToString("dd MMM yyyy"));
                    com.Parameters.Add("TIN_IN", institution.TIN);

                    com.Parameters.Add("BUSINESS_ADDRESS_STREET_IN", institution.BusinessAddress.Street ?? "");
                    if (institution.BusinessAddress.PostalCode == 0)
                        com.Parameters.Add("BUSINESS_ADDRESS_POSTAL_CD_IN", DBNull.Value);
                    else
                        com.Parameters.Add("BUSINESS_ADDRESS_POSTAL_CD_IN", institution.BusinessAddress.PostalCode);
                    com.Parameters.Add("BUSINESS_ADDRESS_DISTRICT_IN", institution.BusinessAddress.District ?? "");
                    com.Parameters.Add("BUSINESS_ADDRESS_COUNTRY_IN", institution.BusinessAddress.CountryCode ?? "");

                    if (institution.FactoryAddress != null)
                        com.Parameters.Add("FACTORY_ADDRESS_STREET_IN", institution.FactoryAddress.Street);
                    else
                        com.Parameters.Add("FACTORY_ADDRESS_STREET_IN", DBNull.Value);
                    if (institution.FactoryAddress == null || institution.FactoryAddress.PostalCode == 0)
                        com.Parameters.Add("FACTORY_ADDRESS_POSTAL_CODE_IN", DBNull.Value);
                    else
                        com.Parameters.Add("FACTORY_ADDRESS_POSTAL_CODE_IN", institution.FactoryAddress.PostalCode);
                    if (institution.FactoryAddress != null)
                        com.Parameters.Add("FACTORY_ADDRESS_DISTRICT_IN", institution.FactoryAddress.District);
                    else
                        com.Parameters.Add("FACTORY_ADDRESS_DISTRICT_IN", DBNull.Value);
                    if (institution.FactoryAddress != null)
                        com.Parameters.Add("FACTORY_ADDRESS_COUNTRY_IN", institution.FactoryAddress.CountryCode);
                    else
                        com.Parameters.Add("FACTORY_ADDRESS_COUNTRY_IN", DBNull.Value);

                    com.Parameters.Add("CRG_SCORING_IN", institution.CRGScoring);
                    com.Parameters.Add("CREDIT_RATING_IN", institution.CreditRating);
                    com.Parameters.Add("PHONE_NUMBER_IN", institution.PhoneNumber);
                    com.Parameters.Add("MAKE_BY_IN", institution.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", institution.MakeDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("RECORD_STATUS_IN", institution.RecordStatus);
                    com.Parameters.Add("IN_CIF_NO", institution.CifNo != null ? institution.CifNo : (object)DBNull.Value);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                            fiSubCode = dr.GetString(0);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex.ToString());
                if (unitOfWork != null) throw;
            }
            finally
            {
                if (tran == null && con != null && con.State == ConnectionState.Open)
                    con.Close();
            }
            return fiSubCode;
        }

        public Institution SelectInstitution(string subCode)
        {
            Institution institution = null;
            try
            {
                using (OracleConnection con =
     new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_GET_INSTITUTION";
                    com.Parameters.Add("FI_SUBJECT_CODE_IN", subCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                            institution = FillInstitutionRecord(dr);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return institution;
        }

        private Institution FillInstitutionRecord(IDataRecord dr)
        {
            Institution institution = new Institution();
            var ord = 0;
            if (dr.TryGetOrdinal("RECORD_TYPE", out ord) && !dr.IsDBNull(ord))
                institution.RecordType = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CODE", out ord) && !dr.IsDBNull(ord))
                institution.FICode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                institution.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                institution.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("TITLE", out ord) && !dr.IsDBNull(ord))
                institution.Title = dr.GetString(ord);
            if (dr.TryGetOrdinal("TRADE_NAME", out ord) && !dr.IsDBNull(ord))
                institution.TradeName = dr.GetString(ord);
            if (dr.TryGetOrdinal("SECTOR_TYPE", out ord) && !dr.IsDBNull(ord))
                institution.SectorType = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("SECTOR_CODE", out ord) && !dr.IsDBNull(ord))
                institution.SectorCode = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("LEGAL_FORM", out ord) && !dr.IsDBNull(ord))
                institution.LegalForm = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("REGISTRATION_NUMBER_RJSC", out ord) && !dr.IsDBNull(ord))
                institution.RegistrationNumberRJSC = dr.GetString(ord);
            if (dr.TryGetOrdinal("REGISTRATION_DATE_RJSC", out ord) && !dr.IsDBNull(ord))
                institution.RegistrationDateRJSC = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("TIN", out ord) && !dr.IsDBNull(ord))
                institution.TIN = dr.GetString(ord);
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_STREET", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.BusinessAddress == null) institution.BusinessAddress = new Address();
                institution.BusinessAddress.Street = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_POSTAL_CODE", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.BusinessAddress == null) institution.BusinessAddress = new Address();
                institution.BusinessAddress.PostalCode = dr.GetInt32(ord);
            }
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_DISTRICT", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.BusinessAddress == null) institution.BusinessAddress = new Address();
                institution.BusinessAddress.District = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_COUNTRY", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.BusinessAddress == null) institution.BusinessAddress = new Address();
                institution.BusinessAddress.CountryCode = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("FACTORY_ADDRESS_STREET", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.FactoryAddress == null) institution.FactoryAddress = new Address();
                institution.FactoryAddress.Street = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("FACTORY_ADDRESS_POSTAL_CODE", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.FactoryAddress == null) institution.FactoryAddress = new Address();
                institution.FactoryAddress.PostalCode = dr.GetInt32(ord);
            }
            if (dr.TryGetOrdinal("FACTORY_ADDRESS_DISTRICT", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.FactoryAddress == null) institution.FactoryAddress = new Address();
                institution.FactoryAddress.District = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("FACTORY_ADDRESS_COUNTRY", out ord) && !dr.IsDBNull(ord))
            {
                if (institution.FactoryAddress == null) institution.FactoryAddress = new Address();
                institution.FactoryAddress.CountryCode = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("CRG_SCORING", out ord) && !dr.IsDBNull(ord))
                institution.CRGScoring = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("CREDIT_RATING", out ord) && !dr.IsDBNull(ord))
                institution.CreditRating = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("PHONE_NUMBER", out ord) && !dr.IsDBNull(ord))
                institution.PhoneNumber = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                institution.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                institution.MakeDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                institution.RecordStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CIF_NO", out ord) && !dr.IsDBNull(ord))
                institution.CifNo = dr.GetString(ord);
            return institution;
        }

        public PersonalData SelectPerson(string subCode)
        {
            PersonalData person = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_GET_PERSON";
                    com.Parameters.Add("FI_SUBJECT_CODE_IN", subCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                            person = FillPersonRecord(dr);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return person;
        }

        private PersonalData FillPersonRecord(IDataRecord dr)
        {
            PersonalData person = new PersonalData();
            var ord = 0;
            if (dr.TryGetOrdinal("RECORD_TYPE", out ord) && !dr.IsDBNull(ord))
                person.RecordType = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CODE", out ord) && !dr.IsDBNull(ord))
                person.FICode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                person.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                person.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("TITLE", out ord) && !dr.IsDBNull(ord))
                person.Title = dr.GetString(ord);
            if (dr.TryGetOrdinal("NAME", out ord) && !dr.IsDBNull(ord))
                person.Name = dr.GetString(ord);
            if (dr.TryGetOrdinal("FATHERS_TITLE", out ord) && !dr.IsDBNull(ord))
                person.FathersTitle = dr.GetString(ord);
            if (dr.TryGetOrdinal("FATHERS_NAME", out ord) && !dr.IsDBNull(ord))
                person.FathersName = dr.GetString(ord);
            if (dr.TryGetOrdinal("MOTHERS_TITLE", out ord) && !dr.IsDBNull(ord))
                person.MothersTitle = dr.GetString(ord);
            if (dr.TryGetOrdinal("MOTHERS_NAME", out ord) && !dr.IsDBNull(ord))
                person.MothersName = dr.GetString(ord);
            if (dr.TryGetOrdinal("SPOUSES_TITLE", out ord) && !dr.IsDBNull(ord))
                person.SpousesTitle = dr.GetString(ord);
            if (dr.TryGetOrdinal("SPOUSES_NAME", out ord) && !dr.IsDBNull(ord))
                person.SpousesName = dr.GetString(ord);
            if (dr.TryGetOrdinal("SECTOR_TYPE", out ord) && !dr.IsDBNull(ord))
                person.SectorType = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("SECTOR_CODE", out ord) && !dr.IsDBNull(ord))
                person.SectorCode = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("GENDER", out ord) && !dr.IsDBNull(ord))
                person.Gender = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_OF_BIRTH", out ord) && !dr.IsDBNull(ord))
                person.DateOfBirth = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("PLACE_OF_BIRTH", out ord) && !dr.IsDBNull(ord))
                person.PlaceOfBirth = dr.GetString(ord);
            if (dr.TryGetOrdinal("COUNTRY_OF_BIRTH", out ord) && !dr.IsDBNull(ord))
                person.CountryOfBirth = dr.GetString(ord);
            if (dr.TryGetOrdinal("NATIONAL_ID_NUMBER", out ord) && !dr.IsDBNull(ord))
                person.NationalIDNumber = dr.GetString(ord);
            if (dr.TryGetOrdinal("NATIONAL_ID_AVAILABLE", out ord) && !dr.IsDBNull(ord))
                person.NationalIDAvailable = dr.GetInt32(ord);
            if (dr.TryGetOrdinal("TIN", out ord) && !dr.IsDBNull(ord))
                person.TIN = dr.GetString(ord);
            if (dr.TryGetOrdinal("PERMANENT_ADDRESS_STREET", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PermanentAddress == null) person.PermanentAddress = new Address();
                person.PermanentAddress.Street = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("PERMANENT_ADDRESS_POSTAL_CODE", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PermanentAddress == null) person.PermanentAddress = new Address();
                person.PermanentAddress.PostalCode = dr.GetInt32(ord);
            }
            if (dr.TryGetOrdinal("PERMANENT_ADDRESS_DISTRICT", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PermanentAddress == null) person.PermanentAddress = new Address();
                person.PermanentAddress.District = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("PERMANENT_ADDRESS_COUNTRY", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PermanentAddress == null) person.PermanentAddress = new Address();
                person.PermanentAddress.CountryCode = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("PRESENT_ADDRESS_STREET", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PresentAddress == null) person.PresentAddress = new Address();
                person.PresentAddress.Street = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("PRESENT_ADDRESS_POSTAL_CODE", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PresentAddress == null) person.PresentAddress = new Address();
                person.PresentAddress.PostalCode = dr.GetInt32(ord);
            }
            if (dr.TryGetOrdinal("PRESENT_ADDRESS_DISTRICT", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PresentAddress == null) person.PresentAddress = new Address();
                person.PresentAddress.District = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("PRESENT_ADDRESS_COUNTRY", out ord) && !dr.IsDBNull(ord))
            {
                if (person.PresentAddress == null) person.PresentAddress = new Address();
                person.PresentAddress.CountryCode = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_STREET", out ord) && !dr.IsDBNull(ord))
            {
                if (person.BusinessAddress == null) person.BusinessAddress = new Address();
                person.BusinessAddress.Street = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_POSTAL_CODE", out ord) && !dr.IsDBNull(ord))
            {
                if (person.BusinessAddress == null) person.BusinessAddress = new Address();
                person.BusinessAddress.PostalCode = dr.GetInt32(ord);
            }
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_DISTRICT", out ord) && !dr.IsDBNull(ord))
            {
                if (person.BusinessAddress == null) person.BusinessAddress = new Address();
                person.BusinessAddress.District = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_COUNTRY", out ord) && !dr.IsDBNull(ord))
            {
                if (person.BusinessAddress == null) person.BusinessAddress = new Address();
                person.BusinessAddress.CountryCode = dr.GetString(ord);
            }
            if (dr.TryGetOrdinal("ID_TYPE", out ord) && !dr.IsDBNull(ord))
                person.IDType = dr.GetString(ord);
            if (dr.TryGetOrdinal("ID_NUMBER", out ord) && !dr.IsDBNull(ord))
                person.IDNumber = dr.GetString(ord);
            if (dr.TryGetOrdinal("ID_ISSUE_DATE", out ord) && !dr.IsDBNull(ord))
                person.IDIssueDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("ID_ISSUE_COUNTRY", out ord) && !dr.IsDBNull(ord))
                person.IDIssueCountryCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("PHONE_NUMBER", out ord) && !dr.IsDBNull(ord))
                person.PhoneNumber = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                person.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                person.MakeDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                person.RecordStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CIF_NO", out ord) && !dr.IsDBNull(ord))
                person.CifNo = dr.GetString(ord);
            if (dr.TryGetOrdinal("SMART_ID_NUMBER", out ord) && !dr.IsDBNull(ord))
                person.SmartIDNumber = dr.GetString(ord);
            return person;
        }


        public bool InsertOwnerLink(OwnerLink link)
        {
            bool success = false;
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_OWNER_LINK_INSERT";
                    com.Parameters.Add("RECORD_TYPE_IN", link.RecordType);
                    com.Parameters.Add("FI_CODE_IN", link.FiCode);
                    com.Parameters.Add("BRANCH_CODE_IN", link.CibBranchCode);
                    com.Parameters.Add("INSTITUTION_FI_SUBJECT_CODE_IN", link.InstitutionFiSubjectCode);
                    com.Parameters.Add("ROLE_IN", link.Role);
                    com.Parameters.Add("OWNER_FI_SUBJECT_CODE_IN", link.OwnerFiSubjectCode);
                    com.Parameters.Add("MAKE_BY_IN", link.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", link.MakeDate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("RECORD_STATUS_IN", link.RecordStatus);
                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return success;
        }

        public Subject SelectSubjectByContractCode(string contractCode)
        {
            Subject subject = new Subject();
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_GET_SUBJECT_BY_CONTRACT";
                    com.Parameters.Add("FI_CONTRACT_CODE", contractCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                        {
                            var subCode = dr["FI_SUBJECT_CODE"].ToString();
                            if (subCode.StartsWith("P"))
                                subject.Person = FillPersonRecord(dr);
                            else
                                subject.Institution = FillInstitutionRecord(dr);
                        }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return subject;
        }

        public List<PersonalData> SelectSubjectsByFilters(string cibBranchCode, string subjectCode, string subjectName, string fathersName, DateTime dob, string cifNo)
        {
            List<PersonalData> list = new List<PersonalData>();
            try
            {
                using (OracleConnection con =
     new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_GET_SUBJECTS_BY_FILTERS";
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(cibBranchCode) ? (object)DBNull.Value : (object)cibBranchCode);
                    com.Parameters.Add("FI_SUBJECT_CODE_IN", string.IsNullOrEmpty(subjectCode) ? (object)DBNull.Value : (object)subjectCode);
                    com.Parameters.Add("FI_SUBJECT_NAME_IN", string.IsNullOrEmpty(subjectName) ? (object)DBNull.Value : (object)subjectName);
                    com.Parameters.Add("FATHERS_NAME_IN", string.IsNullOrEmpty(fathersName) ? (object)DBNull.Value : (object)fathersName);
                    com.Parameters.Add("DATE_OF_BIRTH_IN", dob == default(DateTime) ? (object)DBNull.Value : (object)dob.ToString("dd-MMM-yyyy"));
                    com.Parameters.Add("CIF_NO_IN", string.IsNullOrEmpty(cifNo) ? (object)DBNull.Value : (object)cifNo);
                    //com.Parameters.Add("RECORD_STATUS_IN", string.IsNullOrEmpty(subjectName) ? (object)DBNull.Value : (object)subjectName);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillShortPersonRecord(dr));
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return list;
        }

        private PersonalData FillShortPersonRecord(OracleDataReader dr)
        {
            PersonalData person = new PersonalData();
            var ord = 0;
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                person.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                person.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("TITLE", out ord) && !dr.IsDBNull(ord))
                person.Title = dr.GetString(ord);
            if (dr.TryGetOrdinal("NAME", out ord) && !dr.IsDBNull(ord))
                person.Name = dr.GetString(ord);
            if (dr.TryGetOrdinal("FATHERS_TITLE", out ord) && !dr.IsDBNull(ord))
                person.FathersTitle = dr.GetString(ord);
            if (dr.TryGetOrdinal("FATHERS_NAME", out ord) && !dr.IsDBNull(ord))
                person.FathersName = dr.GetString(ord);
            if (dr.TryGetOrdinal("MOTHERS_TITLE", out ord) && !dr.IsDBNull(ord))
                person.MothersTitle = dr.GetString(ord);
            if (dr.TryGetOrdinal("MOTHERS_NAME", out ord) && !dr.IsDBNull(ord))
                person.MothersName = dr.GetString(ord);
            if (dr.TryGetOrdinal("DATE_OF_BIRTH", out ord) && !dr.IsDBNull(ord))
                person.DateOfBirth = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("CIF_NO", out ord) && !dr.IsDBNull(ord))
                person.CifNo = dr.GetString(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                person.RecordStatus = dr.GetString(ord);
            return person;
        }

        public List<Institution> SelectInstitutionssByFilters(string cibBranchCode, string subjectCode, string subjectName, string businessAddress, string cifNo)
        {
            List<Institution> list = new List<Institution>();
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_GET_INSTITUTIONS_BY_FILTERS";
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(cibBranchCode) ? (object)DBNull.Value : (object)cibBranchCode);
                    com.Parameters.Add("FI_SUBJECT_CODE_IN", string.IsNullOrEmpty(subjectCode) ? (object)DBNull.Value : (object)subjectCode);
                    com.Parameters.Add("FI_SUBJECT_NAME_IN", string.IsNullOrEmpty(subjectName) ? (object)DBNull.Value : (object)subjectName);
                    com.Parameters.Add("BUSINESS_ADDRESS_IN", string.IsNullOrEmpty(businessAddress) ? (object)DBNull.Value : (object)businessAddress);
                    com.Parameters.Add("CIF_NO_IN", string.IsNullOrEmpty(cifNo) ? (object)DBNull.Value : (object)cifNo);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillShortInstitutionRecord(dr));
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return list;
        }

        private Institution FillShortInstitutionRecord(OracleDataReader dr)
        {
            Institution institution = new Institution();
            var ord = 0;
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                institution.BranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                institution.FISubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("TITLE", out ord) && !dr.IsDBNull(ord))
                institution.Title = dr.GetString(ord);
            if (dr.TryGetOrdinal("TRADE_NAME", out ord) && !dr.IsDBNull(ord))
                institution.TradeName = dr.GetString(ord);
            if (dr.TryGetOrdinal("LEGAL_FORM", out ord) && !dr.IsDBNull(ord))
                institution.LegalFormText = dr.GetString(ord);
            if (dr.TryGetOrdinal("BUSINESS_ADDRESS_STREET", out ord) && !dr.IsDBNull(ord))
                institution.BusinessAddress = new Address { Street = dr.GetString(ord) };
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                institution.RecordStatus = dr.GetString(ord);
            if (dr.TryGetOrdinal("CIF_NO", out ord) && !dr.IsDBNull(ord))
                institution.CifNo = dr.GetString(ord);
            return institution;
        }

        public List<OwnerLink> SelectOwnerLinksFiltered(string cibBranchCode, string ownerSubjectCode, string ownerName, string instSubjectCode, string instName)
        {
            List<OwnerLink> list = new List<OwnerLink>();
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_SUBJECT.SP_GET_OWNER_LINKS_BY_FILTERS";
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(cibBranchCode) ? (object)DBNull.Value : (object)cibBranchCode);
                    com.Parameters.Add("FI_OWNER_CODE_IN", string.IsNullOrEmpty(ownerSubjectCode) ? (object)DBNull.Value : (object)ownerSubjectCode);
                    com.Parameters.Add("FI_OWNER_NAME_IN", string.IsNullOrEmpty(ownerName) ? (object)DBNull.Value : (object)ownerName);
                    com.Parameters.Add("FI_INSTITUTION_CODE_IN", string.IsNullOrEmpty(instSubjectCode) ? (object)DBNull.Value : (object)instSubjectCode);
                    com.Parameters.Add("FI_INSTITUTION_NAME_IN", string.IsNullOrEmpty(instName) ? (object)DBNull.Value : (object)instName);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(FillOwnerLinkRecord(dr));
                        }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }

            return list;
        }

        private OwnerLink FillOwnerLinkRecord(OracleDataReader dr)
        {
            OwnerLink link = new OwnerLink();
            var ord = 0;
            if (dr.TryGetOrdinal("RECORD_TYPE", out ord) && !dr.IsDBNull(ord))
                link.RecordType = dr.GetString(ord);
            if (dr.TryGetOrdinal("FI_CODE", out ord) && !dr.IsDBNull(ord))
                link.FiCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                link.CibBranchCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("INSTITUTION_FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                link.InstitutionFiSubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("TRADE_NAME", out ord) && !dr.IsDBNull(ord))
                link.InstitutionName = dr.GetString(ord);
            if (dr.TryGetOrdinal("ROLE", out ord) && !dr.IsDBNull(ord))
                link.Role = dr.GetString(ord);
            if (dr.TryGetOrdinal("OWNER_FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                link.OwnerFiSubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("NAME", out ord) && !dr.IsDBNull(ord))
                link.OwnerName = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                link.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                link.MakeDate = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("RECORD_STATUS", out ord) && !dr.IsDBNull(ord))
                link.RecordStatus = dr.GetString(ord);
            return link;
        }

        public bool FlagDeletePersons(List<PersonalData> subCodes)
        {
            try
            {
                using (OracleConnection con =
     new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_SUBJECT.SP_PERSON_FLAG_DELETE";
                        com.Parameters.Add("FI_SUBJECT_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["FI_SUBJECT_CODE_IN"].Value = subCode.FISubjectCode;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
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
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                return false;
            }
            return true;
        }

        public bool FlagDeleteInstitutions(List<Institution> subCodes)
        {
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_SUBJECT.SP_INSTITUTION_FLAG_DELETE";
                        com.Parameters.Add("FI_SUBJECT_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["FI_SUBJECT_CODE_IN"].Value = subCode.FISubjectCode;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
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
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                return false;
            }
            return true;
        }

        public bool FlagDeleteOwnerLinks(List<OwnerLink> links)
        {
            try
            {
                using (OracleConnection con =
     new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_SUBJECT.SP_OWNER_LINK_FLAG_DELETE";
                        com.Parameters.Add("FI_INSTITUTION_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("FI_PERSON_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var link in links)
                            {
                                com.Parameters["FI_INSTITUTION_CODE_IN"].Value = link.InstitutionFiSubjectCode;
                                com.Parameters["FI_PERSON_CODE_IN"].Value = link.OwnerFiSubjectCode;
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
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
                return false;
            }
            return true;
        }

        public List<SubjectError> SelectPersonalDataError(string cibBranchCode, string subCode, string subName)
        {
            List<SubjectError> list = new List<SubjectError>();
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ERROR.SP_GET_PERSONAL_DATA_ERROR";
                    com.Parameters.Add("P_BRANCH_CODE", cibBranchCode);
                    com.Parameters.Add("P_FI_SUBJECT_CODE", subCode);
                    com.Parameters.Add("P_FI_SUBJECT_NAME", subName);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillSubjectErrorRecord(dr));

                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return list;
        }

        private SubjectError FillSubjectErrorRecord(IDataRecord dr)
        {
            SubjectError per = new SubjectError();
            var ord = 0;
            if (dr.TryGetOrdinal("VERSION_NO", out ord) && !dr.IsDBNull(ord))
                per.VersionNo = dr.GetInt64(ord);
            if (dr.TryGetOrdinal("REPORTING_PERIOD", out ord) && !dr.IsDBNull(ord))
                per.ReportingPeriod = dr.GetDateTime(ord);
            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                per.FiSubjectCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("SUBJECT_NAME", out ord) && !dr.IsDBNull(ord))
                per.FiSubjectName = dr.GetString(ord);
            if (dr.TryGetOrdinal("ERROR_CODE", out ord) && !dr.IsDBNull(ord))
                per.ErrorCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("ERROR_DESCRIPTION", out ord) && !dr.IsDBNull(ord))
                per.ErrorDescription = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_BY", out ord) && !dr.IsDBNull(ord))
                per.MakeBy = dr.GetString(ord);
            if (dr.TryGetOrdinal("MAKE_DATE", out ord) && !dr.IsDBNull(ord))
                per.MakeDate = dr.GetDateTime(ord);

            return per;
        }

        public List<SubjectError> SelectInstitutionError(string cibBranchCode, string subCode, string subName)
        {
            List<SubjectError> list = new List<SubjectError>();
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ERROR.SP_GET_INSTITUTION_ERROR";
                    com.Parameters.Add("P_BRANCH_CODE", cibBranchCode);
                    com.Parameters.Add("P_FI_SUBJECT_CODE", subCode);
                    com.Parameters.Add("P_FI_SUBJECT_NAME", subName);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillSubjectErrorRecord(dr));

                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.Error(ex, ex.Message);
            }
            return list;
        }
        public bool IsInPersonalInformation(string subjectCode, DateTime reportingPerioddate)
        {
            string FISubjectCode = null;
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_PERSONAL_DATA_REPORT";
                    com.Parameters.Add("SUBJECT_CODE", subjectCode);
                    com.Parameters.Add("REPORT_PERIOD", reportingPerioddate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            var ord = 0;
                            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                                FISubjectCode = dr.GetString(ord);
                        }
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            if (FISubjectCode != null)
                return true;
            else
                return false;
        }
        public bool IsInInstitutionInformation(string subjectCode, DateTime reportingPerioddate)
        {
            string FISubjectCode = null;
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_CONTRACT.SP_GET_INSTITUTION_REPORT";
                    com.Parameters.Add("SUBJECT_CODE", subjectCode);
                    com.Parameters.Add("REPORT_PERIOD", reportingPerioddate.ToString("dd MMM yyyy"));
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            var ord = 0;
                            if (dr.TryGetOrdinal("FI_SUBJECT_CODE", out ord) && !dr.IsDBNull(ord))
                                FISubjectCode = dr.GetString(ord);
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            if (FISubjectCode != null)
                return true;
            else
                return false;
        }

        public IEnumerable<string> SelectNotExistingCif(IEnumerable<string> cifs)
        {
            var common_cifs = new List<string>();

            using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
            {
                for (int i = 0; i <= cifs.Count() / 1000 ; i++)
                {
                    var cif_split = cifs.Skip(i * 1000).Take(1000);
                    var common_cif_split = con.Query<string>("select distinct cif_no from vw_subjects where cif_no in :cifs", new { cifs = cif_split });
                    common_cifs.AddRange(common_cif_split);
                }
            }

            return cifs.Except(common_cifs);
        }

        public async Task<PersonalData> SelectSubjectBySmartId(string smartId)
        {
            PersonalData person = null;
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("SMART_ID_IN", smartId);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = await con.QueryFirstAsync<PersonalData>("PKG_SUBJECT.SP_GET_PERSON_BY_SMART_ID", dp, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateSubjectStatus(string subjectCode, string status)
        {
            string sql = subjectCode.StartsWith("P")
                       ? "update personal_data set record_status = :status where fi_subject_code = :sub_code"
                       : "update institutions set record_status = :status where fi_subject_code = :sub_code";
            try
            {
                using (OracleConnection con =
    new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add(":status", status);
                    dp.Add(":sub_code", subjectCode);
                    var result = await con.ExecuteAsync(sql, dp);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return false;
            }
        }
    }
}