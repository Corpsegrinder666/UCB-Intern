using CIBnext.DAL;
using CIBnext.DAO;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace CIBnext.BLL
{
    public class SubjectBLL
    {
        private readonly SubjectDAL subDal;
        private readonly IConfiguration _configuration;
        private Logger _logger = LogManager.GetLogger("SubjectBLL");
        public SubjectBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            subDal = new SubjectDAL(_configuration);
        }
        public string[] Messages { get; internal set; }

        public string SavePersonData(PersonalData personal)
        {
            return subDal.InsertPersonalData(personal);
        }

        public string SaveInstitution(Institution institution)
        {
            return subDal.InsertInstitution(institution);
        }

        public Institution GetInstitution(string subCode)
        {
            return subDal.SelectInstitution(subCode);
        }

        public PersonalData GetPerson(string subCode)
        {
            return subDal.SelectPerson(subCode);
        }

        public bool SaveOwnerLink(OwnerLink link)
        {
            return subDal.InsertOwnerLink(link);
        }

        public Subject GetSubjectByContractCode(string contractCode)
        {
            return subDal.SelectSubjectByContractCode(contractCode);
        }

        public List<PersonalData> GetFilteredPersonList(string cibBranchCode, string subjectCode, string subjectName, string fathersName, DateTime dob, string cifNo)
        {
            return subDal.SelectSubjectsByFilters(cibBranchCode, subjectCode, subjectName, fathersName, dob, cifNo);
        }

        public List<Institution> GetInstitutionsByFilter(string cibBranchCode, string subjectCode, string subjectName, string businessAddress, string cifNo)
        {
            return subDal.SelectInstitutionssByFilters(cibBranchCode, subjectCode, subjectName, businessAddress, cifNo);
        }

        internal async Task<Tuple<PersonalData, Institution>> GetSubjectByCifNo(string cifNo)
        {
            var res = await subDal.SelectSubjectByCifNo(cifNo);
            if (res == null)
                Messages = new[] { subDal.Message };
            return res;
        }

        public List<OwnerLink> GetOwnerLinksFiltered(string cibBranchCode, string ownerSubjectCode, string ownerName, string instSubjectCode, string instName)
        {
            return subDal.SelectOwnerLinksFiltered(cibBranchCode, ownerSubjectCode, ownerName, instSubjectCode, instName);
        }

        public bool FlagDeletePersons(List<PersonalData> subCodes)
        {
            return subDal.FlagDeletePersons(subCodes);
        }

        public bool FlagDeleteInstitutions(List<Institution> subCodes)
        {
            return subDal.FlagDeleteInstitutions(subCodes);
        }

        public bool FlagDeleteOwnerLinks(List<OwnerLink> links)
        {
            return subDal.FlagDeleteOwnerLinks(links);
        }

        public List<SubjectError> GetPersonalDataError(string cibBranchCode, string subCode, string subName)
        {
            return subDal.SelectPersonalDataError(cibBranchCode, subCode, subName);
        }

        public List<SubjectError> GetInstitutionError(string cibBranchCode, string subCode, string subName)
        {
            return subDal.SelectInstitutionError(cibBranchCode, subCode, subName);
        }
        public bool IsInPersonalInformationReport(string subjectCode, DateTime reportingPerioddate)
        {
            return subDal.IsInPersonalInformation(subjectCode, reportingPerioddate);
        }
        public bool IsInInstitutionInformationReport(string subjectCode, DateTime reportingPerioddate)
        {
            return subDal.IsInInstitutionInformation(subjectCode, reportingPerioddate);
        }

        public IEnumerable<string> GetNotExistingCif(IEnumerable<string> cifs)
        {
            return subDal.SelectNotExistingCif(cifs);
        }

        public async Task<PersonalData> GetSubjectBySmartId(string smartId)
        {
            return await subDal.SelectSubjectBySmartId(smartId);
        }

        public async Task<bool> UnlockSubject(string subjectCode, string name)
        {
            var res = await subDal.UpdateSubjectStatus(subjectCode, "U");
            if (res)
            {
                _logger.Info("{0} unlocked subject {1}", name, subjectCode);
            }
            return res;
        }
    }
}
