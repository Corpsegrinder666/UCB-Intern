using CIBnext.DAL;
using CIBnext.DAO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CIBnext.BLL
{
    public class ContractBLL
    {
        private readonly ContractDAL conDal;
        private readonly IConfiguration _configuration;

        public ContractBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            conDal = new ContractDAL(_configuration);
        }

        public bool SaveContract(InstalmentContract contract)
        {
            return conDal.InsertContract(contract);
        }

        public bool SaveContract(NonInstalmentContract contract)
        {
            return conDal.InsertContract(contract);
        }

        public bool SaveContract(CardContract contract)
        {
            return conDal.InsertContract(contract);
        }

        public bool SaveContractLink(ContractLink link)
        {
            return conDal.InsertContractLink(link);
        }

        public InstalmentContract GetInstalmentContract(string contractCode)
        {
            return conDal.SelectInstalmentContract(contractCode);
        }

        public List<InstalmentContract> GetInstalmentContractsWithFilters(string subjectCode, string cibBranchCode, string contractCode, string contractPhase, string ubsAcNo)
        {
            return conDal.SelectInstalmentContractsWithFilters(subjectCode, cibBranchCode, contractCode, contractPhase, ubsAcNo);
        }

        public NonInstalmentContract GetNonInstalmentContract(string contractCode)
        {
            return conDal.SelectNonInstalmentContract(contractCode);
        }

        public List<NonInstalmentContract> GetNonInstalmentContractsWithFilters(string subjectCode, string cibBranchCode, string contractCode, string contractPhase, string ubsAcNo)
        {
            return conDal.SelectNonInstalmentContractsWithFilters(subjectCode, cibBranchCode, contractCode, contractPhase, ubsAcNo);
        }
        public CardContract GetCardContract(string contractCode)
        {
            return conDal.SelectCardContract(contractCode);
        }

        public List<CardContract> GetCardContractsWithFilters(string subjectCode, string cibBranchCode, string contractCode, string contractPhase)
        {
            return conDal.SelectCardContractsWithFilters(subjectCode, cibBranchCode, contractCode, contractPhase);
        }

        public List<ContractLink> GetContractLinksFiltered(string branchCode, string primaryCode, string primaryName, string secondaryCode, string secondaryName, string contractCode)
        {
            return conDal.SelectContractLinksFiltered(branchCode, primaryCode, primaryName, secondaryCode, secondaryName, contractCode);
        }

        public bool FlagDeleteCardContracts(List<Contract> contracts)
        {
            return conDal.FlagDeleteCardContracts(contracts);
        }

        public bool FlagDeleteInstalmentContracts(List<Contract> contracts)
        {
            return conDal.FlagDeleteInstalmentContracts(contracts);
        }

        public bool FlagDeleteNonInstalmentContracts(List<Contract> contracts)
        {
            return conDal.FlagDeleteNonInstalmentContracts(contracts);
        }

        public bool FlagDeleteContractLinks(List<ContractLink> links)
        {
            return conDal.FlagDeleteContractLinks(links);
        }

        public List<ContractError> GetInstalmentContractError(string cibBranchCode, string conCode, string subCode)
        {
            return conDal.SelectInstalmentContractError(cibBranchCode, conCode, subCode);
        }

        public List<ContractError> GetNonInstalmentContractError(string cibBranchCode, string conCode, string subCode)
        {
            return conDal.SelectNonInstalmentContractError(cibBranchCode, conCode, subCode);
        }

        public List<ContractError> GetCardContractError(string cibBranchCode, string conCode, string subCode)
        {
            return conDal.SelectCardContractError(cibBranchCode, conCode, subCode);
        }

        internal bool IsContractSavedAsNonInstalment(string conCode)
        {
            return conDal.SelectNonInstalmentContractExists(conCode);
        }

        internal bool IsContractSavedAsInstalment(string conCode)
        {
            return conDal.SelectInstalmentContractExists(conCode);
        }
        public bool IsInInstalmentContractReport(string contractCode, DateTime reportingPerioddate)
        {
            return conDal.IsInInstalmentContract(contractCode, reportingPerioddate);
        }
        public bool IsInNonInstalmentContractReport(string contractCode, DateTime reportingPerioddate)
        {
            return conDal.IsInNonInstalmentContract(contractCode, reportingPerioddate);
        }
    }
}