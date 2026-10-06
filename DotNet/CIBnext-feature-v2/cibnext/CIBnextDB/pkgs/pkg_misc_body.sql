CREATE OR REPLACE PACKAGE BODY PKG_MISC 
AS
PROCEDURE SP_GET_PARAMETERS(
    REF_CURSOR OUT SYS_REFCURSOR)
AS
BEGIN
  OPEN REF_CURSOR FOR
  SELECT
  KEY ,
  VALUE
  FROM
  REPORTING_PARAMETERS;
END SP_GET_PARAMETERS;

PROCEDURE SP_GET_BRANCHES(
    REF_CURSOR OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN REF_CURSOR FOR
    SELECT 
        BRANCH_CODE "BranchCode",
        BRANCH_NAME "BranchName",
        CIB_BRANCH_CODE "CibBranchCode",
        MAKE_BY "MakeBy",
        MAKE_DATE "MakeDate"
    FROM BRANCHES
    ORDER BY BRANCH_NAME;
END SP_GET_BRANCHES;


PROCEDURE SP_CLR_ERR_AND_RPT_OF_CURR_MTH
AS
rep_period DATE;
BEGIN

select to_Date(a.value) into rep_period from REPORTING_PARAMETERS a where a.key = 'REPORTING_PERIOD';

execute immediate 'truncate table CARD_CONTRACT_ERROR';
execute immediate 'truncate table CONTRACT_LINK_ERROR';
execute immediate 'truncate table INSTALMENT_CONTRACT_ERROR';
execute immediate 'truncate table INSTITUTION_ERROR';
execute immediate 'truncate table NON_INSTALMENT_CONTRACT_ERROR';
execute immediate 'truncate table OWNER_LINK_ERROR';
execute immediate 'truncate table PERSONAL_DATA_ERROR';

delete from CARD_CONTRACT_REPORT where reporting_period=rep_period;
delete from CONTRACT_LINK_REPORT where reporting_period=rep_period;
delete from INSTALMENT_CONTRACT_REPORT where reporting_period=rep_period;
delete from INSTITUTION_REPORT where reporting_period=rep_period;
delete from NON_INSTALMENT_CONTRACT_REPORT where reporting_period=rep_period;
delete from OWNER_LINK_REPORT where reporting_period=rep_period;
delete from PERSONAL_DATA_REPORT where reporting_period=rep_period;
END SP_CLR_ERR_AND_RPT_OF_CURR_MTH;


END PKG_MISC;
