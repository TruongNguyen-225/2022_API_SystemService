using System;
using System.Data;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace SystemServiceAPICore3.Utilities
{
    public class EpplusHelper
    {
        public static byte[] ExportExcel(string pathTemplate, int rowStart, int colStart, int maxCol, string textMarker, DataTable dataTable)
        {
            

            if(File.Exists(pathTemplate))
            {
                FileInfo file = new FileInfo(pathTemplate);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
                    ExcelWorksheet firstWorksheet = excelPackage.Workbook.Worksheets[1];

                    //Get a WorkSheet by name. If the worksheet doesn't exist, throw an exeption
                    //ExcelWorksheet namedWorksheet = excelPackage.Workbook.Worksheets["SomeWorksheet"];

                    //If you don't know if a worksheet exists, you could use LINQ,
                    //So it doesn't throw an exception, but return null in case it doesn't find it
                    //ExcelWorksheet anotherWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "SomeWorksheet");

                    //Get the content from cells A1 and B1 as string, in two different notations
                    
                    firstWorksheet.Cells[4, 2].Value = "DANH SÁCH KHÁCH HÀNG SỬ DỤNG DỊCH VỤ TẠI " + dataTable.Rows[0]["retailName"].ToString();
                    firstWorksheet.Cells[5, 2].Value = DateTime.Now.ToString("dd-MM-yyyy hh:mm");

                    if (dataTable.Rows.Count > 0)
                    {
                        int index = 0;
                        var valueCell = String.Empty;
                        int sumMoney = 0;
                        int sumTotal = 0;
                        string columnName = String.Empty;
                        string value = String.Empty;

                        foreach(DataRow row in dataTable.Rows)
                        {
                            index = dataTable.Rows.IndexOf(row);
                            //copy tại rowStart
                            //paste tại rowStart + 1
                            firstWorksheet.Cells[rowStart, colStart].Copy(firstWorksheet.Cells[rowStart + 1, colStart]);

                            for(int i=0; i < maxCol; i++)
                            {
                                valueCell = firstWorksheet.Cells[rowStart + index, i].Value.ToString();

                                if (!String.IsNullOrEmpty(valueCell) && valueCell.Contains(textMarker))
                                {
                                    columnName = valueCell.Split(textMarker)[1];
                                    value = row[columnName].ToString();

                                    firstWorksheet.Cells[rowStart + index, i].Value = value;

                                    if(columnName == "money")
                                    {
                                        sumMoney += Convert.ToInt32(value);
                                    }
                                    else if(columnName == "total")
                                    {
                                        sumTotal += Convert.ToInt32(value);
                                    }
                                }    
                            }    
                        }
                    }    

                    //Save your file
                    return excelPackage.GetAsByteArray();
                }
            }

            return null;
        }
    }
}

