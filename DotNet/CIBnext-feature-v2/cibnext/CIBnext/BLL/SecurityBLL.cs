using CIBnext.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIBnext.DAO;
using System.Threading.Tasks;
using System.Data.Common;
namespace CIBnext.BLL
{
    public class SecurityBLL
    {
        private List<string> messages = new List<string>();
        private readonly IConfiguration _configuration;

        public string[] Messages { get { return messages.ToArray(); } }

        public SecurityBLL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        internal async Task<string> SaveFlat(Flat ft)
        {
            SecurityDAL secDal = new SecurityDAL();
            string securityCode = await secDal.InsertFlat(ft);
            if (string.IsNullOrEmpty(securityCode))
                messages.Add(secDal.Message);
            return securityCode;
        }

        internal async Task<bool> SaveSecurityPayload(SecurityPayload payload, AppUser user)
        {
            using (var dalSession = new DalSession())
            {
                dalSession.UnitOfWork.Begin();
                try
                {

                    SecurityDAL secDal = new SecurityDAL(dalSession.UnitOfWork);
                    SubjectDAL subDal = new SubjectDAL(dalSession.UnitOfWork, _configuration);

                    if (payload.LandBuildings != null && payload.LandBuildings.Length > 0)
                        foreach (var lb in payload.LandBuildings)
                        {
                            PrepareLandBuildingForSave(lb, user);
                            lb.FiSecurityCode = await secDal.InsertLandBuilding(lb);
                            if (string.IsNullOrEmpty(lb.FiSecurityCode))
                            {
                                messages.Add(secDal.Message);
                                return false;
                            }
                        }

                    if (payload.Flats != null && payload.Flats.Length > 0)
                        foreach (var ft in payload.Flats)
                        {
                            PrepareFlatForSave(ft, user);
                            ft.FiSecurityCode = await secDal.InsertFlat(ft);
                            if (string.IsNullOrEmpty(ft.FiSecurityCode))
                            {
                                messages.Add(secDal.Message);
                                return false;
                            }
                        }

                    if (payload.Machineries != null && payload.Machineries.Length > 0)
                        foreach (var mc in payload.Machineries)
                        {
                            PrepareMachineryForSave(mc, user);
                            mc.FiSecurityCode = await secDal.InsertMachinery(mc);
                            if (string.IsNullOrEmpty(mc.FiSecurityCode))
                            {
                                messages.Add(secDal.Message);
                                return false;
                            }
                        }

                    if (payload.Mortgages != null && payload.Mortgages.Length > 0)
                        foreach (var mg in payload.Mortgages)
                        {
                            PrepareMortgageForSave(mg, user);
                            mg.FiMortgageCode = await secDal.InsertMortgage(mg);
                            if (!string.IsNullOrEmpty(mg.FiMortgageCode))
                            {
                                if (payload.LandBuildings != null && payload.LandBuildings.Length > 0)
                                    foreach (var lb in payload.LandBuildings)
                                        if (!await secDal.InsertSecurityLink(lb.FiSecurityCode, mg.FiMortgageCode, user.CibBranchCode, "1", user.DomainID))
                                        {
                                            messages.Add(secDal.Message);
                                            return false;
                                        }

                                if (payload.Flats != null && payload.Flats.Length > 0)
                                    foreach (var ft in payload.Flats)
                                        if (!await secDal.InsertSecurityLink(ft.FiSecurityCode, mg.FiMortgageCode, user.CibBranchCode, "1", user.DomainID))
                                        {
                                            messages.Add(secDal.Message);
                                            return false;
                                        }

                                if (payload.Machineries != null && payload.Machineries.Length > 0)
                                    foreach (var mc in payload.Machineries)
                                        if (!await secDal.InsertSecurityLink(mc.FiSecurityCode, mg.FiMortgageCode, user.CibBranchCode, "1", user.DomainID))
                                        {
                                            messages.Add(secDal.Message);
                                            return false;
                                        }
                            }
                            else
                            {
                                messages.Add(secDal.Message);
                                return false;
                            }
                        }

                    if (payload.Hypothecations != null && payload.Hypothecations.Length > 0)
                        foreach (var hp in payload.Hypothecations)
                        {
                            PrepareHypothecationForSave(hp, user);
                            hp.FiHypothecationCode = await secDal.InsertHypothecation(hp);
                            if (!string.IsNullOrEmpty(hp.FiHypothecationCode))
                            {
                                if (payload.LandBuildings != null && payload.LandBuildings.Length > 0)
                                    foreach (var lb in payload.LandBuildings)
                                        if (!await secDal.InsertSecurityLink(lb.FiSecurityCode, hp.FiHypothecationCode, user.CibBranchCode, "2", user.DomainID))
                                        {
                                            messages.Add(secDal.Message);
                                            return false;
                                        }

                                if (payload.Flats != null && payload.Flats.Length > 0)
                                    foreach (var ft in payload.Flats)
                                        if (!await secDal.InsertSecurityLink(ft.FiSecurityCode, hp.FiHypothecationCode, user.CibBranchCode, "2", user.DomainID))
                                        {
                                            messages.Add(secDal.Message);
                                            return false;
                                        }

                                if (payload.Machineries != null && payload.Machineries.Length > 0)
                                    foreach (var mc in payload.Machineries)
                                        if (!await secDal.InsertSecurityLink(mc.FiSecurityCode, hp.FiHypothecationCode, user.CibBranchCode, "2", user.DomainID))
                                        {
                                            messages.Add(secDal.Message);
                                            return false;
                                        }
                            }
                            else
                            {
                                messages.Add(secDal.Message);
                                return false;
                            }
                        }

                    if (payload.Persons != null && payload.Persons.Length > 0)
                        foreach (var person in payload.Persons)
                        {
                            PreparePersonForSave(person, user);
                            person.FISubjectCode = subDal.InsertPersonalData(person);
                            if (!string.IsNullOrEmpty(person.FISubjectCode))
                            {
                                if (payload.LandBuildings != null && payload.LandBuildings.Length > 0)
                                    foreach (var lb in payload.LandBuildings)
                                    {
                                        if (!string.IsNullOrEmpty(person.Role) && person.Role.Equals("O"))
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, lb.FiSecurityCode, person.Role, user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (person.IsBorrower)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, lb.FiSecurityCode, "B", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (person.IsMortgagor)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, lb.FiSecurityCode, "M", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                    }

                                if (payload.Flats != null && payload.Flats.Length > 0)
                                    foreach (var ft in payload.Flats)
                                    {
                                        if (!string.IsNullOrEmpty(person.Role) && person.Role.Equals("O"))
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, ft.FiSecurityCode, person.Role, user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (person.IsBorrower)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, ft.FiSecurityCode, "B", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (person.IsMortgagor)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, ft.FiSecurityCode, "M", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                    }

                                if (payload.Machineries != null && payload.Machineries.Length > 0)
                                    foreach (var mc in payload.Machineries)
                                    {
                                        if (!string.IsNullOrEmpty(person.Role) && person.Role.Equals("O"))
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, mc.FiSecurityCode, person.Role, user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (person.IsBorrower)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, mc.FiSecurityCode, "B", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (person.IsMortgagor)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, person.FISubjectCode, mc.FiSecurityCode, "M", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                    }
                            }
                            else
                            {
                                messages.Add(subDal.Message);
                                return false;
                            }
                        }

                    if (payload.Institutions != null && payload.Institutions.Length != 0)
                        foreach (var inst in payload.Institutions)
                        {
                            PrepareInstitutionForSave(inst, user);
                            inst.FISubjectCode = subDal.InsertInstitution(inst);
                            if (!string.IsNullOrEmpty(inst.FISubjectCode))
                            {
                                if (payload.LandBuildings != null && payload.LandBuildings.Length > 0)
                                    foreach (var lb in payload.LandBuildings)
                                    {
                                        if (!string.IsNullOrEmpty(inst.Role) && inst.Role.Equals("O"))
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, lb.FiSecurityCode, inst.Role, user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (inst.IsBorrower)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, lb.FiSecurityCode, "B", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (inst.IsMortgagor)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, lb.FiSecurityCode, "M", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                    }

                                if (payload.Flats != null && payload.Flats.Length > 0)
                                    foreach (var ft in payload.Flats)
                                    {
                                        if (!string.IsNullOrEmpty(inst.Role) && inst.Role.Equals("O"))
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, ft.FiSecurityCode, inst.Role, user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (inst.IsBorrower)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, ft.FiSecurityCode, "B", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (inst.IsMortgagor)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, ft.FiSecurityCode, "M", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                    }

                                if (payload.Machineries != null && payload.Machineries.Length > 0)
                                    foreach (var mc in payload.Machineries)
                                    {
                                        if (!string.IsNullOrEmpty(inst.Role) && inst.Role.Equals("O"))
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, mc.FiSecurityCode, inst.Role, user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (inst.IsBorrower)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, mc.FiSecurityCode, "B", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                        if (inst.IsMortgagor)
                                        {
                                            if (!await SaveSubjectLink(user.CibBranchCode, inst.FISubjectCode, mc.FiSecurityCode, "M", user.DomainID, secDal))
                                            {
                                                messages.Add(subDal.Message);
                                                return false;
                                            }
                                        }
                                    }
                            }
                            else
                            {
                                messages.Add(subDal.Message);
                                return false;
                            }
                        }
                    dalSession.UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    messages.Add(ex.Message);
                    dalSession.UnitOfWork.Rollback();
                    return false;
                }
            }

            return true;
        }

        internal async Task<List<Hypothecation>> GetFilteredHypothecations(string branchCode, string fiHypothecationCode, DateTime? hypothecationDate, string rjscFillingNo, DateTime? rjscFillingDate)
        {
            SecurityDAL secDal = new SecurityDAL();
            List<Hypothecation> hypos = await secDal.SelectFilteredHypothecations(branchCode, fiHypothecationCode, hypothecationDate, rjscFillingNo, rjscFillingDate);
            if(hypos == null)
            {
                messages.Add(secDal.Message);
                hypos = new List<Hypothecation>();
            }
            return hypos;
        }

        internal async Task<List<Mortgage>> GetFilteredMortgages(string branchCode, string fiMortgageCode, string mortgageDeedNo, DateTime? mortgageDate, decimal? mortgageValue)
        {
            SecurityDAL secDal = new SecurityDAL();
            List<Mortgage> mortgages = await secDal.SelectFilteredMortgages(branchCode, fiMortgageCode, mortgageDeedNo, mortgageDate, mortgageValue);
            if (mortgages == null)
            {
                messages.Add(secDal.Message);
                mortgages = new List<Mortgage>();
            }
            return mortgages;
        }

        internal async Task<SecurityPayload> GetSecurityPayload(string fiSecurityCode)
        {
            SecurityDAL secDal = new SecurityDAL();
            var pl = await secDal.SelectPayload(fiSecurityCode);
            if (pl == null)
                messages.Add(secDal.Message);
            else
            {
                List<PersonalData> persons = new List<PersonalData>();
                foreach (var code in pl.Persons.Select(p => p.FISubjectCode).Distinct())
                {
                    var tp = pl.Persons.Where(p => p.FISubjectCode.Equals(code)).FirstOrDefault();
                    if (pl.Persons.Where(p => p.FISubjectCode.Equals(code) && p.Role.Equals("B")).FirstOrDefault() != null)
                        tp.IsBorrower = true;
                    if (pl.Persons.Where(p => p.FISubjectCode.Equals(code) && p.Role.Equals("M")).FirstOrDefault() != null)
                        tp.IsMortgagor = true;
                    if (pl.Persons.Where(p => p.FISubjectCode.Equals(code) && p.Role.Equals("O")).FirstOrDefault() != null)
                        tp.Role = "O";
                    persons.Add(tp);
                }
                pl.Persons = persons.ToArray();

                List<Institution> institutions = new List<Institution>();
                foreach (var code in pl.Institutions.Select(i => i.FISubjectCode).Distinct())
                {
                    var ti = pl.Institutions.Where(i => i.FISubjectCode.Equals(code)).FirstOrDefault();
                    if (pl.Institutions.Where(i => i.FISubjectCode.Equals(code) && i.Role.Equals("B")).FirstOrDefault() != null)
                        ti.IsBorrower = true;
                    if (pl.Institutions.Where(i => i.FISubjectCode.Equals(code) && i.Role.Equals("M")).FirstOrDefault() != null)
                        ti.IsMortgagor = true;
                    if (pl.Institutions.Where(i => i.FISubjectCode.Equals(code) && i.Role.Equals("O")).FirstOrDefault() != null)
                        ti.Role = "O";
                    institutions.Add(ti);
                }
                pl.Institutions = institutions.ToArray();
            }
            return pl;

        }

        internal async Task<List<Machinery>> GetPagedMachineries(int pageIndex, int pageSize, string branchCode, string filterString, string filterBy)
        {
            SecurityDAL secDal = new SecurityDAL();
            return await secDal.SelectPagedMachineries(pageIndex, pageSize, branchCode, filterString, filterBy);
        }

        internal async Task<List<Flat>> GetPagedFlats(int pageIndex, int pageSize, string branchCode, string filterString, string filterBy)
        {
            SecurityDAL secDal = new SecurityDAL();
            return await secDal.SelectPagedFlats(pageIndex, pageSize, branchCode, filterString, filterBy);
        }

        internal async Task<List<LandBuilding>> GetPagedLandBuildings(int pageIndex, int pageSize, string branchCode, string filterString, string filterBy)
        {
            SecurityDAL secDal = new SecurityDAL();
            return await secDal.SelectPagedLandBuildings(pageIndex, pageSize, branchCode, filterString, filterBy);
        }

        internal async Task<bool> SaveSubjectLink(string cibBranchCode, string fISubjectCode, string fiSecMortHypCode, string role, string maker, SecurityDAL secDal)
        {
            string linkType = "";
            if (role.Equals("O")) linkType = "1";
            else if (role.Equals("B") && fiSecMortHypCode[0] == 'C') linkType = "3";
            else linkType = "2";
            bool res = await secDal.InsertSubjectLink(cibBranchCode, linkType, fISubjectCode, fiSecMortHypCode, role, maker);
            if (!res) messages.Add(secDal.Message);
            return res;
        }

        private void PrepareLandBuildingForSave(LandBuilding landBuilding, AppUser user)
        {
            landBuilding.FiCode = "046";
            landBuilding.BranchCode = user.CibBranchCode;
            landBuilding.MakeBy = user.DomainID;
            landBuilding.MakeDate = DateTime.Now;
            landBuilding.RecordStatus = "C";
            landBuilding.SecurityValueCode = "50";
            landBuilding.SecurityCategory = "L";
        }

        private void PrepareFlatForSave(Flat flat, AppUser user)
        {
            flat.FiCode = "046";
            flat.BranchCode = user.CibBranchCode;
            flat.MakeBy = user.DomainID;
            flat.MakeDate = DateTime.Now;
            flat.RecordStatus = "C";
            flat.SecurityValueCode = "50";
            flat.SecurityCategory = "F";
        }

        private void PrepareMachineryForSave(Machinery machinery, AppUser user)
        {
            machinery.FiCode = "046";
            machinery.BranchCode = user.CibBranchCode;
            machinery.MakeBy = user.DomainID;
            machinery.MakeDate = DateTime.Now;
            machinery.RecordStatus = "C";
            machinery.SecurityValueCode = "40";
            machinery.SecurityCategory = "M";
        }

        private void PrepareMortgageForSave(Mortgage mortgage, AppUser user)
        {
            mortgage.FiCode = "046";
            mortgage.BranchCode = user.CibBranchCode;
            mortgage.MakeBy = user.DomainID;
            mortgage.MakeDate = DateTime.Now;
            mortgage.RecordStatus = "C";
        }

        private void PrepareHypothecationForSave(Hypothecation hypthecation, AppUser user)
        {
            hypthecation.FiCode = "046";
            hypthecation.BranchCode = user.CibBranchCode;
            hypthecation.MakeBy = user.DomainID;
            hypthecation.MakeDate = DateTime.Now;
            hypthecation.RecordStatus = "C";
        }

        private void PrepareInstitutionForSave(Institution institution, AppUser user)
        {
            institution.FICode = "046";
            institution.BranchCode = user.CibBranchCode;
            institution.MakeBy = user.DomainID;
            institution.MakeDate = DateTime.Now;
            institution.RecordStatus = "C";
        }

        private void PreparePersonForSave(PersonalData personal, AppUser user)
        {
            personal.FICode = "046";
            personal.BranchCode = user.CibBranchCode;
            personal.MakeBy = user.DomainID;
            personal.MakeDate = DateTime.Now;
            personal.RecordStatus = "C";
        }
    }
}