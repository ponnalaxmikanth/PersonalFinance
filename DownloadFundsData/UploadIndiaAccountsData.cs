using BusinessAccess.Accounts;
using BusinessEntities.Entities.Accounts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;

namespace DownloadFundsData
{
    public class UploadIndiaAccountsData : BaseClass
    {
        List<string> ignoreSheets = new List<string>();
        readonly DownloadExcelAccountInfo _downloadExcelAccountInfo;
        List<AccountMappingDetails> _accountMappingDetails;
        readonly string _application = "UploadIndiaAccountsData";
        readonly string _component = "Program";
        Boolean exceptionOccurred = false;

        public UploadIndiaAccountsData()
        {
            _downloadExcelAccountInfo = new DownloadExcelAccountInfo();
        }

        public bool UploadData(string file, DateTime minDate)
        {
            _accountMappingDetails = _downloadExcelAccountInfo.GetAccountMappingDetails("IndiaAccounts");
            ParseExcel(file, minDate);
            return true;
        }

        private Boolean ParseExcel(string file, DateTime minDate)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(file, false))
                {
                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                    foreach (Sheet sheet in sheets)
                    {
                        List<Transactions> transactions = new List<Transactions>();
                        string sheetName = sheet.Name;

                        var res = from ac in _accountMappingDetails
                                  where ac.ExcelMapping == sheetName
                                  select ac;

                        if (res != null && res.Count() <= 0)//if (ignoreSheets.Contains(sheetName.ToUpper()))
                        {
                            LogMessage("Ignoring Sheet: " + sheetName);
                            continue;
                        }

                        LogMessage("Processing Sheet: " + sheetName);

                        string relationshipId = sheet.Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();


                        for (int i = 0; i < rows.Count(); i++)
                        {
                            if (i == 0) continue;

                            Transactions _transaction = ParseTransaction(spreadSheetDocument, sheetName, rows.ElementAt(i), i);
                            if (_transaction != null)
                                transactions.Add(_transaction);
                        }
                        if (transactions.Count() > 0)
                        {
                            transactions = transactions
                                            .Where(t => t != null && t.PostedDate < DateTime.Now.AddDays(30) && t.PostedDate >= minDate)
                                            .OrderBy(t => t.rowNumber)
                                            .ToList();

                            LogMessage("Total Records processed for sheet " + sheetName + " : " + rows.Count() + " transactions: " + transactions.Count());

                            if (transactions.Count() > 0)
                            {
                                string xml = GetTransactionsXML(transactions, res.ElementAt(0).AccountId);
                                _downloadExcelAccountInfo.UpdateTransactions("IndiaAccounts", xml, res.ElementAt(0).AccountId, minDate);
                            }
                            transactions.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception ParseExcel Exception: " + ex.Message);
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
                exceptionOccurred = true;
            }
            return exceptionOccurred;
        }

        private Transactions ParseTransaction(SpreadsheetDocument spreadSheetDocument, string sheetName, Row row, int rowNumber)
        {
            Transactions _transaction = null;
            int cellCount = 0;
            string firstCellValue = "";
            try
            {
                if (rowNumber == 0)
                {
                }
                else
                {
                    var cellValues = from cell in row.Descendants<Cell>()
                                     select cell;
                    cellCount = cellValues.Count();

                    firstCellValue = GetCellValue(spreadSheetDocument, cellValues, "A", rowNumber + 1);
                    DateTime dtPostedDate = DateTime.Now.AddYears(1);
                    if (!string.IsNullOrWhiteSpace(firstCellValue) && ParseDateValue(firstCellValue) <= DateTime.Now.AddMonths(1))
                    {
                        try
                        {
                            _transaction = new Transactions()
                            {
                                rowNumber = rowNumber + 1,
                                PostedDate = ParseDateValue(firstCellValue),
                                Description = GetCellValue(spreadSheetDocument, cellValues, "B", rowNumber + 1),
                                Debit = Conversions.ToDecimal(GetCellValue(spreadSheetDocument, cellValues, "C", rowNumber + 1), 0),
                                Credit = Conversions.ToDecimal(GetCellValue(spreadSheetDocument, cellValues, "D", rowNumber + 1), 0),
                                Total = Conversions.ToDecimal(GetCellValue(spreadSheetDocument, cellValues, "E", rowNumber + 1), 0),
                                Comments = cellCount > 4 ? GetCellValue(spreadSheetDocument, cellValues, "F", rowNumber + 1) : string.Empty,
                                TransactedBy = "Kanth",
                                Group = "Home",
                                SubGroup = "Home"
                                //TransactedBy = cellCount > 4 ? GetCellValue(spreadSheetDocument, cellValues, "F", rowNumber + 1) : string.Empty,
                                //Group = cellCount > 5 ? GetCellValue(spreadSheetDocument, cellValues, "G", rowNumber + 1) : string.Empty,
                                //SubGroup = cellCount > 6 ? GetCellValue(spreadSheetDocument, cellValues, "H", rowNumber + 1) : string.Empty,
                            };
                            //_transaction.TransactDate = _transaction.TransactDate > DateTime.Now.AddDays(30) ? _transaction.PostedDate : _transaction.TransactDate;
                        }
                        catch (Exception ex)
                        {
                            LogMessage("Exception while processing sheet:" + sheetName + " row number: " + rowNumber + " Cell count: " + cellCount + " first cell: " + firstCellValue + " Exception: " + ex.Message);
                            LogMessage(ex.StackTrace);
                            exceptionOccurred = true;
                        }
                    }
                    else
                    {
                        //DisplayMessage("sheetName: " + sheetName + " rowNumber: " + rowNumber + " cellCount: " + cellCount + " firstCellValue: " + firstCellValue);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception while processing sheet:" + sheetName + " row number: " + rowNumber + " Cell count: " + cellCount + " first cell: " + firstCellValue + " Exception: " + ex.Message);
                LogMessage(ex.StackTrace);
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
                exceptionOccurred = true;
            }
            return _transaction;
        }

        private string GetCellValue(SpreadsheetDocument document, IEnumerable<Cell> cellValues, string v1, int v2)
        {
            string reusltStr = string.Empty;
            try
            {
                Cell cell = cellValues.Where(c => c.CellReference.Value == (v1 + v2.ToString())).FirstOrDefault();
                SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
                string value = (cell == null || cell.CellValue == null) ? string.Empty : cell.CellValue.InnerXml;

                if (cell != null && cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                LogMessage("GetCellValue cell reference: " + v1 + v2 + " Exception: " + ex.Message);
                exceptionOccurred = true;
            }
            return reusltStr;
        }

        private DateTime ParseDateValue(string val)
        {
            DateTime retVal = DateTime.Now.AddYears(1);
            try
            {
                if (!string.IsNullOrWhiteSpace(val))
                {
                    retVal = DateTime.FromOADate(double.Parse(val));
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception ParseDateValue: " + ex.Message);
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
                exceptionOccurred = true;
            }
            return retVal;
        }

        private string GetTransactionsXML(List<Transactions> transactions, int accountId)
        {
            string result = string.Empty;
            try
            {
                result = new XElement("root",
                    (from t in transactions
                     select
                     new XElement("row",
                           new XElement("acctNo", accountId),
                           new XElement("rowNum", t.rowNumber),
                           new XElement("posteddate", t.PostedDate.ToString("MM/dd/yyyy")),
                           new XElement("description", t.Description),
                           new XElement("debit", t.Debit),
                           new XElement("credit", t.Credit),
                           new XElement("total", t.Total),
                           new XElement("transactby", t.TransactedBy),
                           new XElement("group", t.Group),
                           new XElement("subgroup", t.SubGroup),
                           new XElement("comments", t.Comments)
                           )
                            )
                         ).ToString();
            }
            catch (Exception ex)
            {
                LogMessage("Exception while GetTransactionsXML Exception: " + ex.Message);
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
                exceptionOccurred = true;
            }
            return result;
        }
    }
}
