using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using SystemServiceAPICore3.Helpers.Constant;

namespace SystemServiceAPICore3.Utilities
{
    public class ExcelUtility
    {
        public static void ReadExcelFile()
        {
            try
            {
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open("testdata.xlsx", false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    StringBuilder excelResult = new StringBuilder();

                    //using for each loop to get the sheet from the sheetcollection  
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        excelResult.AppendLine("Excel Sheet Name : " + thesheet.Name);
                        excelResult.AppendLine("----------------------------------------------- ");
                        //statement to get the worksheet object by using the sheet id  
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                        SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();
                        foreach (Row thecurrentrow in thesheetdata)
                        {
                            foreach (Cell thecurrentcell in thecurrentrow)
                            {
                                //statement to take the integer value  
                                string currentcellvalue = string.Empty;
                                if (thecurrentcell.DataType != null)
                                {
                                    if (thecurrentcell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                //code to take the string value  
                                                excelResult.Append(item.Text.Text + " ");
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    excelResult.Append(Convert.ToInt16(thecurrentcell.InnerText) + " ");
                                }
                            }
                            excelResult.AppendLine();
                        }
                        excelResult.Append("");
                        Console.WriteLine(excelResult.ToString());
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static void CreateAndWriteBillExcel(string fileName, DataTable table)
        {

            // Lets converts our object data to Datatable for a simplified logic.
            // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
            //DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(persons), (typeof(DataTable)))

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                string currentTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                string fullName = "Khách Hàng : " + table.Rows[0]["FullName"].ToString();
                string code = "Mã số hợp đồng : " + table.Rows[0]["Code"].ToString();
                string bank = "Ngân hàng : " + table.Rows[0]["Bank"].ToString();
                string money = "Số tiền : " + table.Rows[0]["Money"].ToString();
                string postage = "Phí dịch vụ : " + table.Rows[0]["Postage"].ToString();
                string total = "Tổng cộng : " + table.Rows[0]["Total"].ToString();

                List<string> listRow = new List<string>()
                {
                    Constant.SYSTEM_SERVICE,
                    Constant.SERVICE_ELECTRIC_NAME,
                    Constant.ADDRESS_BRANCH_1,
                    Constant.HOTLINE_BRANCH1,
                    currentTime,
                    Constant.UNDER_LINE,
                    fullName,
                    code,
                    bank,
                    money,
                    postage,
                    total,
                    Constant.UNDER_LINE,
                    Constant.THANK_YOU
                };

                Row newRow = new Row();


                foreach (string item in listRow)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(item);
                    newRow.AppendChild(cell);
                }

                sheetData.AppendChild(newRow);
                workbookPart.Workbook.Save();
            }
        }
    }
}

