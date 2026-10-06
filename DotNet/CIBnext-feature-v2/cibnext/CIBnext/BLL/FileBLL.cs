//using CIBnext.ContractUI;
using CIBnext.DAL;
using CIBnext.LIB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CIBnext.BLL
{
    public class FileBLL
    {
        FileDAL fileDal = new FileDAL();

        //AppSession.Parameters.ReportingPeriod
        public void GenerateSubjectFile(DateTime reportingPeriod)
        {
            int serial = GetSerialNumber();

            string[] lines = fileDal.GetSubjectFileData(reportingPeriod);
            lines = lines.Skip(1).Take(lines.Length - 2).ToArray();

            var header = $"H046{reportingPeriod:ddMMyyyy}{DateTime.Now:ddMMyyyy}{serial:000}{new String(' ', 1077)}";
            var footer = $"Q046{reportingPeriod:ddMMyyyy}{DateTime.Now:ddMMyyyy}{lines.Length:0000000}{new String(' ', 1073)}";

            lines = (new[] { header }).Concat(lines).Concat(new[] { footer }).ToArray();

            string directoryName = WebConfig.CibDocs + @"\Temp";
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            string fileName = directoryName + @"\046SJF.txt";

            File.WriteAllLines(fileName, lines);
        }

        public void GenerateSecuritySubjectFile()
        {
            int serial = GetSecuritySerialNumber();

            string[] lines = fileDal.GetSecuritySubjectFileData();

            var newVal = "00000000" + (lines.Length - 2);
            newVal = newVal.Substring(newVal.Length - 8);
            lines[lines.Length - 1] = lines[lines.Length - 1].Replace("XXXXXXXX", newVal);
            lines[0] = lines[0].Replace("RRRRRRRR", newVal);

            newVal = "000" + serial;
            newVal = newVal.Substring(newVal.Length - 3);
            lines[0] = lines[0].Replace("XXX", newVal);

            string directoryName = WebConfig.CibDocs + @"\Temp\security";
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            string fileName = directoryName + @"\046SUBJECTINFO.txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        public void GenerateContractFile(DateTime reportingPeriod)
        {
            string[] lines = fileDal.GetContractFileData(reportingPeriod);
            var le = lines.Length;
            lines = lines.Skip(1).Take(le - 2).ToArray();

            const int NO_OF_RECORDS = 300000;
            for (int i = 0; i <= (int) (lines.Length / NO_OF_RECORDS); i++)
            { 
                int serial = GetSerialNumber();
                var tlines = lines.Skip(i * NO_OF_RECORDS).Take(NO_OF_RECORDS).ToArray();

                var serialStr = "000" + serial;
                serialStr = serialStr.Substring(serialStr.Length - 3);

                var newVal = "00000" + (lines.Length - 2);
                newVal = newVal.Substring(newVal.Length - 7);

                var header = $"H046{reportingPeriod:ddMMyyyy}{DateTime.Now:ddMMyyyy}{serial.ToString().PadLeft(3, '0')}{new String(' ', 577)}";
                var footer = $"Q046{reportingPeriod:ddMMyyyy}{DateTime.Now:ddMMyyyy}{tlines.Length.ToString().PadLeft(7, '0')}{new String(' ',573)}";

                string directoryName = WebConfig.CibDocs + @"\Temp";
                if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

                string fileName = directoryName + @"\046CNF.txt";
                if (File.Exists(fileName))
                    File.Delete(fileName);

                List<string> lLines = new List<string>(tlines);
                lLines.Insert(0, header);
                lLines.Add(footer);

                File.WriteAllLines(fileName, lLines);

                GenerateZipArchive(FileType.Contract, reportingPeriod);
            }
        }

        public void GenerateSecurityFile()
        {
            int serial = GetSecuritySerialNumber();

            string[] lines = fileDal.GetSecurityFileData();

            var newVal = "00000000" + (lines.Length - 2);
            newVal = newVal.Substring(newVal.Length - 8);
            lines[lines.Length - 1] = lines[lines.Length - 1].Replace("XXXXXXXX", newVal);
            lines[0] = lines[0].Replace("RRRRRRRR", newVal);

            newVal = "000" + serial;
            newVal = newVal.Substring(newVal.Length - 3);
            lines[0] = lines[0].Replace("XXX", newVal);

            string directoryName = WebConfig.CibDocs + @"\Temp\security";
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            string fileName = directoryName + @"\046SECURITYINFO.txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static int GetSerialNumber()
        {
            string directoryName = string.Format(@"{0}\{1:yyyyMMdd}", WebConfig.CibDocs, AppSession.Parameters.ReportingPeriod);
            int serial = 1;
            if (Directory.Exists(directoryName))
            {
                string[] files = Directory.GetFiles(directoryName, DateTime.Now.ToString("yyyyMMdd_") + "*.zip");
                serial = files.Length + 1;
            }
            return serial;
        }

        private static int GetSecuritySerialNumber()
        {
            string directoryName = string.Format(@"{0}\security\{1:yyyyMMdd}", WebConfig.CibDocs, AppSession.Parameters.ReportingPeriod);
            int serial = 1;
            if (Directory.Exists(directoryName))
            {
                string[] files = Directory.GetFiles(directoryName, DateTime.Now.ToString("yyyyMMdd_") + "*.zip");
                serial = files.Length + 1;
            }
            return serial;
        }

        public enum FileType
        {
            Contract,
            Subject,
        }
        public void GenerateZipArchive(FileType type, DateTime reportingPeriod)
        {
            string directoryName = string.Format(@"{0}\{1:yyyyMMdd}", WebConfig.CibDocs, reportingPeriod);
            int serial = 1;
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);
            else
            {
                string[] files = Directory.GetFiles(directoryName, DateTime.Now.ToString("yyyyMMdd_") + "*.zip");
                serial = files.Length + 1;
            }
            string subFile = WebConfig.CibDocs + @"\Temp\046SJF.txt";
            string conFile = WebConfig.CibDocs + @"\Temp\046CNF.txt";

            ZipManager.CreateArchive(directoryName + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + serial + ".zip", type == FileType.Subject ? subFile : conFile);
        }

        public void GenerateSecurityZipArchive()
        {
            string directoryName = string.Format(@"{0}\security\{1:yyyyMMdd}", WebConfig.CibDocs, AppSession.Parameters.ReportingPeriod);
            int serial = 1;
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);
            else
            {
                string[] files = Directory.GetFiles(directoryName, DateTime.Now.ToString("yyyyMMdd_") + "*.zip");
                serial = files.Length + 1;
            }
            string subFile = WebConfig.CibDocs + @"\Temp\security\046SUBJECTINFO.txt";
            string secFile = WebConfig.CibDocs + @"\Temp\security\046SECURITYINFO.txt";
            if (File.Exists(subFile) && File.Exists(secFile))
            {
                ZipManager.CreateArchive(directoryName + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + serial + ".zip", subFile, secFile);
            }
        }

        public void GenerateGuarantorFile(string[] contracts, DateTime reportingPeriod)
        {
            List<DAO.ContractLink> links = fileDal.GetLinksForContracts(contracts);
            int serial = GetSerialNumber();

            var serialStr = "000" + serial;
            serialStr = serialStr.Substring(serialStr.Length - 3);

            var header = $"H046{reportingPeriod:ddMMyyyy}{DateTime.Now:ddMMyyyy}{serial.ToString().PadLeft(3, '0')}{new String(' ', 577)}";

            string[] lines = new string[links.Count + 2];
            lines[0] = header;
            for(int i = 0; i < links.Count; i++)
            {
                var link = links[i];
                lines[i + 1] = $"{link.RecordType}{link.FICode}{link.BranchCode}{link.TypeOfLink}{link.FIPrimaryCode}{link.FISecondaryCode}{link.FIContractCode,16}{new String(' ', 543)}";
            }
            var footer = $"Q046{reportingPeriod:ddMMyyyy}{DateTime.Now:ddMMyyyy}{links.Count.ToString().PadLeft(7, '0')}{new String(' ',573)}";
            lines[links.Count + 1] = footer;

            string directoryName = WebConfig.CibDocs + @"\Temp";
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            string fileName = directoryName + @"\046CNF.txt";
            if (File.Exists(fileName))
                File.Delete(fileName);

            File.WriteAllLines(fileName, lines);

            GenerateZipArchive(FileType.Contract, reportingPeriod);

            fileDal.UpdateLinksAsReportForContracts(contracts);
        }
    }
}