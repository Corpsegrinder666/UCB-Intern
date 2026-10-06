CREATE OR REPLACE VIEW VW_SECUIRY_SUBJECT_REPORT 
(
    VERSION_NO,
    REPORTING_PERIOD,
    FI_SUBJECT_CODE,
    MAKE_BY,
    MAKE_DATE
) AS
SELECT
    version_no,
    reporting_period,
    fi_subject_code,
    make_by,
    make_date
FROM
    security_person_report
UNION ALL
SELECT
    version_no,
    reporting_period,
    fi_subject_code,
    make_by,
    make_date
FROM
    security_institution_report;
