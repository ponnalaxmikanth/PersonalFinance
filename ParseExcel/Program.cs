using BusinessAccess.Accounts;
using BusinessEntities.Entities.Accounts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParseExcel
{
    class Program
    {
        static List<string> ignoreSheets = new List<string>();
        static DownloadExcelAccountInfo _downloadExcelAccountInfo = new DownloadExcelAccountInfo();
        static List<AccountMappingDetails> _accountMappingDetails = null;

        static void Main(string[] args)
        {
            int DaysOld = 30;
            if (args.Length < 0)
            {

                DisplayMessage("Please provide file path in arguments....");
                DisplayMessage("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            if(args.Length > 1)
            {
                int.TryParse(args[1], out DaysOld);
            }
            _accountMappingDetails = _downloadExcelAccountInfo.GetAccountMappingDetails();
            //ParseConfigurationFile();
            ParseExcel(args[0], DaysOld);
        }

        private static void ParseConfigurationFile()
        {
            try
            {
                
                XElement configurationsFile = XElement.Load(@"C:\Kanth\Projects\dotNet\PersonalFinance\ParseExcel\ExcelConfigurations.xml");

                var ignoreList = configurationsFile.Elements().Where(e => e.Name == "ignoredSheets").ToList();
                if(ignoreList != null && ignoreList.Count > 0)
                {
                    ignoreSheets = ignoreList.Elements().Select(e => e.Attribute("name").Value.ToUpper()).ToList();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private static void ParseExcel(string file, int _daysOld)
        {
            DataTable dt = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(file, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                foreach(Sheet sheet in sheets)
                {
                    List<Transactions> transactions = new List<Transactions>();
                    string sheetName = sheet.Name;

                    var res = from ac in _accountMappingDetails
                              where ac.ExcelMapping == sheetName
                              select ac;
                    
                    if(res != null && res.Count() <= 0)//if (ignoreSheets.Contains(sheetName.ToUpper()))
                    {
                        DisplayMessage("Ignoring Sheet: " + sheetName);
                        continue;
                    }

                    DisplayMessage("Processing Sheet: " + sheetName);

                    string relationshipId = sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        //DisplayMessage("Processing Sheet: " + sheetName + " - Row Number: " + i);
                        if (i == 0) continue;

                        Transactions _transaction = ParseTransaction(spreadSheetDocument, sheetName, rows.ElementAt(i), i);
                        if(_transaction != null)
                            transactions.Add(_transaction);
                    }

                    DateTime _minDate = new DateTime(1900, 1, 1);
                    if (_daysOld > 0)
                         _minDate = DateTime.Now.AddDays(-_daysOld).Date;
                    if(transactions.Count() > 0)
                    {
                        transactions = transactions
                                        //.Where(t => t != null && t.Date < DateTime.Now.AddYears(1) && t.Date>= _minDate)
                                        .Where(t => t != null && t.PostedDate < DateTime.Now.AddDays(30) && t.PostedDate >= _minDate)
                                        .Where(t => t.Debit != 0 || t.Credit != 0)
                                        //.OrderBy(t => t.Date)
                                        .ToList();

                        DisplayMessage("Total Records processed for sheet " + sheetName + " : " + rows.Count() + " transactions: " + transactions.Count() + " min date: " + _minDate.ToString("MM/dd/yyyy"));

                        //res.ElementAt(0).AccountId
                        if (transactions.Count() > 0)
                        {
                            string xml = GetTransactionsXML(transactions, res.ElementAt(0).AccountId);
                            _downloadExcelAccountInfo.UpdateTransactions(xml, res.ElementAt(0).AccountId, _minDate);
                        }
                        transactions.Clear();
                    }
                }
            }
        }

        private static string GetTransactionsXML(List<Transactions> transactions, int accountId)
        {
            string result = string.Empty;
            try
            {
                //new XElement("root")
                result = new XElement("root", 
                    (from t in transactions
                          select 
                          new XElement("row",
                                new XElement("acctNo", accountId),
                                new XElement("transactdate", t.TransactDate.ToString("MM/dd/yyyy")),
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
            catch(Exception ex)
            {
                DisplayMessage("Exception while GetTransactionsXML Exception: " + ex.Message);
            }
            return result;
        }

        private static Transactions ParseTransaction(SpreadsheetDocument spreadSheetDocument, string sheetName, Row row, int rowNumber)
        {
            Transactions _transaction = null;
            try
            {
                if (rowNumber == 0)
                {
                    //foreach (Cell cell in row)
                    //{
                    //    if (cell != null && cell.CellValue != null && cell.CellValue.InnerXml != null)
                    //        dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                    //}
                }
                else
                {
                    var cellValues = from cell in row.Descendants<Cell>()
                                     select cell;
                    int cellCount = cellValues.Count();

                    if (cellCount > 5)
                    {
                        _transaction = new Transactions()
                        {
                            TransactDate = ParseDateValue(GetCellValue(spreadSheetDocument, cellValues.ElementAt(0))),
                            PostedDate = ParseDateValue(GetCellValue(spreadSheetDocument, cellValues.ElementAt(1))),
                            Description = GetCellValue(spreadSheetDocument, cellValues.ElementAt(2)),
                            Debit = ParseDecimalValue(GetCellValue(spreadSheetDocument, cellValues.ElementAt(3))),
                            Credit = ParseDecimalValue(GetCellValue(spreadSheetDocument, cellValues.ElementAt(4))),
                            Total = ParseDecimalValue(GetCellValue(spreadSheetDocument, cellValues.ElementAt(5))),
                            TransactedBy = cellCount > 6 ? GetCellValue(spreadSheetDocument, cellValues.ElementAt(6)) : string.Empty,
                            Group = cellCount > 7 ? GetCellValue(spreadSheetDocument, cellValues.ElementAt(7)) : string.Empty,
                            SubGroup = cellCount > 8 ? GetCellValue(spreadSheetDocument, cellValues.ElementAt(8)) : string.Empty,
                            Comments = cellCount > 9 ? GetCellValue(spreadSheetDocument, cellValues.ElementAt(9)) : string.Empty
                        };
                        _transaction.TransactDate = _transaction.TransactDate > DateTime.Now.AddDays(30) ? _transaction.PostedDate : _transaction.TransactDate;

                    }
                }
            }
            catch(Exception ex)
            {
                DisplayMessage("Exception while processing sheet:" + sheetName + " row number: " + rowNumber + " Exception: " + ex.Message);
            }
            return _transaction;
        }

        public static DateTime ParseDateValue(string val)
        {
            DateTime retVal = DateTime.Now.AddYears(1);
            try
            {
                if(!string.IsNullOrWhiteSpace(val))
                {
                    retVal = DateTime.FromOADate(double.Parse(val));
                    //DateTime.TryParse(val, out retVal);
                }
            }
            catch(Exception ex)
            {
                DisplayMessage("Exception ParseDateValue: " + ex.Message);
            }
            return retVal;
        }

        public static Decimal? ParseDecimalValue(string val)
        {
            Decimal retVal = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(val))
                {
                    if(!decimal.TryParse(val, out retVal))
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Exception ParseDecimalValue: " + ex.Message);
            }
            return retVal;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue == null ? string.Empty : cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        public static void DisplayMessage(string msg)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "] " + msg);
        }

    }
}
