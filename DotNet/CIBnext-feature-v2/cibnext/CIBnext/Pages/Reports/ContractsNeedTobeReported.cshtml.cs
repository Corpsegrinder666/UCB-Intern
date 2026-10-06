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
    [Authorize(Roles = "5002,ADMIN")]
    public class ContractsNeedTobeReportedModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly ReportBLL _reportBLL;

        public ContractsNeedTobeReportedModel(ListOfValuesBLL lovBLL, ReportBLL reportBLL)
        {
            _lovBLL = lovBLL;
            _reportBLL = reportBLL;
        }

        [BindProperty(SupportsGet = true)]
        public string BranchCode { get; set; }

        public string ValidationMessage { get; set; }
        public List<SelectListItem> BranchCodeList { get; set; }

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
            var contracts = await _reportBLL.GetContractsNeedTobeReported(cibBranchCode);

            if (!contracts.Any())
            {
                ValidationMessage = "No data found";
                return Page();
            }

            var fileBytes = GenerateReport(contracts, genDate);
            return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Report.xlsx");
        }

        private byte[] GenerateReport(IEnumerable<ContractNeedTobeReportedInfo> contracts, DateTime? genDate)
        {
            var appUser = AppSession.GetUserInfo(User);
            var reportingPeriod = appUser.Parameters?.ReportingPeriod ?? DateTime.Today;

            using (var ep = new ExcelPackage())
            {
                int endCol = 36;
                ExcelWorksheet ws = ep.Workbook.Worksheets.Add("Report");

                ExcelReportHelper.SetTitleRow(ws, 1, "List of contract need to be submitted to BB", endCol, 16, true);
                ExcelReportHelper.SetTitleRow(ws, 2, $"Reporting Month: {reportingPeriod:MMMM-yyyy}", endCol, 14, true);
                ExcelReportHelper.SetTitleRow(ws, 3, $"Branch Name: {BranchCodeList.FirstOrDefault(x => x.Value == BranchCode)?.Text ?? "All"}", endCol, 14, true);
                ExcelReportHelper.SetTitleRow(ws, 4, $"Generation Date: {genDate:dd-MMM-yyyy}", endCol, 14, false);

                int j = 0;
                string[] headers =
                {
                    "BranchName","CustomerID","CustomerName","AccountNumber","LimitAmount","AccountLimit",
                    "OutstandingAmount","Overdue","OverdueDays","AmountPaidLastMonth","TotalAmountPaid",
                    "OpeningDate","ExpiryDate","Status","NumberOfTimesRescheduled","LastRescheduledate",
                    "ProductCode","ProductDescription","SecurityValue","SectorCode","EconomicPurposeCode",
                    "LastClassificationDateP","SmeCode","EmiAmount","InstallmentSizeForCL","InstallmentFrequency",
                    "NoOfInstallmentDue","LawSuitDate","NoOfInstallmentPaid","InstalmentFiContractCode",
                    "InstalmentFiSubjectCode","NonInstalmentFiContractCode","NonInstalmentFiSubjectCode",
                    "TotalNoOfInstalment","NoOfRemainingInstalment","NoOfOverdueInstalment"
                };
                foreach (var h in headers) ws.Cells[5, ++j].Value = h;

                ws.Cells[5, 1, 5, j].Style.Font.Bold = true;
                ExcelReportHelper.SetBorder(ws.Cells[5, 1, 5, j]);

                int i = 6;
                foreach (var contract in contracts)
                {
                    j = 0;
                    ws.Cells[i, ++j].Value = contract.BranchName;
                    ws.Cells[i, ++j].Value = contract.CustomerID;
                    ws.Cells[i, ++j].Value = contract.CustomerName;
                    ws.Cells[i, ++j].Value = contract.AccountNumber;
                    ws.Cells[i, ++j].Value = contract.LimitAmount;
                    ws.Cells[i, ++j].Value = contract.AccountLimit;
                    ws.Cells[i, ++j].Value = contract.OutstandingAmount;
                    ws.Cells[i, ++j].Value = contract.Overdue;
                    ws.Cells[i, ++j].Value = contract.OverdueDays;
                    ws.Cells[i, ++j].Value = contract.AmountPaidLastMonth;
                    ws.Cells[i, ++j].Value = contract.TotalAmountPaid;
                    ws.Cells[i, ++j].Value = contract.OpeningDate.ToString("dd-MMM-yyyy");
                    ws.Cells[i, ++j].Value = contract.ExpiryDate.HasValue ? contract.ExpiryDate.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.Status;
                    ws.Cells[i, ++j].Value = contract.NumberOfTimesRescheduled;
                    ws.Cells[i, ++j].Value = contract.LastRescheduleDate.HasValue ? contract.LastRescheduleDate.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.ProductCode;
                    ws.Cells[i, ++j].Value = contract.ProductDescription;
                    ws.Cells[i, ++j].Value = contract.SecurityValue;
                    ws.Cells[i, ++j].Value = contract.SectorCode;
                    ws.Cells[i, ++j].Value = contract.EconomicPurposeCode;
                    ws.Cells[i, ++j].Value = contract.LastClassificationDate.HasValue ? contract.LastClassificationDate.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.SmeCode;
                    ws.Cells[i, ++j].Value = contract.EmiAmount;
                    ws.Cells[i, ++j].Value = contract.InstallmentSizeForCL;
                    ws.Cells[i, ++j].Value = contract.InstallmentFrequency;
                    ws.Cells[i, ++j].Value = contract.NoOfInstallmentDue;
                    ws.Cells[i, ++j].Value = contract.LawSuitDate.HasValue ? contract.LawSuitDate.Value.ToString("dd-MMM-yyyy") : null;
                    ws.Cells[i, ++j].Value = contract.NoOfInstallmentPaid;
                    ws.Cells[i, ++j].Value = contract.InstalmentFiContractCode;
                    ws.Cells[i, ++j].Value = contract.InstalmentFiSubjectCode;
                    ws.Cells[i, ++j].Value = contract.NonInstalmentFiContractCode;
                    ws.Cells[i, ++j].Value = contract.NonInstalmentFiSubjectCode;
                    ws.Cells[i, ++j].Value = contract.TotalNoOfInstalment;
                    ws.Cells[i, ++j].Value = contract.NoOfRemainingInstalment;
                    ws.Cells[i, ++j].Value = contract.NoOfOverdueInstalment;

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
        }
    }
}
