create or replace PACKAGE BODY PKG_REPORT AS

PROCEDURE SP_ACCS_READY_TO_REPORT(
    IN_CIB_BRANCH_CODE IN VARCHAR2,
    IN_CONTRACT_PHASE IN VARCHAR2,
    IN_CONTRACT_STATUS in VARCHAR2,
    REF_CURSOR OUT SYS_REFCURSOR)
    AS
    P_RPT_PERIOD DATE;
    BEGIN
        SELECT TO_DATE(VALUE, 'DD MON YYYY') INTO P_RPT_PERIOD FROM REPORTING_PARAMETERS rp WHERE "KEY" = 'REPORTING_PERIOD';

        OPEN REF_CURSOR FOR
        SELECT
            b.BRANCH_CODE "BranchCode",
            b.BRANCH_NAME "BranchName",
            ubs_account_no "UbsAccountNo",
            ic.fi_contract_code "FiContractCode",
            lct.NAME "ContractType",
            vs.CIF_NO "CifNo",
            ic.fi_subject_code "FiSubjectCode",
            vs.SUBJECT_NAME "SubjectName",
            ll.CUSTOMER_NAME "CustomerName",
            ic.STARTING_DATE "StartingDate",
            ic.TOTAL_DISBURSED_AMOUNT "TotalDisbursedAmount",
            lcp.NAME "ContractPhase",
            ic.OVERDUE_AMOUNT "OverdueAmount",
            ic.NO_OF_OVERDUE_INSTALMENT "NoOfOverdueInstalment",
            ic.NO_OF_REMAINING_INSTALMENT "NoOfRemainingInstalment",
            ic.TOTAL_OUTSTANDING_AMOUNT "TotalOutstandingAmount",
            'Installment' "I_NI",
            lcs.NAME "ContractStatus",
            cd.Status "CLStatus",
            ic.DATE_OF_CLASSIFICATION "DateOfClassification",
            ic.PLANNED_END_DATE "PlannedEndDate",
            ic.ACTUAL_END_DATE "ActualEndDate",
            NULL "Remarks",
            ic.DEFAULTER_STATUS "DefaulterStatus",
            ic.TOTAL_NUMBER_INSTALMENTS "TotalInstallmentNo",
            ic.INSTALMENT_AMOUNT "InstallmentAmount",
            ic.NO_TIMES_RESCHEDULING "NoOfTimesRescheduling",
            ic.DATE_LAST_RESCHEDULING "DateOfLastRescheduling",
            ic.RECORD_STATUS "RecordStatus"
        FROM
            instalment_contracts ic
        JOIN vw_subjects vs
        ON ic.FI_SUBJECT_CODE = vs.FI_SUBJECT_CODE
        JOIN INSTALMENT_CONTRACT_REPORT icr
        ON ic.FI_CONTRACT_CODE = icr.FI_CONTRACT_CODE
        JOIN BRANCHES b
        ON ic.BRANCH_CODE = b.CIB_BRANCH_CODE
        AND b.BRANCH_CODE != '9001'
        JOIN LOV_CONTRACT_TYPES lct
        ON ic.CONTRACT_TYPE = lct.CODE
        JOIN LOV_CONTRACT_STATUS lcs
        ON ic.CONTRACT_STATUS = lcs.CODE
        JOIN LOV_CONTRACT_PHASES lcp
        ON ic.CONTRACT_PHASE = lcp.CODE
        LEFT JOIN LOAN_LIST ll
        ON ic.UBS_ACCOUNT_NO = ll.CONT_REF_NO
        LEFT JOIN CL_DATA cd
        on ic.UBS_ACCOUNT_NO = cd.ACCOUNT_NO
        WHERE
            icr.REPORTING_PERIOD = P_RPT_PERIOD
        AND
            ic.RECORD_STATUS NOT IN ('T', 'D')
        AND
            ic.BRANCH_CODE = NVL(IN_CIB_BRANCH_CODE, ic.BRANCH_CODE)
        AND
            NVL(IN_CONTRACT_PHASE, 'ALL') = CASE WHEN IN_CONTRACT_PHASE IS NOT NULL THEN
                                                 CASE WHEN ic.CONTRACT_PHASE = 'LV' THEN 'LV'
                                                      WHEN ic.CONTRACT_PHASE = 'TM' THEN 'T'
                                                      WHEN ic.CONTRACT_PHASE = 'TA' THEN 'T'
                                                      ELSE 'LV' END
                                                 ELSE 'ALL' END
        AND
            ic.CONTRACT_STATUS = CASE WHEN IN_CONTRACT_STATUS IS NULL THEN ic.CONTRACT_STATUS ELSE IN_CONTRACT_STATUS END
        UNION ALL
        SELECT
            b.BRANCH_CODE,
            b.BRANCH_NAME,
            ubs_account_no,
            nic.fi_contract_code,
            lct.NAME "ContractType",
            vs.CIF_NO,
            nic.fi_subject_code,
            vs.SUBJECT_NAME ,
            ll.CUSTOMER_NAME "CustomerName",
            nic.STARTING_DATE,
            0,
            lcp.NAME "ContractPhase",
            nic.DUE_FOR_RECOVERY,
            0,
            0,
            nic.TOTAL_OUTSTANDING_AMOUNT,
            'Non Installment' "I/NI",
            lcs.NAME "ContractStatus",
            cd.Status "CL Status",
            nic.DATE_OF_CLASSIFICATION,
            nic.PLANNED_END_DATE,
            nic.ACTUAL_END_DATE,
            NULL "Remarks",
            nic.DEFAULTER_STATUS "DefaulterStatus",
            0 "TotalInstallmentNo",
            0 "InstallmentAmount",
            nic.NO_OF_TIMES_RESCHEDULING "NoOfTimesRescheduling",
            nic.DATE_LAST_RESCHEDULING "DateOfLastRescheduling",
            nic.RECORD_STATUS
        FROM
            non_instalment_contracts nic
        JOIN vw_subjects vs
        ON nic.FI_SUBJECT_CODE = vs.FI_SUBJECT_CODE
        JOIN NON_INSTALMENT_CONTRACT_REPORT nicr
        ON nic.FI_CONTRACT_CODE = nicr.FI_CONTRACT_CODE
        JOIN BRANCHES b
        ON nic.BRANCH_CODE = b.CIB_BRANCH_CODE
        AND b.BRANCH_CODE != '9001'
        JOIN LOV_CONTRACT_TYPES lct
        ON nic.CONTRACT_TYPE = lct.CODE
        JOIN LOV_CONTRACT_STATUS lcs
        ON nic.CONTRACT_STATUS = lcs.CODE
        JOIN LOV_CONTRACT_PHASES lcp
        ON nic.CONTRACT_PHASE = lcp.CODE
        LEFT JOIN LOAN_LIST ll
        ON nic.UBS_ACCOUNT_NO = ll.CONT_REF_NO
        LEFT JOIN CL_DATA cd
        on nic.UBS_ACCOUNT_NO = cd.ACCOUNT_NO
        WHERE
            nicr.REPORTING_PERIOD = P_RPT_PERIOD
        AND
            nic.RECORD_STATUS NOT IN ('T', 'D')
        AND
            nic.BRANCH_CODE = NVL(IN_CIB_BRANCH_CODE, nic.BRANCH_CODE)
        AND
            NVL(IN_CONTRACT_PHASE, 'ALL') = CASE WHEN IN_CONTRACT_PHASE IS NOT NULL THEN
                                                 CASE WHEN nic.CONTRACT_PHASE = 'LV' THEN 'LV'
                                                      WHEN nic.CONTRACT_PHASE = 'TM' THEN 'T'
                                                      WHEN nic.CONTRACT_PHASE = 'TA' THEN 'T'
                                                      ELSE 'LV' END
                                                 ELSE 'ALL' END
        AND
            nic.CONTRACT_STATUS = CASE WHEN IN_CONTRACT_STATUS IS NULL THEN nic.CONTRACT_STATUS ELSE IN_CONTRACT_STATUS END;
    END SP_ACCS_READY_TO_REPORT;

PROCEDURE SP_NEED_TOBE_REPORTED(
    IN_CIB_BRANCH_CODE IN VARCHAR2,
    REF_CURSOR OUT SYS_REFCURSOR)
    AS
    P_RPT_PERIOD DATE;
    BEGIN
        SELECT TO_DATE(VALUE, 'DD MON YYYY') INTO P_RPT_PERIOD FROM REPORTING_PARAMETERS rp WHERE "KEY" = 'REPORTING_PERIOD';

        INSERT INTO TMP_AC_CODES
        SELECT
            ubs_account_no
        FROM
            instalment_contracts ic
        JOIN INSTALMENT_CONTRACT_REPORT icr
        ON ic.FI_CONTRACT_CODE = icr.FI_CONTRACT_CODE
        WHERE
            icr.REPORTING_PERIOD = P_RPT_PERIOD
        AND
            ic.RECORD_STATUS NOT IN ('T', 'D')
        AND
            ic.BRANCH_CODE = NVL(IN_CIB_BRANCH_CODE, ic.BRANCH_CODE)
        UNION ALL
        SELECT
            ubs_account_no
        FROM
            non_instalment_contracts nic
        JOIN NON_INSTALMENT_CONTRACT_REPORT nicr
        ON nic.FI_CONTRACT_CODE = nicr.FI_CONTRACT_CODE
        WHERE
            nicr.REPORTING_PERIOD = P_RPT_PERIOD
        AND
            nic.RECORD_STATUS NOT IN ('T', 'D')
        AND
            nic.BRANCH_CODE = NVL(IN_CIB_BRANCH_CODE, nic.BRANCH_CODE);

        OPEN REF_CURSOR FOR
        select b.branch_name "BranchName",
               a.customer_id "CustomerID",
               a.customer_name "CustomerName",
               a.cont_ref_no "AccountNumber",
               Limit_Amount "LimitAmount",
               a.account_limit "AccountLimit",
               a.outstanding_local_currency + a.interest_outstanding "OutstandingAmount",
               a.Overdue "Overdue",
               a.Overdue_days "OverdueDays",
               Amount_paid_last_Mnth "AmountPaidLastMonth",
               Total_amount_paid "TotalAmountPaid",
               TO_DATE(Value_date, 'DD-MM-YYYY') "OpeningDate",
               TO_DATE(a.maturity_date, 'DD-MM-YYYY') "ExpiryDate",
               a.status1 as "Status",
               a.NUMBER_OF_RESCHEDULED "NumberOfTimesRescheduled",
               a.LAST_RESCHEDULE_DATE "LastRescheduledate",
               a.Product_code "ProductCode",
               a.Proddes "ProductDescription",
               a.Security_value "SecurityValue",
               a.Sect_code "SectorCode",
               a.Eco_Purpose_code "EconomicPurposeCode",
               a.Last_classification_date "LastClassificationDate",
               a.SME_code "SmeCode",
               a.EMI_Amount "EmiAmount",
               Installment_size_for_CL "InstallmentSizeForCL",
               Installment_Frequence "InstallmentFrequency",
               No_of_intallment_due "NoOfInstallmentDue",
               Law_Suit_date "LawSuitDate",
               No_of_intallment_paid "NoOfInstallmentPaid",
               (select fi_contract_code from instalment_contracts iic where iic.ubs_account_no = a.CONT_REF_NO and iic.record_status != 'D') "InstalmentFiContractCode",
               (select fi_subject_code from instalment_contracts iic where iic.ubs_account_no = a.CONT_REF_NO and iic.record_status != 'D') "InstalmentFiSubjectCode",
               (select TOTAL_NUMBER_INSTALMENTS from instalment_contracts iic where iic.ubs_account_no = a.CONT_REF_NO and iic.record_status != 'D') "TotalNoOfInstalment",
               (select NO_OF_REMAINING_INSTALMENT from instalment_contracts iic where iic.ubs_account_no = a.CONT_REF_NO and iic.record_status != 'D') "NoOfRemainingInstalment",
               (select NO_OF_OVERDUE_INSTALMENT from instalment_contracts iic where iic.ubs_account_no = a.CONT_REF_NO and iic.record_status != 'D') "NoOfOverdueInstalment",
               (select fi_contract_code from non_instalment_contracts inic where inic.ubs_account_no = a.CONT_REF_NO and inic.record_status != 'D') "NonInstalmentFiContractCode",
               (select fi_subject_code from non_instalment_contracts inic where inic.ubs_account_no = a.CONT_REF_NO and inic.record_status != 'D') "NonInstalmentFiSubjectCode"
          from LOAN_LIST a join BRANCHES b
                on a.branch_code=substr(b.branch_code,2,3)
               AND b.branch_code != '9001'
         where a.branch_code not in ('789','ECAR')
           and Product_code NOT IN ('AFRS01', 'ECAR')
           AND b.CIB_BRANCH_CODE = NVL(IN_CIB_BRANCH_CODE, b.CIB_BRANCH_CODE)
           AND a.CONT_REF_NO NOT IN (
               SELECT fi_code FROM TMP_AC_CODES where fi_code IS NOT NULL
           );
    END SP_NEED_TOBE_REPORTED;

END PKG_REPORT;
