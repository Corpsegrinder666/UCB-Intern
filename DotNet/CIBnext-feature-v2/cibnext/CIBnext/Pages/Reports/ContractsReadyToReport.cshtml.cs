using CIBnext.BLL;
using CIBnext.DAO;
using CIBnext.LIB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIBnext.Pages.Reports
{
    [Authorize(Roles = "5001,ADMIN")]
    public class ContractsReadyToReportModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly ReportBLL _reportBLL;

        public ContractsReadyToReportModel(ListOfValuesBLL lovBLL, ReportBLL reportBLL)
        {
            _lovBLL = lovBLL;
            _reportBLL = reportBLL;
        }

        [BindProperty(SupportsGet = true)]
        public string BranchCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ContractPhase { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ContractStatus { get; set; }

        public string ValidationMessage { get; set; }
        public List<SelectListItem> BranchCodeList { get; set; }
        public List<SelectListItem> ContractPhaseList { get; set; }
        public List<SelectListItem> ContractStatusList { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
        }

        public async Task<IActionResult> OnPostDownload()
        {
            LoadDropdowns();

            var appUser = AppSession.GetUserInfo(User);
            var cibBranchCode = appUser.BranchCode != "0000"
                ? appUser.CibBranchCode
                : (string.IsNullOrEmpty(BranchCode) ? null : BranchCode);

            var genDate = await _reportBLL.GetLastValidatorRunDate();
            var contracts = await _reportBLL.GetContractsReadyToReport(cibBranchCode, ContractPhase, ContractStatus);

            if (!contracts.Any())
            {
                ValidationMessage = "No data found";
                return Page();
            }

            var fileBytes = GenerateReport(contracts, genDate, cibBranchCode);
            return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Report.xlsx");
        }

        private byte[] GenerateReport(IEnumerable<ContractReadyToReportInfo> contracts, DateTime? genDate, string cibBranchCode)
        {
            var appUser = AppSession.GetUserInfo(User);
            var reportingPeriod = appUser.Parameters?.ReportingPeriod ?? DateTime.Today;

            using (var ep = new ExcelPackage())
            {
                int end = string.IsNullOrWhiteSpace(cibBranchCode) ? 28 : 26;
                ExcelWorksheet ws = ep.Workbook.Worksheets.Add("Report");

                ExcelReportHelper.SetTitleRow(ws, 1, "List of Ready Error Free Accounts being submitted to BB", end, 16, true);
                ExcelReportHelper.SetTitleRow(ws, 2, $"Reporting Month: {reportingPeriod:MMMM-yyyy}", end, 14, true);
                ExcelReportHelper.SetTitleRow(ws, 3, $"Branch Name: {BranchCodeList.FirstOrDefault(x => x.Value == BranchCode)?.Text ?? "All"}", end, 14, true);
                ExcelReportHelper.SetTitleRow(ws, 4, $"Generation Date: {genDate:dd-MMM-yyyy}", end, 14, false);

                int j = 0;
                if (string.IsNullOrEmpty(cibBranchCode))
                {
                    ws.Cells[5, ++j].Value = "Branch Code";
                    ws.Cells[5, ++j].Value = "Branch Name";
                }

                string[] headers =
                {
                    "Loan A/C number","Contract code","Contract Type","CIF","Borrower subject code","Subject name",
                    "Customer name","Starting Date of Contract","Total Disbursed Amount","Contract Phase",
                    "Overdue Amount","Number of Overdue Installments","Number of Remaining Installments","O/S",
                    "Instalment/Non-Instalment Contract","Loan A/C status(as per CIB database)",
                    "Loan A/C status(as per last quarter CL reporting)","Date of Classification",
                    "Planned End Date of Contract","Actual End Date of Contract","Defaulter","Remarks",
                    "Total Installment No","Installment Amount","No of Time Rescheduling","Date of Last Rescheduling"
                };
                foreach (var h in headers) ws.Cells[5, ++j].Value = h;

                ws.Cells[5, 1, 5, j].Style.Font.Bold = true;
                ExcelReportHelper.SetBorder(ws.Cells[5, 1, 5, j]);

                int i = 6;
                foreach (var contract in contracts)
                {
                    j = 0;
                    if (string.IsNullOrEmpty(cibBranchCode))
                    {
                        ws.Cells[i, ++j].Value = contract.BranchCode;
                        ws.Cells[i, ++j].Value = contract.BranchName;
                    }
                    ws.Cells[i, ++j].Value = contract.UbsAccountNo;
                    ws.Cells[i, ++j].Value = contract.FiContractCode;
                    ws.Cells[i, ++j].Value = contract.ContractType;
                    ws.Cells[i, ++j].Value = contract.CifNo;
                    ws.Cells[i, ++j].Value = contract.FiSubjectCode;
                    ws.Cells[i, ++j].Value = contract.SubjectName;
                    ws.Cells[i, ++j].Value = contract.CustomerName;
                    ws.Cells[i, ++j].Value = contract.StartingDate.HasValue ? contract.StartingDate.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.TotalDisbursedAmount;
                    ws.Cells[i, ++j].Value = contract.ContractPhase;
                    ws.Cells[i, ++j].Value = contract.OverdueAmount;
                    ws.Cells[i, ++j].Value = contract.NoOfOverdueInstalment;
                    ws.Cells[i, ++j].Value = contract.NoOfRemainingInstalment;
                    ws.Cells[i, ++j].Value = contract.TotalOutstandingAmount;
                    ws.Cells[i, ++j].Value = contract.I_NI;
                    ws.Cells[i, ++j].Value = contract.ContractStatus;
                    ws.Cells[i, ++j].Value = contract.CLStatus;
                    ws.Cells[i, ++j].Value = contract.DateOfClassification.HasValue ? contract.DateOfClassification.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.PlannedEndDate.HasValue ? contract.PlannedEndDate.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.ActualEndDate.HasValue ? contract.ActualEndDate.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.DefaulterStatus == "Y" ? "Yes" : contract.DefaulterStatus == "W" ? "Wilful" : "No";
                    ws.Cells[i, ++j].Value = contract.Remarks;
                    ws.Cells[i, ++j].Value = contract.TotalInstallmentNo;
                    ws.Cells[i, ++j].Value = contract.InstallmentAmount;
                    ws.Cells[i, ++j].Value = contract.NoOfTimesRescheduling;
                    ws.Cells[i, ++j].Value = contract.DateOfLastRescheduling.HasValue ? contract.DateOfLastRescheduling.Value.ToString("dd-MMM-yyyy") : null;

                    ExcelReportHelper.SetBorder(ws.Cells[i, 1, i, j]);
                    i++;
                }

                return ep.GetAsByteArray();
            }
        }

        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL.GetListOfValues(LovTypes.BRANCHES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            ContractPhaseList = _lovBLL.GetListOfValues(LovTypes.CONTRACT_PHASES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            ContractStatusList = _lovBLL.GetListOfValues(LovTypes.CONTRACT_STATUS)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
        }
    }
}
