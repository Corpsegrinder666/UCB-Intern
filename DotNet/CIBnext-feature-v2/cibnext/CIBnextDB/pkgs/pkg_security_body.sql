CREATE OR REPLACE PACKAGE BODY PKG_SECURITY AS

  PROCEDURE SP_SECURITY_PAYLOAD_GET(
    IN_FI_SECURITY_CODE IN VARCHAR2,
    PERSON_CURSOR OUT SYS_REFCURSOR,
    INSTITUTION_CURSOR OUT SYS_REFCURSOR,
    LAND_BUILDING_CURSOR OUT SYS_REFCURSOR,
    FLAT_CURSOR OUT SYS_REFCURSOR,
    MACHINERY_CURSOR OUT SYS_REFCURSOR,
    MORTGAGE_CURSOR OUT SYS_REFCURSOR,
    HYPOTHECATION_CURSOR OUT SYS_REFCURSOR
  )
  AS
  BEGIN
    OPEN PERSON_CURSOR FOR
    SELECT 
      B.ROLE "Role",
      A.RECORD_TYPE "RecordType",
      A.FI_CODE "FICode",
      A.BRANCH_CODE "BranchCode",
      A.FI_SUBJECT_CODE "FISubjectCode",
      A.TITLE "Title",
      A.NAME "Name",
      A.FATHERS_TITLE "FathersTitle",
      A.FATHERS_NAME "FathersName",
      A.MOTHERS_TITLE "MothersTitle",
      A.MOTHERS_NAME "MothersName",
      A.SPOUSES_TITLE "SpousesTitle",
      A.SPOUSES_NAME "SpousesName",
      A.SECTOR_TYPE "SectorType",
      A.SECTOR_CODE "SectorCode",
      A.GENDER "Gender",
      A.DATE_OF_BIRTH "DateOfBirth",
      A.PLACE_OF_BIRTH "PlaceOfBirth",
      A.COUNTRY_OF_BIRTH "CountryOfBirth",
      A.NATIONAL_ID_NUMBER "NationalIDNumber",
      A.NATIONAL_ID_AVAILABLE "NationalIDAvailable",
      A.TIN "TIN",      
      A.ID_TYPE "IDType",
      A.ID_NUMBER "IDNumber",
      A.ID_ISSUE_DATE "IDIssueDate",
      A.ID_ISSUE_COUNTRY "IDIssueCountryCode",
      A.PHONE_NUMBER "PhoneNumber",
      A.MAKE_BY "MakeBy",
      A.MAKE_DATE "MakeDate",
      A.CIF_NO "CifNo",
      A.RECORD_STATUS "RecordStatus",
      A.PERMANENT_ADDRESS_STREET "Street",
      A.PERMANENT_ADDRESS_POSTAL_CODE "PostalCode",
      A.PERMANENT_ADDRESS_DISTRICT "District",
      A.PERMANENT_ADDRESS_COUNTRY "CountryCode"
    FROM
      PERSONAL_DATA A
    JOIN
      SUBJECT_LINKS B
    ON A.FI_SUBJECT_CODE = B.FI_SUBJECT_CODE    
    WHERE 
      B.FI_SEC_MORT_HYPO_CODE = IN_FI_SECURITY_CODE
    OR 
      B.FI_SEC_MORT_HYPO_CODE 
    IN (
      SELECT FI_MORT_HYPO_CODE
      FROM
        SECURITY_LINKS
      WHERE
        FI_SECURITY_CODE = IN_FI_SECURITY_CODE);

    OPEN INSTITUTION_CURSOR FOR
    SELECT
      B.ROLE "Role",
      A.RECORD_TYPE "RecordType",
      A.FI_CODE "FICode",
      A.BRANCH_CODE "BranchCode",
      A.FI_SUBJECT_CODE "FISubjectCode",
      A.TITLE "Title",
      A.TRADE_NAME "TradeName",
      A.SECTOR_TYPE "SectorType",
      A.SECTOR_CODE "SectorCode",
      A.LEGAL_FORM "LegalForm",
      A.REGISTRATION_NUMBER_RJSC "RegistrationNumberRJSC",
      A.REGISTRATION_DATE_RJSC "RegistrationDateRJSC",
      A.TIN "TIN",           
      A.CRG_SCORING "CRGScoring",
      A.CREDIT_RATING "CreditRating",
      A.PHONE_NUMBER "PhoneNumber",
      A.MAKE_BY "MakeBy",
      A.MAKE_DATE "MakeDate",
      A.CIF_NO "CifNo",
      A.RECORD_STATUS "RecordStatus",
      A.BUSINESS_ADDRESS_STREET "Street",
      A.BUSINESS_ADDRESS_POSTAL_CODE "PostalCode",
      A.BUSINESS_ADDRESS_DISTRICT "District",      
      A.BUSINESS_ADDRESS_COUNTRY "CountryCode"      
    FROM
      INSTITUTIONS A
    JOIN
      SUBJECT_LINKS B
    ON A.FI_SUBJECT_CODE = B.FI_SUBJECT_CODE    
    WHERE 
      B.FI_SEC_MORT_HYPO_CODE = IN_FI_SECURITY_CODE
    OR 
      B.FI_SEC_MORT_HYPO_CODE 
    IN (
      SELECT FI_MORT_HYPO_CODE
      FROM
        SECURITY_LINKS
      WHERE
        FI_SECURITY_CODE = IN_FI_SECURITY_CODE);

    OPEN LAND_BUILDING_CURSOR FOR
    SELECT 
      record_type "RecordType",
      fi_code "FiCode",
      branch_code "BranchCode",
      security_value_code "SecurityValueCode",
      security_category "SecurityCategory",
      fi_security_code "FiSecurityCode",
      type_deed "TypeDeed",
      title_deed_poa_no "TitleDeedPoaNo",
      to_date(lpad(date_registration,8,'0'),'DDMMYYYY') "DateRegistration",
      name_sub_registry_office "NameSubRegistryOffice",
      district "District",
      thana "Thana",
      mouza "Mouza",
      area_land "AreaLand",
      plot_no "PlotNo",
      holding_no "HoldingNo",
      address "Address",
      jote_no "JoteNo",
      dag_no_cs "DagNoCs",
      dag_no_sa "DagNoSa",
      dag_no_rs "DagNoRs",
      dag_no_bs "DagNoBs",
      dag_no_city_jorip "DagNoCityJorip",
      khatian_no_cs "KhatianNoCs",
      khatian_no_sa "KhatianNoSa",
      khatian_no_rs "KhatianNoRs",
      khatian_no_bs "KhatianNoBs",
      KHATIAN_NO_CITY_JORIP "KhatianNoCityJorip",
      mutation_khatian_no "MutationKhatianNo",
      leasehold_property "LeaseholdProperty",
      building_exists_in_land "BuildingExistsInLand",
      size_area_building "SizeAreaBuilding",
      number_floor "NumberFloor",
      make_by "MakeBy",
      make_date "MakeDate",
      check_by "CheckBy",
      check_date "CheckDate",
      record_status "RecordStatus"
    FROM LAND_BUILDINGS
    WHERE
      FI_SECURITY_CODE = IN_FI_SECURITY_CODE;

    OPEN FLAT_CURSOR FOR
    SELECT 
      record_type "RecordType",
      fi_code "FiCode",
      branch_code "BranchCode",
      security_value_code "SecurityValueCode",
      security_category "SecurityCategory",
      fi_security_code "FiSecurityCode",
      type_deed "TypeDeed",
      title_deed_poa_no "TitleDeedPoaNo",
      to_date(lpad(date_registration,8,'0'),'DDMMYYYY') "DateRegistration",
      name_sub_registry_office "NameSubRegistryOffice",
      district "District",
      thana "Thana",
      mouza "Mouza",
      tripartite_agreement "TripartiteAgreement",
      area_flat "AreaFlat",
      undemarcated_total_land_area "UndemarcatedTotalLandArea",
      apartment_flat_no "ApartmentFlatNo",
      floor_no "FloorNo",
      location_flat "LocationFlat",
      city_corpration_holding_no "CityCorprationHoldingNo",
      name_building "NameBuilding",
      name_project "NameProject",
      name_developer "NameDeveloper",
      rehab_member_no_developer "RehabMemberNoDeveloper",
      address "Address",
      jote_no "JoteNo",
      dag_no_cs "DagNoCs",
      dag_no_sa "DagNoSa",
      dag_no_rs "DagNoRs",
      dag_no_bs "DagNoBs",
      dag_no_city_jorip "DagNoCityJorip",
      khatian_no_cs "KhatianNoCs",
      khatian_no_sa "KhatianNoSa",
      khatian_no_rs "KhatianNoRs",
      khatian_no_bs "KhatianNoBs",
      khatian_no_city_jorip "KhatianNoCityJorip",
      mutation_khatian_no "MutationKhatianNo",
      leasehold_property "LeaseholdProperty",
      make_by "MakeBy",
      make_date "MakeDate",
      check_by "CheckBy",
      check_date "CheckDate",
      record_status "RecordStatus"
    FROM FLATS
    WHERE
      FI_SECURITY_CODE = IN_FI_SECURITY_CODE;

    OPEN MACHINERY_CURSOR FOR
    SELECT
      record_type "RecordType",
      fi_code "FiCode",
      branch_code "BranchCode",
      security_value_code "SecurityValueCode",
      security_category "SecurityCategory",
      fi_security_code "FiSecurityCode",
      name_machinery "NameMachinery",
      name_factory "NameFactory",
      address_factory "AddressFactory",
      mfg_co_brand_name "MfgCoBrandName",
      mfg_country "MfgCountry",
      mfg_year "MfgYear",
      model_no "ModelNo",
      no_unit "NoUnit",
      lc_no "LcNo",
      to_date(lpad(lc_date,8,'0'),'DDMMYYYY') "LcDate",
      value_lc "ValueLc",
      lading_air_way_bill_no "LadingAirWayBillNo",
      present_value "PresentValue",
      book_invoice_value "BookInvoiceValue",
      make_by "MakeBy",
      make_date "MakeDate",
      check_by "CheckBy",
      check_date "CheckDate",
      record_status "RecordStatus"
    FROM MACHINERIES
    WHERE
      FI_SECURITY_CODE = IN_FI_SECURITY_CODE;

    OPEN MORTGAGE_CURSOR FOR
    SELECT
      a.RECORD_TYPE "RecordType",
      a.FI_CODE "FiCode",
      a.BRANCH_CODE "BranchCode",
      a.FI_MORTGAGE_CODE "FiMortgageCode",
      a.MORTGAGE_TYPE "MortgageType",
      a.MORTGAGE_DEED_NO "MortgageDeedNo",
      to_date(lpad(a.MORTGAGE_DATE,8,'0'),'DDMMYYYY') "MortgageDate",
      a.MORTGAGE_VALUE "MortgageValue",
      a.MARKET_VALUE "MarketValue",
      a.RJSC_FILLING_NO "RjscFillingNo",
      to_date(lpad(a.RJSC_FILLING_DATE,8,'0'),'DDMMYYYY') "RjscFillingDate",
      a.RANKING_CHARGE "RankingCharge",
      a.PARI_PASSU_CHARGE "PariPassuCharge",
      a.RIGPA_NO "RigpaNo",
      to_date(lpad(a.RIGPA_DATE,8,'0'),'DDMMYYYY') "RigpaDate",
      a.MORTGAGE_PHASE "MortgagePhase",
      a.SECURITY_CATEGORY "SecurityCategory",
      A.MORTGAGED_LAND_AREA "MortgagedLandArea",
      a.MAKE_BY "MakeBy",
      a.MAKE_DATE "MakeDate",
      a.CHECK_BY "CheckBy",
      a.CHECK_DATE "CheckDate",
      a.RECORD_STATUS "RecordStatus"
    FROM
      MORTGAGES a 
    JOIN
      SECURITY_LINKS b
    ON
      a.FI_MORTGAGE_CODE = b.FI_MORT_HYPO_CODE
    WHERE
      b.FI_SECURITY_CODE = IN_FI_SECURITY_CODE;

    OPEN HYPOTHECATION_CURSOR FOR
    SELECT
      A.RECORD_TYPE "RecordType",
      A.FI_CODE "FiCode",
      A.BRANCH_CODE "BranchCode",
      A.FI_HYPOTHECATION_CODE "FiHypothecationCode",
      A.DATE_HYPOTHECATION "DateHypothecation",
      A.RJSC_FILLING_NO "RjscFillingNo",
      to_date(lpad(A.RJSC_FILLING_DATE,8,'0'),'DDMMYYYY') "RjscFillingDate",
      A.RANKING_CHARGE "RankingCharge",
      A.PARI_PASSU_CHARGE "PariPassuCharge",
      A.HYPOTHECATION_PHASE "HypothecationPhase",
      A.MAKE_BY "MakeBy",
      A.MAKE_DATE "MakeDate",
      A.CHECK_BY "CheckBy",
      A.CHECK_DATE "CheckDate",
      A.RECORD_STATUS "RecordStatus"
    FROM
      HYPOTHECATIONS A
    JOIN
      SECURITY_LINKS B
    ON
      A.FI_HYPOTHECATION_CODE = B.FI_MORT_HYPO_CODE
    WHERE
      B.FI_SECURITY_CODE = IN_FI_SECURITY_CODE;

  END SP_SECURITY_PAYLOAD_GET;

  PROCEDURE SP_LAND_BUILDING_HIST_INSERT (
    IN_FI_SECURITY_CODE IN VARCHAR2
  )
  AS
  BEGIN
    INSERT INTO LAND_BUILDINGS_HIST
    SELECT
      SEQ_LAND_BUILDINGS_HIST.NEXTVAL,
      SYSDATE,
      A.*
    FROM LAND_BUILDINGS A
    WHERE 
      A.FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
  END SP_LAND_BUILDING_HIST_INSERT;

  PROCEDURE SP_LAND_BUILDING_UPDATE(
    IN_RECORD_TYPE IN VARCHAR2,
    IN_FI_CODE IN VARCHAR2,
    IN_BRANCH_CODE IN VARCHAR2,
    IN_SECURITY_VALUE_CODE IN VARCHAR2,
    IN_SECURITY_CATEGORY IN VARCHAR2,
    IN_FI_SECURITY_CODE IN VARCHAR2,
    IN_TYPE_DEED IN VARCHAR2,
    IN_TITLE_DEED_POA_NO IN VARCHAR2,
    IN_DATE_REGISTRATION IN NUMBER,
    IN_NAME_SUB_REGISTRY_OFFICE IN VARCHAR2,
    IN_DISTRICT IN VARCHAR2,
    IN_THANA IN VARCHAR2,
    IN_MOUZA IN VARCHAR2,
    IN_AREA_LAND IN NUMBER,
    IN_PLOT_NO IN VARCHAR2,
    IN_HOLDING_NO IN VARCHAR2,
    IN_ADDRESS IN VARCHAR2,
    IN_JOTE_NO IN VARCHAR2,
    IN_DAG_NO_CS IN VARCHAR2,
    IN_DAG_NO_SA IN VARCHAR2,
    IN_DAG_NO_RS IN VARCHAR2,
    IN_DAG_NO_BS IN VARCHAR2,
    IN_DAG_NO_CITY_JORIP IN VARCHAR2,
    IN_KHATIAN_NO_CS IN VARCHAR2,
    IN_KHATIAN_NO_SA IN VARCHAR2,
    IN_KHATIAN_NO_RS IN VARCHAR2,
    IN_KHATIAN_NO_BS IN VARCHAR2,
    IN_KHATIAN_NO_CITY_JORIP IN VARCHAR2,
    IN_MUTATION_KHATIAN_NO IN VARCHAR2,
    IN_LEASEHOLD_PROPERTY IN VARCHAR2,
    IN_BUILDING_EXISTS_IN_LAND IN VARCHAR2,
    IN_SIZE_AREA_BUILDING IN NUMBER,
    IN_NUMBER_FLOOR IN NUMBER,
    IN_MAKE_BY IN VARCHAR2,
    IN_MAKE_DATE IN DATE,
    IN_CHECK_BY IN VARCHAR2,
    IN_CHECK_DATE IN DATE,
    IN_RECORD_STATUS IN VARCHAR2
  ) 
  AS
    L_RECORD_STATUS VARCHAR2(16);
  BEGIN
    SELECT RECORD_STATUS INTO L_RECORD_STATUS FROM LAND_BUILDINGS WHERE FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    IF L_RECORD_STATUS != 'R' AND L_RECORD_STATUS != 'L' THEN
      SP_LAND_BUILDING_HIST_INSERT(IN_FI_SECURITY_CODE);
      UPDATE LAND_BUILDINGS
      SET
        RECORD_TYPE = IN_RECORD_TYPE,
        FI_CODE = IN_FI_CODE,
        BRANCH_CODE = IN_BRANCH_CODE,
        SECURITY_VALUE_CODE = IN_SECURITY_VALUE_CODE,
        SECURITY_CATEGORY = IN_SECURITY_CATEGORY,        
        TYPE_DEED = IN_TYPE_DEED,
        TITLE_DEED_POA_NO = IN_TITLE_DEED_POA_NO,
        DATE_REGISTRATION = IN_DATE_REGISTRATION,
        NAME_SUB_REGISTRY_OFFICE = IN_NAME_SUB_REGISTRY_OFFICE,
        DISTRICT = IN_DISTRICT,
        THANA = IN_THANA,
        MOUZA = IN_MOUZA,
        AREA_LAND = IN_AREA_LAND,
        PLOT_NO = IN_PLOT_NO,
        HOLDING_NO = IN_HOLDING_NO,
        ADDRESS = IN_ADDRESS,
        JOTE_NO = IN_JOTE_NO,
        DAG_NO_CS = IN_DAG_NO_CS,
        DAG_NO_SA = IN_DAG_NO_SA,
        DAG_NO_RS = IN_DAG_NO_RS,
        DAG_NO_BS = IN_DAG_NO_BS,
        DAG_NO_CITY_JORIP = IN_DAG_NO_CITY_JORIP,
        KHATIAN_NO_CS = IN_KHATIAN_NO_CS,
        KHATIAN_NO_SA = IN_KHATIAN_NO_SA,
        KHATIAN_NO_RS = IN_KHATIAN_NO_RS,
        KHATIAN_NO_BS = IN_KHATIAN_NO_BS,
        KHATIAN_NO_CITY_JORIP = IN_KHATIAN_NO_CITY_JORIP,
        MUTATION_KHATIAN_NO = IN_MUTATION_KHATIAN_NO,
        LEASEHOLD_PROPERTY = IN_LEASEHOLD_PROPERTY,
        BUILDING_EXISTS_IN_LAND = IN_BUILDING_EXISTS_IN_LAND,
        SIZE_AREA_BUILDING = IN_SIZE_AREA_BUILDING,
        NUMBER_FLOOR = IN_NUMBER_FLOOR,
        MAKE_BY = IN_MAKE_BY,
        MAKE_DATE = IN_MAKE_DATE,
        CHECK_BY = IN_CHECK_BY,
        CHECK_DATE = IN_CHECK_DATE,
        RECORD_STATUS = IN_RECORD_STATUS
      WHERE
        FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    END IF;
  END SP_LAND_BUILDING_UPDATE;

  PROCEDURE SP_LAND_BUILDING_GET(
    IN_BRANCH_CODE IN VARCHAR2,
    IN_FI_SECURITY_CODE IN VARCHAR2,
    IN_SUBJECT_NAME IN VARCHAR2,    
    IN_ROW_START IN INT,
    IN_ROW_END IN INT,
    REF_CURSOR OUT SYS_REFCURSOR
  )
  AS        
  BEGIN
    OPEN REF_CURSOR FOR
    SELECT
      aa.*
    FROM
    (
      SELECT
        a.record_type "RecordType",
        a.fi_code "FiCode",
        a.branch_code "BranchCode",
        a.security_value_code "SecurityValueCode",
        a.security_category "SecurityCategory",
        a.fi_security_code "FiSecurityCode",
        a.type_deed "TypeDeed",
        a.title_deed_poa_no "TitleDeedPoaNo",
        to_date(lpad(a.date_registration,8,'0'),'DDMMYYYY') "DateRegistration",
        a.name_sub_registry_office "NameSubRegistryOffice",
        a.district "District",
        a.thana "Thana",
        a.mouza "Mouza",
        a.area_land "AreaLand",
        a.plot_no "PlotNo",
        a.holding_no "HoldingNo",
        a.address "Address",
        a.jote_no "JoteNo",
        a.dag_no_cs "DagNoCs",
        a.dag_no_sa "DagNoSa",
        a.dag_no_rs "DagNoRs",
        a.dag_no_bs "DagNoBs",
        a.dag_no_city_jorip "DagNoCityJorip",
        a.khatian_no_cs "KhatianNoCs",
        a.khatian_no_sa "KhatianNoSa",
        a.khatian_no_rs "KhatianNoRs",
        a.khatian_no_bs "KhatianNoBs",
        a.KHATIAN_NO_CITY_JORIP "KhatianNoCityJorip",
        a.mutation_khatian_no "MutationKhatianNo",
        a.leasehold_property "LeaseholdProperty",
        a.building_exists_in_land "BuildingExistsInLand",
        a.size_area_building "SizeAreaBuilding",
        a.number_floor "NumberFloor",
        a.make_by "MakeBy",
        a.make_date "MakeDate",
        a.check_by "CheckBy",
        a.check_date "CheckDate",
        a.record_status "RecordStatus",
        C.SUBJECT_NAME "SubjectName",
        B.ROLE "SubjectRole",
        rownum "rownumber"
      FROM
        land_buildings a
      JOIN
        SUBJECT_LINKS b
      ON
        A.fi_security_code = B.FI_SEC_MORT_HYPO_CODE      
      JOIN
        VW_SUBJECTS c
      on
        B.FI_SUBJECT_CODE = C.FI_SUBJECT_CODE
      WHERE
        A.FI_SECURITY_CODE = nvl(IN_FI_SECURITY_CODE, A.FI_SECURITY_CODE)
      AND
        A.BRANCH_CODE = nvl(IN_BRANCH_CODE, A.BRANCH_CODE)
      AND
        lower(C.SUBJECT_NAME) like lower(nvl('%'||IN_SUBJECT_NAME||'%', '%'))      
      order by a.fi_security_code
    ) aa
    WHERE
    "rownumber" BETWEEN IN_ROW_START AND IN_ROW_END;
  END SP_LAND_BUILDING_GET;

  PROCEDURE SP_LAND_BUILDING_INSERT(
    IN_RECORD_TYPE IN VARCHAR2,
    IN_FI_CODE IN VARCHAR2,
    IN_BRANCH_CODE IN VARCHAR2,
    IN_SECURITY_VALUE_CODE IN VARCHAR2,
    IN_SECURITY_CATEGORY IN VARCHAR2,
    IN_FI_SECURITY_CODE IN VARCHAR2,
    IN_TYPE_DEED IN VARCHAR2,
    IN_TITLE_DEED_POA_NO IN VARCHAR2,
    IN_DATE_REGISTRATION IN NUMBER,
    IN_NAME_SUB_REGISTRY_OFFICE IN VARCHAR2,
    IN_DISTRICT IN VARCHAR2,
    IN_THANA IN VARCHAR2,
    IN_MOUZA IN VARCHAR2,
    IN_AREA_LAND IN NUMBER,
    IN_PLOT_NO IN VARCHAR2,
    IN_HOLDING_NO IN VARCHAR2,
    IN_ADDRESS IN VARCHAR2,
    IN_JOTE_NO IN VARCHAR2,
    IN_DAG_NO_CS IN VARCHAR2,
    IN_DAG_NO_SA IN VARCHAR2,
    IN_DAG_NO_RS IN VARCHAR2,
    IN_DAG_NO_BS IN VARCHAR2,
    IN_DAG_NO_CITY_JORIP IN VARCHAR2,
    IN_KHATIAN_NO_CS IN VARCHAR2,
    IN_KHATIAN_NO_SA IN VARCHAR2,
    IN_KHATIAN_NO_RS IN VARCHAR2,
    IN_KHATIAN_NO_BS IN VARCHAR2,
    IN_KHATIAN_NO_CITY_JORIP IN VARCHAR2,
    IN_MUTATION_KHATIAN_NO IN VARCHAR2,
    IN_LEASEHOLD_PROPERTY IN VARCHAR2,
    IN_BUILDING_EXISTS_IN_LAND IN VARCHAR2,
    IN_SIZE_AREA_BUILDING IN NUMBER,
    IN_NUMBER_FLOOR IN NUMBER,
    IN_MAKE_BY IN VARCHAR2,
    IN_MAKE_DATE IN DATE,
    IN_CHECK_BY IN VARCHAR2,
    IN_CHECK_DATE IN DATE,
    IN_RECORD_STATUS IN VARCHAR2,
    REF_CURSOR OUT SYS_REFCURSOR
  )
  AS
    TMP_FI_SECURITY_CODE VARCHAR2(16);
    S_EXISTS INT;
  BEGIN
    SELECT COUNT(1) INTO S_EXISTS FROM LAND_BUILDINGS WHERE FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    IF IN_FI_SECURITY_CODE IS NULL AND S_EXISTS = 0 THEN
      SELECT
        IN_RECORD_TYPE
        ||IN_FI_CODE
        ||IN_BRANCH_CODE
        ||LPAD(TO_CHAR(SEQ_SECURITY.NEXTVAL),8,'0')
      INTO
        TMP_FI_SECURITY_CODE
      FROM
        DUAL;      
      INSERT INTO LAND_BUILDINGS VALUES (
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_SECURITY_VALUE_CODE,
        IN_SECURITY_CATEGORY,
        TMP_FI_SECURITY_CODE,
        IN_TYPE_DEED,
        IN_TITLE_DEED_POA_NO,
        IN_DATE_REGISTRATION,
        IN_NAME_SUB_REGISTRY_OFFICE,
        IN_DISTRICT,
        IN_THANA,
        IN_MOUZA,
        IN_AREA_LAND,
        IN_PLOT_NO,
        IN_HOLDING_NO,
        IN_ADDRESS,
        IN_JOTE_NO,
        IN_DAG_NO_CS,
        IN_DAG_NO_SA,
        IN_DAG_NO_RS,
        IN_DAG_NO_BS,
        IN_DAG_NO_CITY_JORIP,
        IN_KHATIAN_NO_CS,
        IN_KHATIAN_NO_SA,
        IN_KHATIAN_NO_RS,
        IN_KHATIAN_NO_BS,
        IN_KHATIAN_NO_CITY_JORIP,
        IN_MUTATION_KHATIAN_NO,
        IN_LEASEHOLD_PROPERTY,
        IN_BUILDING_EXISTS_IN_LAND,
        IN_SIZE_AREA_BUILDING,
        IN_NUMBER_FLOOR,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        IN_RECORD_STATUS
      );
    ELSE
      SP_LAND_BUILDING_UPDATE(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_SECURITY_VALUE_CODE,
        IN_SECURITY_CATEGORY,
        IN_FI_SECURITY_CODE,
        IN_TYPE_DEED,
        IN_TITLE_DEED_POA_NO,
        IN_DATE_REGISTRATION,
        IN_NAME_SUB_REGISTRY_OFFICE,
        IN_DISTRICT,
        IN_THANA,
        IN_MOUZA,
        IN_AREA_LAND,
        IN_PLOT_NO,
        IN_HOLDING_NO,
        IN_ADDRESS,
        IN_JOTE_NO,
        IN_DAG_NO_CS,
        IN_DAG_NO_SA,
        IN_DAG_NO_RS,
        IN_DAG_NO_BS,
        IN_DAG_NO_CITY_JORIP,
        IN_KHATIAN_NO_CS,
        IN_KHATIAN_NO_SA,
        IN_KHATIAN_NO_RS,
        IN_KHATIAN_NO_BS,
        IN_KHATIAN_NO_CITY_JORIP,
        IN_MUTATION_KHATIAN_NO,
        IN_LEASEHOLD_PROPERTY,
        IN_BUILDING_EXISTS_IN_LAND,
        IN_SIZE_AREA_BUILDING,
        IN_NUMBER_FLOOR,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        'U'
      );
    END IF;
    OPEN REF_CURSOR FOR SELECT NVL
    (
      TMP_FI_SECURITY_CODE,IN_FI_SECURITY_CODE
    )
    FROM DUAL;
  END SP_LAND_BUILDING_INSERT;

  PROCEDURE SP_FLAT_HIST_INSERT (
    IN_FI_SECURITY_CODE IN VARCHAR2
  )
  AS    
  BEGIN
    INSERT INTO FLATS_HIST
    SELECT
      SEQ_FLATS_HIST.NEXTVAL,
      SYSDATE,
      A.*
    FROM FLATS A
    WHERE
      A.FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
  END SP_FLAT_HIST_INSERT;

  PROCEDURE SP_FLAT_UPDATE(
    IN_RECORD_TYPE IN VARCHAR2,
    IN_FI_CODE IN VARCHAR2,
    IN_BRANCH_CODE IN VARCHAR2,
    IN_SECURITY_VALUE_CODE IN VARCHAR2,
    IN_SECURITY_CATEGORY IN VARCHAR2,
    IN_FI_SECURITY_CODE IN VARCHAR2,
    IN_TYPE_DEED IN VARCHAR2,
    IN_TITLE_DEED_POA_NO IN VARCHAR2,
    IN_DATE_REGISTRATION IN NUMBER,
    IN_NAME_SUB_REGISTRY_OFFICE IN VARCHAR2,
    IN_DISTRICT IN VARCHAR2,
    IN_THANA IN VARCHAR2,
    IN_MOUZA IN VARCHAR2,
    IN_TRIPARTITE_AGREEMENT IN VARCHAR2,
    IN_AREA_FLAT IN NUMBER,
    IN_UNDEMARCATED_LAND_AREA IN NUMBER,
    IN_APARTMENT_FLAT_NO IN VARCHAR2,
    IN_FLOOR_NO IN NUMBER,
    IN_LOCATION_FLAT IN VARCHAR2,
    IN_CITY_CORPRATION_HOLDING_NO IN VARCHAR2,
    IN_NAME_BUILDING IN VARCHAR2,
    IN_NAME_PROJECT IN VARCHAR2,
    IN_NAME_DEVELOPER IN VARCHAR2,
    IN_REHAB_MEMBER_NO_DEVELOPER IN VARCHAR2,
    IN_ADDRESS IN VARCHAR2,
    IN_JOTE_NO IN VARCHAR2,
    IN_DAG_NO_CS IN VARCHAR2,
    IN_DAG_NO_SA IN VARCHAR2,
    IN_DAG_NO_RS IN VARCHAR2,
    IN_DAG_NO_BS IN VARCHAR2,
    IN_DAG_NO_CITY_JORIP IN VARCHAR2,
    IN_KHATIAN_NO_CS IN VARCHAR2,
    IN_KHATIAN_NO_SA IN VARCHAR2,
    IN_KHATIAN_NO_RS IN VARCHAR2,
    IN_KHATIAN_NO_BS IN VARCHAR2,
    IN_KHATIAN_NO_CITY_JORIP IN VARCHAR2,
    IN_MUTATION_KHATIAN_NO IN VARCHAR2,
    IN_LEASEHOLD_PROPERTY IN VARCHAR2,
    IN_MAKE_BY IN VARCHAR2,
    IN_MAKE_DATE IN DATE,
    IN_CHECK_BY IN VARCHAR2,
    IN_CHECK_DATE IN DATE,
    IN_RECORD_STATUS IN VARCHAR2
  )
  AS
    L_RECORD_STATUS VARCHAR2(1);
  BEGIN
    SELECT RECORD_STATUS INTO L_RECORD_STATUS FROM FLATS WHERE FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    IF L_RECORD_STATUS != 'R' AND L_RECORD_STATUS != 'L' THEN
      SP_FLAT_HIST_INSERT(IN_FI_SECURITY_CODE);
      UPDATE FLATS
      SET
        RECORD_TYPE = IN_RECORD_TYPE,
        FI_CODE = IN_FI_CODE,
        BRANCH_CODE = IN_BRANCH_CODE,
        SECURITY_VALUE_CODE = IN_SECURITY_VALUE_CODE,
        SECURITY_CATEGORY = IN_SECURITY_CATEGORY,        
        TYPE_DEED = IN_TYPE_DEED,
        TITLE_DEED_POA_NO = IN_TITLE_DEED_POA_NO,
        DATE_REGISTRATION = IN_DATE_REGISTRATION,
        NAME_SUB_REGISTRY_OFFICE = IN_NAME_SUB_REGISTRY_OFFICE,
        DISTRICT = IN_DISTRICT,
        THANA = IN_THANA,
        MOUZA = IN_MOUZA,
        TRIPARTITE_AGREEMENT = IN_TRIPARTITE_AGREEMENT,
        AREA_FLAT = IN_AREA_FLAT,
        UNDEMARCATED_TOTAL_LAND_AREA = IN_UNDEMARCATED_LAND_AREA,
        APARTMENT_FLAT_NO = IN_APARTMENT_FLAT_NO,
        FLOOR_NO = IN_FLOOR_NO,
        LOCATION_FLAT = IN_LOCATION_FLAT,
        CITY_CORPRATION_HOLDING_NO = IN_CITY_CORPRATION_HOLDING_NO,
        NAME_BUILDING = IN_NAME_BUILDING,
        NAME_PROJECT = IN_NAME_PROJECT,
        NAME_DEVELOPER = IN_NAME_DEVELOPER,
        REHAB_MEMBER_NO_DEVELOPER = IN_REHAB_MEMBER_NO_DEVELOPER,
        ADDRESS = IN_ADDRESS,
        JOTE_NO = IN_JOTE_NO,
        DAG_NO_CS = IN_DAG_NO_CS,
        DAG_NO_SA = IN_DAG_NO_SA,
        DAG_NO_RS = IN_DAG_NO_RS,
        DAG_NO_BS = IN_DAG_NO_BS,
        DAG_NO_CITY_JORIP = IN_DAG_NO_CITY_JORIP,
        KHATIAN_NO_CS = IN_KHATIAN_NO_CS,
        KHATIAN_NO_SA = IN_KHATIAN_NO_SA,
        KHATIAN_NO_RS = IN_KHATIAN_NO_RS,
        KHATIAN_NO_BS = IN_KHATIAN_NO_BS,
        KHATIAN_NO_CITY_JORIP = IN_KHATIAN_NO_CITY_JORIP,
        MUTATION_KHATIAN_NO = IN_MUTATION_KHATIAN_NO,
        LEASEHOLD_PROPERTY = IN_LEASEHOLD_PROPERTY,
        MAKE_BY = IN_MAKE_BY,
        MAKE_DATE = IN_MAKE_DATE,
        CHECK_BY = IN_CHECK_BY,
        CHECK_DATE = IN_CHECK_DATE,
        RECORD_STATUS = IN_RECORD_STATUS
      WHERE
        FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    END IF;
  END SP_FLAT_UPDATE;

  PROCEDURE SP_FLAT_GET(
    IN_BRANCH_CODE IN VARCHAR2,
    IN_FI_SECURITY_CODE IN VARCHAR2,
    IN_SUBJECT_NAME IN VARCHAR2,        
    IN_ROW_START IN INT,
    IN_ROW_END IN INT,
    REF_CURSOR OUT SYS_REFCURSOR
  )
  AS
  BEGIN
    OPEN REF_CURSOR FOR
    SELECT
      aa.*
    FROM
    (
      SELECT
        a.record_type "RecordType",
        a.fi_code "FiCode",
        a.branch_code "BranchCode",
        a.security_value_code "SecurityValueCode",
        a.security_category "SecurityCategory",
        a.fi_security_code "FiSecurityCode",
        a.type_deed "TypeDeed",
        a.title_deed_poa_no "TitleDeedPoaNo",
        to_date(lpad(a.date_registration,8,'0'),'DDMMYYYY') "DateRegistration",
        a.name_sub_registry_office "NameSubRegistryOffice",
        a.district "District",
        a.thana "Thana",
        a.mouza "Mouza",
        a.tripartite_agreement "TripartiteAgreement",
        a.area_flat "AreaFlat",
        a.undemarcated_total_land_area "UndemarcatedTotalLandArea",
        a.apartment_flat_no "ApartmentFlatNo",
        a.floor_no "FloorNo",
        a.location_flat "LocationFlat",
        a.city_corpration_holding_no "CityCorprationHoldingNo",
        a.name_building "NameBuilding",
        a.name_project "NameProject",
        a.name_developer "NameDeveloper",
        a.rehab_member_no_developer "RehabMemberNoDeveloper",
        a.address "Address",
        a.jote_no "JoteNo",
        a.dag_no_cs "DagNoCs",
        a.dag_no_sa "DagNoSa",
        a.dag_no_rs "DagNoRs",
        a.dag_no_bs "DagNoBs",
        a.dag_no_city_jorip "DagNoCityJorip",
        a.khatian_no_cs "KhatianNoCs",
        a.khatian_no_sa "KhatianNoSa",
        a.khatian_no_rs "KhatianNoRs",
        a.khatian_no_bs "KhatianNoBs",
        a.khatian_no_city_jorip "KhatianNoCityJorip",
        a.mutation_khatian_no "MutationKhatianNo",
        a.leasehold_property "LeaseholdProperty",
        a.make_by "MakeBy",
        a.make_date "MakeDate",
        a.check_by "CheckBy",
        a.check_date "CheckDate",
        a.record_status "RecordStatus",
        C.SUBJECT_NAME "SubjectName",
        B.ROLE "SubjectRole",
        rownum "rownumber"
      FROM
        flats a
      JOIN
        SUBJECT_LINKS b
      ON
        A.fi_security_code = B.FI_SEC_MORT_HYPO_CODE      
      JOIN
        VW_SUBJECTS c
      on
        B.FI_SUBJECT_CODE = C.FI_SUBJECT_CODE
      WHERE
        A.FI_SECURITY_CODE = nvl(IN_FI_SECURITY_CODE, A.FI_SECURITY_CODE)
      AND
        A.BRANCH_CODE = nvl(IN_BRANCH_CODE, A.BRANCH_CODE)
      AND
        lower(C.SUBJECT_NAME) like lower(nvl('%'||IN_SUBJECT_NAME||'%', '%')) 
      ORDER BY a.FI_SECURITY_CODE
    ) aa
    WHERE
    "rownumber" BETWEEN IN_ROW_START AND IN_ROW_END;
  END SP_FLAT_GET;

  PROCEDURE SP_FLAT_INSERT(
    IN_RECORD_TYPE IN VARCHAR2,
    IN_FI_CODE IN VARCHAR2,
    IN_BRANCH_CODE IN VARCHAR2,
    IN_SECURITY_VALUE_CODE IN VARCHAR2,
    IN_SECURITY_CATEGORY IN VARCHAR2,
    IN_FI_SECURITY_CODE IN VARCHAR2,
    IN_TYPE_DEED IN VARCHAR2,
    IN_TITLE_DEED_POA_NO IN VARCHAR2,
    IN_DATE_REGISTRATION IN NUMBER,
    IN_NAME_SUB_REGISTRY_OFFICE IN VARCHAR2,
    IN_DISTRICT IN VARCHAR2,
    IN_THANA IN VARCHAR2,
    IN_MOUZA IN VARCHAR2,
    IN_TRIPARTITE_AGREEMENT IN VARCHAR2,
    IN_AREA_FLAT IN NUMBER,
    IN_UNDEMARCATED_LAND_AREA IN NUMBER,
    IN_APARTMENT_FLAT_NO IN VARCHAR2,
    IN_FLOOR_NO IN NUMBER,
    IN_LOCATION_FLAT IN VARCHAR2,
    IN_CITY_CORPRATION_HOLDING_NO IN VARCHAR2,
    IN_NAME_BUILDING IN VARCHAR2,
    IN_NAME_PROJECT IN VARCHAR2,
    IN_NAME_DEVELOPER IN VARCHAR2,
    IN_REHAB_MEMBER_NO_DEVELOPER IN VARCHAR2,
    IN_ADDRESS IN VARCHAR2,
    IN_JOTE_NO IN VARCHAR2,
    IN_DAG_NO_CS IN VARCHAR2,
    IN_DAG_NO_SA IN VARCHAR2,
    IN_DAG_NO_RS IN VARCHAR2,
    IN_DAG_NO_BS IN VARCHAR2,
    IN_DAG_NO_CITY_JORIP IN VARCHAR2,
    IN_KHATIAN_NO_CS IN VARCHAR2,
    IN_KHATIAN_NO_SA IN VARCHAR2,
    IN_KHATIAN_NO_RS IN VARCHAR2,
    IN_KHATIAN_NO_BS IN VARCHAR2,
    IN_KHATIAN_NO_CITY_JORIP IN VARCHAR2,
    IN_MUTATION_KHATIAN_NO IN VARCHAR2,
    IN_LEASEHOLD_PROPERTY IN VARCHAR2,
    IN_MAKE_BY IN VARCHAR2,
    IN_MAKE_DATE IN DATE,
    IN_CHECK_BY IN VARCHAR2,
    IN_CHECK_DATE IN DATE,
    IN_RECORD_STATUS IN VARCHAR2,
    REF_CURSOR OUT SYS_REFCURSOR
    ) 
  AS
    TMP_FI_SECURITY_CODE VARCHAR2(16);
    S_EXISTS INT;
  BEGIN
    SELECT COUNT(1) INTO S_EXISTS FROM FLATS WHERE FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    IF IN_FI_SECURITY_CODE IS NULL AND S_EXISTS = 0 THEN
       SELECT
        IN_RECORD_TYPE
        ||IN_FI_CODE
        ||IN_BRANCH_CODE
        ||LPAD(TO_CHAR(SEQ_SECURITY.NEXTVAL),8,'0')
      INTO
        TMP_FI_SECURITY_CODE
      FROM
        DUAL; 
      INSERT INTO FLATS VALUES (
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_SECURITY_VALUE_CODE,
        IN_SECURITY_CATEGORY,
        TMP_FI_SECURITY_CODE,
        IN_TYPE_DEED,
        IN_TITLE_DEED_POA_NO,
        IN_DATE_REGISTRATION,
        IN_NAME_SUB_REGISTRY_OFFICE,
        IN_DISTRICT,
        IN_THANA,
        IN_MOUZA,
        IN_TRIPARTITE_AGREEMENT,
        IN_AREA_FLAT,
        IN_UNDEMARCATED_LAND_AREA,
        IN_APARTMENT_FLAT_NO,
        IN_FLOOR_NO,
        IN_LOCATION_FLAT,
        IN_CITY_CORPRATION_HOLDING_NO,
        IN_NAME_BUILDING,
        IN_NAME_PROJECT,
        IN_NAME_DEVELOPER,
        IN_REHAB_MEMBER_NO_DEVELOPER,
        IN_ADDRESS,
        IN_JOTE_NO,
        IN_DAG_NO_CS,
        IN_DAG_NO_SA,
        IN_DAG_NO_RS,
        IN_DAG_NO_BS,
        IN_DAG_NO_CITY_JORIP,
        IN_KHATIAN_NO_CS,
        IN_KHATIAN_NO_SA,
        IN_KHATIAN_NO_RS,
        IN_KHATIAN_NO_BS,
        IN_KHATIAN_NO_CITY_JORIP,
        IN_MUTATION_KHATIAN_NO,
        IN_LEASEHOLD_PROPERTY,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        IN_RECORD_STATUS   
      );
    ELSE
      SP_FLAT_UPDATE(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_SECURITY_VALUE_CODE,
        IN_SECURITY_CATEGORY,
        IN_FI_SECURITY_CODE,
        IN_TYPE_DEED,
        IN_TITLE_DEED_POA_NO,
        IN_DATE_REGISTRATION,
        IN_NAME_SUB_REGISTRY_OFFICE,
        IN_DISTRICT,
        IN_THANA,
        IN_MOUZA,
        IN_TRIPARTITE_AGREEMENT,
        IN_AREA_FLAT,
        IN_UNDEMARCATED_LAND_AREA,
        IN_APARTMENT_FLAT_NO,
        IN_FLOOR_NO,
        IN_LOCATION_FLAT,
        IN_CITY_CORPRATION_HOLDING_NO,
        IN_NAME_BUILDING,
        IN_NAME_PROJECT,
        IN_NAME_DEVELOPER,
        IN_REHAB_MEMBER_NO_DEVELOPER,
        IN_ADDRESS,
        IN_JOTE_NO,
        IN_DAG_NO_CS,
        IN_DAG_NO_SA,
        IN_DAG_NO_RS,
        IN_DAG_NO_BS,
        IN_DAG_NO_CITY_JORIP,
        IN_KHATIAN_NO_CS,
        IN_KHATIAN_NO_SA,
        IN_KHATIAN_NO_RS,
        IN_KHATIAN_NO_BS,
        IN_KHATIAN_NO_CITY_JORIP,
        IN_MUTATION_KHATIAN_NO,
        IN_LEASEHOLD_PROPERTY,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        'U'
      );
    END IF;
    OPEN REF_CURSOR FOR SELECT NVL
    (
      TMP_FI_SECURITY_CODE,IN_FI_SECURITY_CODE
    )
    FROM DUAL;
  END SP_FLAT_INSERT;

  PROCEDURE SP_MACHINERY_HIST_INSERT (
    IN_FI_SECURITY_CODE IN VARCHAR2
  )
  AS
  BEGIN
    INSERT INTO MACHINERIES_HIST
    SELECT
      SEQ_MACHINERIES_HIST.NEXTVAL,
      SYSDATE,
      A.*
    FROM MACHINERIES A
    WHERE 
      A.FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
  END SP_MACHINERY_HIST_INSERT;

  PROCEDURE SP_MACHINERY_UPDATE(
        IN_RECORD_TYPE IN VARCHAR2,
        IN_FI_CODE IN VARCHAR2,
        IN_BRANCH_CODE IN VARCHAR2,
        IN_SECURITY_VALUE_CODE IN VARCHAR2,
        IN_SECURITY_CATEGORY IN VARCHAR2,
        IN_FI_SECURITY_CODE IN VARCHAR2,
        IN_NAME_MACHINERY IN VARCHAR2,
        IN_NAME_FACTORY IN VARCHAR2,
        IN_ADDRESS_FACTORY IN VARCHAR2,
        IN_MFG_CO_BRAND_NAME IN VARCHAR2,
        IN_MFG_COUNTRY IN VARCHAR2,
        IN_MFG_YEAR IN NUMBER,
        IN_MODEL_NO IN VARCHAR2,
        IN_NO_UNIT IN NUMBER,
        IN_LC_NO IN VARCHAR2,
        IN_LC_DATE IN NUMBER,
        IN_VALUE_LC IN NUMBER,
        IN_LADING_AIR_WAY_BILL_NO IN VARCHAR2,
        IN_PRESENT_VALUE IN NUMBER,
        IN_BOOK_INVOICE_VALUE IN NUMBER,
        IN_MAKE_BY IN VARCHAR2,
        IN_MAKE_DATE IN DATE,
        IN_CHECK_BY IN VARCHAR2,
        IN_CHECK_DATE IN DATE,
        IN_RECORD_STATUS IN VARCHAR2
  )
  AS
    L_RECORD_STATUS VARCHAR2(1);
  BEGIN
    SELECT RECORD_STATUS INTO L_RECORD_STATUS FROM MACHINERIES WHERE FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    IF L_RECORD_STATUS != 'R' AND L_RECORD_STATUS != 'L' THEN
      SP_MACHINERY_HIST_INSERT(IN_FI_SECURITY_CODE);
      UPDATE MACHINERIES
      SET
        RECORD_TYPE = IN_RECORD_TYPE,
        FI_CODE = IN_FI_CODE,
        BRANCH_CODE = IN_BRANCH_CODE,
        SECURITY_VALUE_CODE = IN_SECURITY_VALUE_CODE,
        SECURITY_CATEGORY = IN_SECURITY_CATEGORY,        
        NAME_MACHINERY = IN_NAME_MACHINERY,
        NAME_FACTORY = IN_NAME_FACTORY,
        ADDRESS_FACTORY = IN_ADDRESS_FACTORY,
        MFG_CO_BRAND_NAME = IN_MFG_CO_BRAND_NAME,
        MFG_COUNTRY = IN_MFG_COUNTRY,
        MFG_YEAR = IN_MFG_YEAR,
        MODEL_NO = IN_MODEL_NO,
        NO_UNIT = IN_NO_UNIT,
        LC_NO = IN_LC_NO,
        LC_DATE = IN_LC_DATE,
        VALUE_LC = IN_VALUE_LC,
        LADING_AIR_WAY_BILL_NO = IN_LADING_AIR_WAY_BILL_NO,
        PRESENT_VALUE = IN_PRESENT_VALUE,
        BOOK_INVOICE_VALUE = IN_BOOK_INVOICE_VALUE,
        MAKE_BY = IN_MAKE_BY,
        MAKE_DATE = IN_MAKE_DATE,
        CHECK_BY = IN_CHECK_BY,
        CHECK_DATE = IN_CHECK_DATE,
        RECORD_STATUS = IN_RECORD_STATUS
      WHERE
        FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    END IF;
  END SP_MACHINERY_UPDATE;

  PROCEDURE SP_MACHINERY_GET(
    IN_BRANCH_CODE IN VARCHAR2,
    IN_FI_SECURITY_CODE IN VARCHAR2,
    IN_SUBJECT_NAME IN VARCHAR2,        
    IN_ROW_START IN INT,
    IN_ROW_END IN INT,
    REF_CURSOR OUT SYS_REFCURSOR
  )
  AS
  BEGIN
    OPEN REF_CURSOR FOR
    SELECT
      aa.*
    FROM
    (
      SELECT
        a.record_type "RecordType",
        a.fi_code "FiCode",
        a.branch_code "BranchCode",
        a.security_value_code "SecurityValueCode",
        a.security_category "SecurityCategory",
        a.fi_security_code "FiSecurityCode",
        a.name_machinery "NameMachinery",
        a.name_factory "NameFactory",
        a.address_factory "AddressFactory",
        a.mfg_co_brand_name "MfgCoBrandName",
        a.mfg_country "MfgCountry",
        a.mfg_year "MfgYear",
        a.model_no "ModelNo",
        a.no_unit "NoUnit",
        a.lc_no "LcNo",
        to_date(lpad(a.lc_date,8,'0'),'DDMMYYYY') "LcDate",
        a.value_lc "ValueLc",
        a.lading_air_way_bill_no "LadingAirWayBillNo",
        a.present_value "PresentValue",
        a.book_invoice_value "BookInvoiceValue",
        a.make_by "MakeBy",
        a.make_date "MakeDate",
        a.check_by "CheckBy",
        a.check_date "CheckDate",
        a.record_status "RecordStatus",
        C.SUBJECT_NAME "SubjectName",
        B.ROLE "SubjectRole",
        rownum "rownumber"
      FROM
        machineries a
      JOIN
        SUBJECT_LINKS b
      ON
        A.fi_security_code = B.FI_SEC_MORT_HYPO_CODE      
      JOIN
        VW_SUBJECTS c
      on
        B.FI_SUBJECT_CODE = C.FI_SUBJECT_CODE
      WHERE
        A.FI_SECURITY_CODE = nvl(IN_FI_SECURITY_CODE, A.FI_SECURITY_CODE)
      AND
        A.BRANCH_CODE = nvl(IN_BRANCH_CODE, A.BRANCH_CODE)
      AND
        lower(C.SUBJECT_NAME) like lower(nvl('%'||IN_SUBJECT_NAME||'%', '%'))
      ORDER BY
        a.FI_SECURITY_CODE
    ) aa
    WHERE
    "rownumber" BETWEEN IN_ROW_START AND IN_ROW_END;
  END SP_MACHINERY_GET;

  PROCEDURE SP_MACHINERY_INSERT(
        IN_RECORD_TYPE IN VARCHAR2,
        IN_FI_CODE IN VARCHAR2,
        IN_BRANCH_CODE IN VARCHAR2,
        IN_SECURITY_VALUE_CODE IN VARCHAR2,
        IN_SECURITY_CATEGORY IN VARCHAR2,
        IN_FI_SECURITY_CODE IN VARCHAR2,
        IN_NAME_MACHINERY IN VARCHAR2,
        IN_NAME_FACTORY IN VARCHAR2,
        IN_ADDRESS_FACTORY IN VARCHAR2,
        IN_MFG_CO_BRAND_NAME IN VARCHAR2,
        IN_MFG_COUNTRY IN VARCHAR2,
        IN_MFG_YEAR IN NUMBER,
        IN_MODEL_NO IN VARCHAR2,
        IN_NO_UNIT IN NUMBER,
        IN_LC_NO IN VARCHAR2,
        IN_LC_DATE IN NUMBER,
        IN_VALUE_LC IN NUMBER,
        IN_LADING_AIR_WAY_BILL_NO IN VARCHAR2,
        IN_PRESENT_VALUE IN NUMBER,
        IN_BOOK_INVOICE_VALUE IN NUMBER,
        IN_MAKE_BY IN VARCHAR2,
        IN_MAKE_DATE IN DATE,
        IN_CHECK_BY IN VARCHAR2,
        IN_CHECK_DATE IN DATE,
        IN_RECORD_STATUS IN VARCHAR2,
        REF_CURSOR OUT SYS_REFCURSOR
    ) 
  AS
    TMP_FI_SECURITY_CODE VARCHAR2(16);
    S_EXISTS INT;
  BEGIN
    SELECT COUNT(1) INTO S_EXISTS FROM MACHINERIES WHERE FI_SECURITY_CODE = IN_FI_SECURITY_CODE;
    IF IN_FI_SECURITY_CODE IS NULL AND S_EXISTS = 0 THEN
       SELECT
        IN_RECORD_TYPE
        ||IN_FI_CODE
        ||IN_BRANCH_CODE
        ||LPAD(TO_CHAR(SEQ_SECURITY.NEXTVAL),8,'0')
      INTO
        TMP_FI_SECURITY_CODE
      FROM
        DUAL;
      INSERT INTO MACHINERIES VALUES (
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_SECURITY_VALUE_CODE,
        IN_SECURITY_CATEGORY,
        TMP_FI_SECURITY_CODE,
        IN_NAME_MACHINERY,
        IN_NAME_FACTORY,
        IN_ADDRESS_FACTORY,
        IN_MFG_CO_BRAND_NAME,
        IN_MFG_COUNTRY,
        IN_MFG_YEAR,
        IN_MODEL_NO,
        IN_NO_UNIT,
        IN_LC_NO,
        IN_LC_DATE,
        IN_VALUE_LC,
        IN_LADING_AIR_WAY_BILL_NO,
        IN_PRESENT_VALUE,
        IN_BOOK_INVOICE_VALUE,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        IN_RECORD_STATUS   
      );
    ELSE
      SP_MACHINERY_UPDATE(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_SECURITY_VALUE_CODE,
        IN_SECURITY_CATEGORY,
        IN_FI_SECURITY_CODE,
        IN_NAME_MACHINERY,
        IN_NAME_FACTORY,
        IN_ADDRESS_FACTORY,
        IN_MFG_CO_BRAND_NAME,
        IN_MFG_COUNTRY,
        IN_MFG_YEAR,
        IN_MODEL_NO,
        IN_NO_UNIT,
        IN_LC_NO,
        IN_LC_DATE,
        IN_VALUE_LC,
        IN_LADING_AIR_WAY_BILL_NO,
        IN_PRESENT_VALUE,
        IN_BOOK_INVOICE_VALUE,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        'U'
      );
    END IF;
    OPEN REF_CURSOR FOR SELECT NVL
    (
      TMP_FI_SECURITY_CODE,IN_FI_SECURITY_CODE
    )
    FROM DUAL;
  END SP_MACHINERY_INSERT;

  PROCEDURE SP_MORTGAGE_HIST_INSERT(
    IN_FI_MORTGAGE_CODE IN VARCHAR2
  )
  AS
  BEGIN
    INSERT INTO MORTGAGES_HIST
    SELECT
      SEQ_MORTGAGES_HIST.NEXTVAL,
      SYSDATE,
      A.*
    FROM MORTGAGES A
    WHERE
      A.FI_MORTGAGE_CODE = IN_FI_MORTGAGE_CODE;
  END SP_MORTGAGE_HIST_INSERT;

  PROCEDURE SP_MORTGAGE_UPDATE(
    IN_RECORD_TYPE IN VARCHAR2,
    IN_FI_CODE IN VARCHAR2,
    IN_BRANCH_CODE IN VARCHAR2,
    IN_FI_MORTGAGE_CODE IN VARCHAR2,
    IN_MORTGAGE_TYPE IN VARCHAR2,
    IN_MORTGAGE_DEED_NO IN VARCHAR2,
    IN_MORTGAGE_DATE IN NUMBER,
    IN_MORTGAGE_VALUE IN NUMBER,
    IN_MARKET_VALUE IN NUMBER,
    IN_RJSC_FILLING_NO IN VARCHAR2,
    IN_RJSC_FILLING_DATE IN NUMBER,
    IN_RANKING_CHARGE IN VARCHAR2,
    IN_PARI_PASSU_CHARGE IN VARCHAR2,
    IN_RIGPA_NO IN VARCHAR2,
    IN_RIGPA_DATE IN NUMBER,
    IN_MORTGAGE_PHASE IN VARCHAR2,
    IN_SECURITY_CATEGORY IN VARCHAR2,
    IN_MORTGAGED_LAND_AREA IN NUMBER,
    IN_MAKE_BY IN VARCHAR2,
    IN_MAKE_DATE IN DATE,
    IN_CHECK_BY IN VARCHAR2,
    IN_CHECK_DATE IN DATE,
    IN_RECORD_STATUS IN VARCHAR2
  )
  AS
    L_RECORD_STATUS VARCHAR2(1);
  BEGIN
    SELECT RECORD_STATUS INTO L_RECORD_STATUS FROM MORTGAGES WHERE FI_MORTGAGE_CODE = IN_FI_MORTGAGE_CODE;
    IF L_RECORD_STATUS != 'R' AND L_RECORD_STATUS != 'L' THEN
      SP_MORTGAGE_HIST_INSERT(IN_FI_MORTGAGE_CODE);
      UPDATE MORTGAGES
      SET
        RECORD_TYPE = IN_RECORD_TYPE,
        FI_CODE = IN_FI_CODE,
        BRANCH_CODE = IN_BRANCH_CODE,
        MORTGAGE_TYPE = IN_MORTGAGE_TYPE,
        MORTGAGE_DEED_NO = IN_MORTGAGE_DEED_NO,
        MORTGAGE_DATE = IN_MORTGAGE_DATE,
        MORTGAGE_VALUE = IN_MORTGAGE_VALUE,
        MARKET_VALUE = IN_MARKET_VALUE,
        RJSC_FILLING_NO = IN_RJSC_FILLING_NO,
        RJSC_FILLING_DATE = IN_RJSC_FILLING_DATE,
        RANKING_CHARGE = IN_RANKING_CHARGE,
        PARI_PASSU_CHARGE = IN_PARI_PASSU_CHARGE,
        RIGPA_NO = IN_RIGPA_NO,
        RIGPA_DATE = IN_RIGPA_DATE,
        MORTGAGE_PHASE = IN_MORTGAGE_PHASE,
        SECURITY_CATEGORY = IN_SECURITY_CATEGORY,
        MORTGAGED_LAND_AREA = IN_MORTGAGED_LAND_AREA,
        MAKE_BY = IN_MAKE_BY,
        MAKE_DATE = IN_MAKE_DATE,
        CHECK_BY = IN_CHECK_BY,
        CHECK_DATE = IN_CHECK_DATE,
        RECORD_STATUS = IN_RECORD_STATUS
      WHERE
        FI_MORTGAGE_CODE = IN_FI_MORTGAGE_CODE;
    END IF;
  END SP_MORTGAGE_UPDATE;

  PROCEDURE SP_MORTGAGE_INSERT(
        IN_RECORD_TYPE IN VARCHAR2,
        IN_FI_CODE IN VARCHAR2,
        IN_BRANCH_CODE IN VARCHAR2,
        IN_FI_MORTGAGE_CODE IN VARCHAR2,
        IN_MORTGAGE_TYPE IN VARCHAR2,
        IN_MORTGAGE_DEED_NO IN VARCHAR2,
        IN_MORTGAGE_DATE IN NUMBER,
        IN_MORTGAGE_VALUE IN NUMBER,
        IN_MARKET_VALUE IN NUMBER,
        IN_RJSC_FILLING_NO IN VARCHAR2,
        IN_RJSC_FILLING_DATE IN NUMBER,
        IN_RANKING_CHARGE IN VARCHAR2,
        IN_PARI_PASSU_CHARGE IN VARCHAR2,
        IN_RIGPA_NO IN VARCHAR2,
        IN_RIGPA_DATE IN NUMBER,
        IN_MORTGAGE_PHASE IN VARCHAR2,
        IN_SECURITY_CATEGORY IN VARCHAR2,
        IN_MORTGAGED_LAND_AREA IN NUMBER,
        IN_MAKE_BY IN VARCHAR2,
        IN_MAKE_DATE IN DATE,
        IN_CHECK_BY IN VARCHAR2,
        IN_CHECK_DATE IN DATE,
        IN_RECORD_STATUS IN VARCHAR2,
        REF_CURSOR OUT SYS_REFCURSOR
    ) 
  AS
    TMP_FI_MORTGAGE_CODE VARCHAR2(16);
    M_EXISTS INT;
  BEGIN
    SELECT COUNT(1) INTO M_EXISTS FROM MORTGAGES WHERE FI_MORTGAGE_CODE = IN_FI_MORTGAGE_CODE;
    IF IN_FI_MORTGAGE_CODE IS NULL AND M_EXISTS = 0 THEN
       SELECT
        IN_RECORD_TYPE
        ||IN_FI_CODE
        ||IN_BRANCH_CODE
        ||LPAD(TO_CHAR(SEQ_MORTGAGE.NEXTVAL),8,'0')
      INTO
        TMP_FI_MORTGAGE_CODE
      FROM
        DUAL;
      INSERT INTO MORTGAGES VALUES(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        TMP_FI_MORTGAGE_CODE,
        IN_MORTGAGE_TYPE,
        IN_MORTGAGE_DEED_NO,
        IN_MORTGAGE_DATE,
        IN_MORTGAGE_VALUE,
        IN_MARKET_VALUE,
        IN_RJSC_FILLING_NO,
        IN_RJSC_FILLING_DATE,
        IN_RANKING_CHARGE,
        IN_PARI_PASSU_CHARGE,
        IN_RIGPA_NO,
        IN_RIGPA_DATE,
        IN_MORTGAGE_PHASE,
        IN_SECURITY_CATEGORY,
        IN_MORTGAGED_LAND_AREA,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        IN_RECORD_STATUS        
      );
    ELSE
      SP_MORTGAGE_UPDATE(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_FI_MORTGAGE_CODE,
        IN_MORTGAGE_TYPE,
        IN_MORTGAGE_DEED_NO,
        IN_MORTGAGE_DATE,
        IN_MORTGAGE_VALUE,
        IN_MARKET_VALUE,
        IN_RJSC_FILLING_NO,
        IN_RJSC_FILLING_DATE,
        IN_RANKING_CHARGE,
        IN_PARI_PASSU_CHARGE,
        IN_RIGPA_NO,
        IN_RIGPA_DATE,
        IN_MORTGAGE_PHASE,
        IN_SECURITY_CATEGORY,
        IN_MORTGAGED_LAND_AREA,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        'U'
      );
    END IF;
    OPEN REF_CURSOR FOR SELECT NVL
    (
      TMP_FI_MORTGAGE_CODE,IN_FI_MORTGAGE_CODE
    )
    FROM DUAL;
  END SP_MORTGAGE_INSERT;

    PROCEDURE SP_MORTGAGE_GET_FILTERED(
        IN_BRANCH_CODE IN VARCHAR2,
        IN_FI_MORTGAGE_CODE IN VARCHAR2,
        IN_MORTGAGE_DEED_NO IN VARCHAR2,
        IN_MORTGAGE_DATE IN NUMBER,
        IN_MORTGAGE_VALUE IN NUMBER,
        REF_CURSOR OUT SYS_REFCURSOR
    )
    AS
    BEGIN
        OPEN REF_CURSOR FOR
        SELECT
            M.RECORD_TYPE "RecordType",
            M.FI_CODE "FiCode",
            M.BRANCH_CODE "BranchCode",
            M.FI_MORTGAGE_CODE "FiMortgageCode",
            M.MORTGAGE_TYPE "MortgageType",
            M.MORTGAGE_DEED_NO "MortgageDeedNo",
            TO_DATE(LPAD(M.MORTGAGE_DATE,8,'0'),'DDMMYYYY') "MortgageDate",
            M.MORTGAGE_VALUE "MortgageValue",
            M.MARKET_VALUE "MarketValue",
            M.RJSC_FILLING_NO "RjscFillingNo",
            TO_DATE(LPAD(M.RJSC_FILLING_DATE,8,'0'),'DDMMYYYY') "RjscFillingDate",
            M.RANKING_CHARGE "RankingCharge",
            M.PARI_PASSU_CHARGE "PariPassuCharge",
            M.RIGPA_NO "RigpaNo",
            TO_DATE(LPAD(M.RIGPA_DATE,8,'0'),'DDMMYYYY') "RigpaDate",
            M.MORTGAGE_PHASE "MortgagePhase",
            M.SECURITY_CATEGORY "SecurityCategory",
            M.MORTGAGED_LAND_AREA "MortgagedLandArea",
            M.MAKE_BY "MakeBy",
            M.MAKE_DATE "MakeDate",
            M.CHECK_BY "CheckBy",
            M.CHECK_DATE "CheckDate",
            M.RECORD_STATUS "RecordStatus"
        FROM
            MORTGAGES M
        WHERE
            M.BRANCH_CODE = NVL(IN_BRANCH_CODE, M.BRANCH_CODE)
        AND M.FI_MORTGAGE_CODE = NVL(IN_FI_MORTGAGE_CODE, M.FI_MORTGAGE_CODE)
        AND M.MORTGAGE_DEED_NO = NVL(IN_MORTGAGE_DEED_NO, M.MORTGAGE_DEED_NO)
        AND M.MORTGAGE_DATE = NVL(IN_MORTGAGE_DATE, M.MORTGAGE_DATE)
        AND M.MORTGAGE_VALUE = NVL(IN_MORTGAGE_VALUE, M.MORTGAGE_VALUE)
        AND M.RECORD_STATUS NOT IN ('D');            
    END SP_MORTGAGE_GET_FILTERED;

  PROCEDURE SP_HYPOTHECATION_HIST_INSERT (
    IN_FI_HYPOTHECATION_CODE IN VARCHAR2
  )
  AS    
  BEGIN
    INSERT INTO HYPOTHECATIONS_HIST
    SELECT
      SEQ_HYPOTHECATIONS_HIST.NEXTVAL,
      SYSDATE,
      A.*
    FROM HYPOTHECATIONS A
    WHERE
      A.FI_HYPOTHECATION_CODE = IN_FI_HYPOTHECATION_CODE;
  END SP_HYPOTHECATION_HIST_INSERT;

  PROCEDURE SP_HYPOTHECATION_UPDATE(
    IN_RECORD_TYPE IN VARCHAR2,
    IN_FI_CODE IN VARCHAR2,
    IN_BRANCH_CODE IN VARCHAR2,
    IN_FI_HYPOTHECATION_CODE IN VARCHAR2,
    IN_DATE_HYPOTHECATION IN NUMBER,
    IN_RJSC_FILLING_NO IN VARCHAR2,
    IN_RJSC_FILLING_DATE IN NUMBER,
    IN_RANKING_CHARGE IN VARCHAR2,
    IN_PARI_PASSU_CHARGE IN VARCHAR2,
    IN_HYPOTHECATION_PHASE IN VARCHAR2,
    IN_MAKE_BY IN VARCHAR2,
    IN_MAKE_DATE IN DATE,
    IN_CHECK_BY IN VARCHAR2,
    IN_CHECK_DATE IN DATE,
    IN_RECORD_STATUS IN VARCHAR2
  )
  AS
    L_RECORD_STATUS VARCHAR2(1);
  BEGIN
    SELECT RECORD_STATUS INTO L_RECORD_STATUS FROM HYPOTHECATIONS WHERE FI_HYPOTHECATION_CODE = IN_FI_HYPOTHECATION_CODE;
    IF L_RECORD_STATUS != 'R' AND L_RECORD_STATUS != 'L' THEN
      SP_HYPOTHECATION_HIST_INSERT(IN_FI_HYPOTHECATION_CODE);
      UPDATE HYPOTHECATIONS
      SET
        RECORD_TYPE = IN_RECORD_TYPE,
        FI_CODE = IN_FI_CODE,
        BRANCH_CODE = IN_BRANCH_CODE,        
        DATE_HYPOTHECATION = IN_DATE_HYPOTHECATION,
        RJSC_FILLING_NO = IN_RJSC_FILLING_NO,
        RJSC_FILLING_DATE = IN_RJSC_FILLING_DATE,
        RANKING_CHARGE = IN_RANKING_CHARGE,
        PARI_PASSU_CHARGE = IN_PARI_PASSU_CHARGE,
        HYPOTHECATION_PHASE = IN_HYPOTHECATION_PHASE,
        MAKE_BY = IN_MAKE_BY,
        MAKE_DATE = IN_MAKE_DATE,
        CHECK_BY = IN_CHECK_BY,
        CHECK_DATE = IN_CHECK_DATE,
        RECORD_STATUS = IN_RECORD_STATUS
      WHERE
        FI_HYPOTHECATION_CODE = IN_FI_HYPOTHECATION_CODE;
    END IF;
  END SP_HYPOTHECATION_UPDATE;

  PROCEDURE SP_HYPOTHECATION_INSERT(
    IN_RECORD_TYPE IN VARCHAR2,
    IN_FI_CODE IN VARCHAR2,
    IN_BRANCH_CODE IN VARCHAR2,
    IN_FI_HYPOTHECATION_CODE IN VARCHAR2,
    IN_DATE_HYPOTHECATION IN NUMBER,
    IN_RJSC_FILLING_NO IN VARCHAR2,
    IN_RJSC_FILLING_DATE IN NUMBER,
    IN_RANKING_CHARGE IN VARCHAR2,
    IN_PARI_PASSU_CHARGE IN VARCHAR2,
    IN_HYPOTHECATION_PHASE IN VARCHAR2,
    IN_MAKE_BY IN VARCHAR2,
    IN_MAKE_DATE IN DATE,
    IN_CHECK_BY IN VARCHAR2,
    IN_CHECK_DATE IN DATE,
    IN_RECORD_STATUS IN VARCHAR2,
    REF_CURSOR OUT SYS_REFCURSOR
    ) 
  AS
    TMP_FI_HYPOTHECATION_CODE VARCHAR2(16);
    H_EXISTS INT;
  BEGIN
    SELECT COUNT(1) INTO H_EXISTS FROM HYPOTHECATIONS WHERE FI_HYPOTHECATION_CODE = IN_FI_HYPOTHECATION_CODE;
    IF IN_FI_HYPOTHECATION_CODE IS NULL THEN
       SELECT
        IN_RECORD_TYPE
        ||IN_FI_CODE
        ||IN_BRANCH_CODE
        ||LPAD(TO_CHAR(SEQ_HYPOTHECATION.NEXTVAL),8,'0')
      INTO
        TMP_FI_HYPOTHECATION_CODE
      FROM
        DUAL;
      INSERT INTO HYPOTHECATIONS VALUES (
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        TMP_FI_HYPOTHECATION_CODE,
        IN_DATE_HYPOTHECATION,
        IN_RJSC_FILLING_NO,
        IN_RJSC_FILLING_DATE,
        IN_RANKING_CHARGE,
        IN_PARI_PASSU_CHARGE,
        IN_HYPOTHECATION_PHASE,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        IN_RECORD_STATUS
      );
    ELSE
      SP_HYPOTHECATION_UPDATE(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_FI_HYPOTHECATION_CODE,
        IN_DATE_HYPOTHECATION,
        IN_RJSC_FILLING_NO,
        IN_RJSC_FILLING_DATE,
        IN_RANKING_CHARGE,
        IN_PARI_PASSU_CHARGE,
        IN_HYPOTHECATION_PHASE,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        'U'
      );
    END IF;
    OPEN REF_CURSOR FOR SELECT NVL
    (
      TMP_FI_HYPOTHECATION_CODE,IN_FI_HYPOTHECATION_CODE
    )
    FROM DUAL;
  END SP_HYPOTHECATION_INSERT;

    PROCEDURE SP_HYPOTHECATION_GET_FILTERED(
        IN_BRANCH_CODE IN VARCHAR2,
        IN_FI_HYPOTHECATION_CODE IN VARCHAR2,
        IN_DATE_HYPOTHECATION IN NUMBER,
        IN_RJSC_FILLING_NO IN VARCHAR2,
        IN_RJSC_FILLING_DATE IN NUMBER,
        REF_CURSOR OUT SYS_REFCURSOR
    )
    AS
    BEGIN
        OPEN REF_CURSOR FOR
        SELECT
            H.RECORD_TYPE "RecordType",
            H.FI_CODE "FiCode",
            H.BRANCH_CODE "BranchCode",
            H.FI_HYPOTHECATION_CODE "FiHypothecationCode",
            TO_DATE(LPAD(H.DATE_HYPOTHECATION,8,'0'),'DDMMYYYY') "DateHypothecation",
            H.RJSC_FILLING_NO "RjscFillingNo",
            TO_DATE(LPAD(H.RJSC_FILLING_DATE,8,'0'),'DDMMYYYY') "RjscFillingDate",
            H.RANKING_CHARGE "RankingCharge",
            H.PARI_PASSU_CHARGE "PariPassuCharge",
            H.HYPOTHECATION_PHASE "HypothecationPhase",
            H.MAKE_BY "MakeBy",
            H.MAKE_DATE "MakeDate",
            H.CHECK_BY "CheckBy",
            H.CHECK_DATE "CheckDate",
            H.RECORD_STATUS "RecordStatus"
        FROM
            HYPOTHECATIONS H
        WHERE
            H.BRANCH_CODE = NVL(IN_BRANCH_CODE, H.BRANCH_CODE)
        AND H.FI_HYPOTHECATION_CODE = NVL(IN_FI_HYPOTHECATION_CODE, H.FI_HYPOTHECATION_CODE)
        AND (
            H.DATE_HYPOTHECATION = NVL(IN_DATE_HYPOTHECATION, H.DATE_HYPOTHECATION)
            OR H.DATE_HYPOTHECATION IS NULL
            )
        AND (
            H.RJSC_FILLING_NO = NVL(IN_RJSC_FILLING_NO, H.RJSC_FILLING_NO)
            OR H.RJSC_FILLING_NO IS NULL
            )
        AND (
            H.RJSC_FILLING_DATE = NVL(IN_RJSC_FILLING_DATE, H.RJSC_FILLING_DATE)
            OR H.RJSC_FILLING_DATE IS NULL
            )
        AND H.RECORD_STATUS NOT IN ('D');
    END SP_HYPOTHECATION_GET_FILTERED;

  PROCEDURE SP_SECURITY_LINK_INSERT(
    IN_RECORD_TYPE VARCHAR2,
    IN_FI_CODE VARCHAR2,
    IN_BRANCH_CODE VARCHAR2,
    IN_LINK_TYPE VARCHAR2,
    IN_FI_SECURITY_CODE VARCHAR2,
    IN_FI_MORT_HYPO_CODE VARCHAR2,
    IN_MAKE_BY VARCHAR2,
    IN_MAKE_DATE DATE,
    IN_CHECK_BY VARCHAR2,
    IN_CHECK_DATE DATE,
    IN_RECORD_STATUS VARCHAR2
  )
  AS
    L_EXISTS INT;
  BEGIN
    SELECT 
      COUNT(1) INTO L_EXISTS
    FROM
      SECURITY_LINKS
    WHERE
      FI_SECURITY_CODE = IN_FI_SECURITY_CODE
    AND
      FI_MORT_HYPO_CODE = IN_FI_MORT_HYPO_CODE
    AND
      LINK_TYPE = IN_LINK_TYPE;
    IF L_EXISTS = 0 THEN
      INSERT INTO SECURITY_LINKS VALUES(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_LINK_TYPE,
        IN_FI_SECURITY_CODE,
        IN_FI_MORT_HYPO_CODE,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        IN_RECORD_STATUS
      );
    ELSE
      UPDATE SECURITY_LINKS
      SET
        MAKE_BY = IN_MAKE_BY,
        MAKE_DATE = IN_MAKE_DATE,
        CHECK_BY = IN_CHECK_BY,
        CHECK_DATE = IN_CHECK_DATE,
        RECORD_STATUS = IN_RECORD_STATUS
      WHERE
        FI_SECURITY_CODE = IN_FI_SECURITY_CODE
      AND
        FI_MORT_HYPO_CODE = IN_FI_MORT_HYPO_CODE
      AND
        LINK_TYPE = IN_LINK_TYPE;
    END IF;
  END SP_SECURITY_LINK_INSERT;

  PROCEDURE SP_SUBJECT_LINK_INSERT(
    IN_RECORD_TYPE VARCHAR2,
    IN_FI_CODE VARCHAR2,
    IN_BRANCH_CODE VARCHAR2,
    IN_LINK_TYPE VARCHAR2,
    IN_FI_SUBJECT_CODE VARCHAR2,
    IN_FI_SEC_MORT_HYPO_CODE VARCHAR2,
    IN_ROLE VARCHAR2,
    IN_MAKE_BY VARCHAR2,
    IN_MAKE_DATE DATE,
    IN_CHECK_BY VARCHAR2,
    IN_CHECK_DATE DATE,
    IN_RECORD_STATUS VARCHAR2
  )
  AS
    L_EXISTS INT;
  BEGIN
    SELECT 
      COUNT(1) INTO L_EXISTS 
    FROM 
      SUBJECT_LINKS
    WHERE
      FI_SUBJECT_CODE = IN_FI_SUBJECT_CODE
    AND
      FI_SEC_MORT_HYPO_CODE = IN_FI_SEC_MORT_HYPO_CODE
    AND
      LINK_TYPE = IN_LINK_TYPE
    AND
      ROLE = IN_ROLE;

    IF L_EXISTS = 0 THEN    
      INSERT INTO SUBJECT_LINKS VALUES(
        IN_RECORD_TYPE,
        IN_FI_CODE,
        IN_BRANCH_CODE,
        IN_LINK_TYPE,
        IN_FI_SUBJECT_CODE,
        IN_FI_SEC_MORT_HYPO_CODE,
        IN_ROLE,
        IN_MAKE_BY,
        IN_MAKE_DATE,
        IN_CHECK_BY,
        IN_CHECK_DATE,
        IN_RECORD_STATUS
      );
    ELSE
      UPDATE SUBJECT_LINKS 
      SET
        MAKE_BY = IN_MAKE_BY,
        MAKE_DATE = IN_MAKE_DATE,
        CHECK_BY = IN_CHECK_BY,
        CHECK_DATE = IN_CHECK_DATE,
        RECORD_STATUS = IN_RECORD_STATUS
      WHERE
        FI_SUBJECT_CODE = IN_FI_SUBJECT_CODE
      AND
        FI_SEC_MORT_HYPO_CODE = IN_FI_SEC_MORT_HYPO_CODE
      AND
        LINK_TYPE = IN_LINK_TYPE
      AND
        ROLE = IN_ROLE;
    END IF;
  END SP_SUBJECT_LINK_INSERT;

END PKG_SECURITY;
