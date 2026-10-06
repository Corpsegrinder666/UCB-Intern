using CIBnext.DAL;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
//using System.Web.Caching;

namespace CIBnext.BLL
{
    public enum LovTypes
    {
        BRANCHES,
        THIRD_PARTY_GUARANTEE_TYPES,
        CONTRACT_PHASES,
        CONTRACT_STATUS,
        CONTRACT_TYPES_INST,
        CONTRACT_TYPES_CARD,
        CONTRACT_TYPES_NONINST,
        COUNTRIES,
        CURRENCIES,
        ECONOMIC_PURPOSE_CODES,
        ENTERPRISE_TYPES,
        GENDER,
        ID_TYPES,
        INSTALMENT_TYPES,
        INSTITUTION_LEGAL_FORMS,
        LEASED_GOOD_TYPES,
        LINK_TYPES,
        NO_OF_DAYS,
        OWNER_TYPES,
        PAYMENT_METHODS,
        PERIODICITY_OF_PAYMENT,
        PRE_FINANCE_OF_LOAN,
        REORGANIZED_CREDIT,
        SECTOR_CODES,
        SECTOR_TYPES,
        SECURITY_TYPES,
        SUBSIDIZED_CREDIT,
        SUB_REGISTRY_OFFICES,
        THANAS,
        MOUZAS,
        DEED_TYPES
    }


    public class ListOfValuesBLL
    {
        private readonly ListOfValuesDAL _dal;       // 
        private readonly IMemoryCache _cache;         // 

        public ListOfValuesBLL(ListOfValuesDAL dal, IMemoryCache cache)  // 
        {
            _dal = dal;
            _cache = cache;
        }
        public List<KeyValuePair<string, string>> GetListOfValues(LovTypes LovType)
        {
            string cacheKey = Enum.GetName(typeof(LovTypes), LovType);

            
            if (!_cache.TryGetValue(cacheKey, out List<KeyValuePair<string, string>> list))  // 
            {
                list = _dal.SelectListOfValues(LovType);
                _cache.Set(cacheKey, list, TimeSpan.FromDays(1));  
            }
            return list;
        
        }

        internal List<string> GetMouzasByDistrict(string district)
        {
            return _dal.SelectMouzasByDistrict(district);
        }

        public List<string> GetDistrictsByCountryCode(string countryCode)
        {
            return _dal.SelectDistrictsByCountryCode(countryCode);
        }

        internal List<string> GetThanasByDistrict(string district)
        {
            return _dal.SelectThanasByDistrict(district);
        }

        internal List<string> GetSROsByDistricts(string district)
        {
            return _dal.SelectSROsByDistricts(district);
        }
    }
}