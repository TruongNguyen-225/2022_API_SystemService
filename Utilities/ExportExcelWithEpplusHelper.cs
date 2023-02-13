using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using SystemServiceAPICore3.Utilities.Constants;

namespace SystemServiceAPICore3.Utilities
{
    public static class ExportExcelWithEpplusHelper
    {
        private const int FIRST_SHEET = 0;
        private const int MAX_BILL_PER_SHEET = 8;

        #region --- Binding data to excel ---

        public static byte[] LoadFileTemplate(string pathTemplate, System.Data.DataTable dataTable, ExcelParamDefault paramDefault, bool isMultiSheet = false)
        {
            if (File.Exists(pathTemplate))
            {
                FileInfo file = new FileInfo(pathTemplate);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    if (isMultiSheet == false)
                    {
                        dataTable.BindingDataFirtSheet(excelPackage, paramDefault);
                    }
                    else
                    {
                        dataTable.BindingDataMultiSheet(excelPackage, paramDefault);
                    }

                    //Save your file
                    return excelPackage.GetAsByteArray();
                }
            }

            return null;
        }

        /// <summary>
        /// Tính tiền thanh toán.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="excelPackage"></param>
        /// <param name="paramDefault"></param>
        public static void BindingDataFirtSheet(this System.Data.DataTable dataTable, ExcelPackage excelPackage, ExcelParamDefault paramDefault)
        {
            //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
            ExcelWorksheet firstWorksheet = excelPackage.Workbook.Worksheets[FIRST_SHEET];

            //Get a WorkSheet by name. If the worksheet doesn't exist, throw an exeption
            //ExcelWorksheet namedWorksheet = excelPackage.Workbook.Worksheets["SomeWorksheet"];

            //If you don't know if a worksheet exists, you could use LINQ,
            //So it doesn't throw an exception, but return null in case it doesn't find it
            //ExcelWorksheet anotherWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "SomeWorksheet");

            //Get the content from cells A1 and B1 as string, in two different notations
            firstWorksheet.Cells[4, 2].Value = paramDefault.fileNameParam.FileName;
            firstWorksheet.Cells[5, 2].Value = "Thời gian tạo báo cáo : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                int index = 0;
                var valueCell = String.Empty;
                string columnName = String.Empty;
                int startRow = paramDefault.cellParam.StartRow;
                int startColumn = paramDefault.cellParam.StartColumn;
                int maxColumn = paramDefault.cellParam.MaxColumn;
                string valueString = String.Empty;
                int sumMoney = 0;

                foreach (DataRow row in dataTable.Rows)
                {
                    index = dataTable.Rows.IndexOf(row) + 1;
                    var nextRow = startRow + index;

                    //copy tại rowStart => paste tại rowStart + 1
                    firstWorksheet.Cells[startRow, startColumn, startRow, maxColumn].Copy(firstWorksheet.Cells[nextRow, startColumn, nextRow, maxColumn]);

                    //fill row 4 with striped orange background
                    //firstWorksheet.Row(nextRow).Style.Fill.PatternType = ExcelFillStyle.DarkHorizontal;
                    //firstWorksheet.Row(nextRow).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));

                    for (int i = 1; i <= maxColumn; i++)
                    {
                        var cell = firstWorksheet.Cells[nextRow, i].Value;

                        if (cell != null)
                        {
                            valueCell = cell.ToString();

                            if (!String.IsNullOrEmpty(valueCell) && valueCell.Contains(ExportExcelConstants.TEXT_MARKER))
                            {
                                columnName = valueCell.Replace(ExportExcelConstants.TEXT_MARKER, String.Empty);
                                Type type = row[columnName].GetType();

                                if (type.Name != "DBNull")
                                {
                                    valueString = DataAccess.CorrectValue(row[columnName], type).ToString();
                                }

                                int output;
                                bool success = int.TryParse(valueString, out output);

                                if (success)
                                {
                                    firstWorksheet.Cells[nextRow, i].Value = output;

                                    if (columnName == "Money" || columnName == "Postage" || columnName == "Total")
                                    {
                                        if (columnName == "Total")
                                        {
                                            sumMoney += output;
                                        }

                                        firstWorksheet.Cells[nextRow, i].Style.Numberformat.Format = "#,##0";
                                    }
                                }
                                else
                                {
                                    firstWorksheet.Cells[nextRow, i].Value = valueString;
                                }
                            }
                        }
                    }
                }

                firstWorksheet.Rows.Height = 25;
                firstWorksheet.DeleteRow(startRow, 1);

                //Add thêm dòng tính sum
                int positionSumX = startRow + dataTable.Rows.Count + 1;
                int postionSumY = startColumn;

                firstWorksheet.Cells[positionSumX, postionSumY + 7].Value = "TỔNG TIỀN DỊCH VỤ TÍNH ĐƯỢC LÀ : ";
                firstWorksheet.Cells[positionSumX, postionSumY + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                firstWorksheet.Cells[positionSumX, postionSumY + 7].Style.Font.Bold = true;

                firstWorksheet.Cells[positionSumX, postionSumY + 8].Value = sumMoney;
                firstWorksheet.Cells[positionSumX, postionSumY + 8].Style.Numberformat.Format = "#,##0";
                firstWorksheet.Cells[positionSumX, postionSumY + 8].Style.Font.Bold = true;
            }
        }

        /// <summary>
        /// In hoá đơn tiền điện
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="excelPackage"></param>
        /// <param name="paramDefault"></param>
        public static void BindingDataMultiSheet(this System.Data.DataTable dataTable, ExcelPackage excelPackage, ExcelParamDefault paramDefault)
        {
            //1. Calculator sheet
            int count = dataTable.Rows.Count;
            int maxSheet = (int)Math.Round(count / (float)MAX_BILL_PER_SHEET);

            if (maxSheet > 0)
            {
                //2. Generate template for sheet
                ExcelWorksheet firstWorksheet = excelPackage.Workbook.Worksheets[FIRST_SHEET];
                string firtSheetName = firstWorksheet.Name;
                ExcelWorkbook workbook = excelPackage.Workbook;

                for (int i = 1; i < maxSheet; i++)
                {
                    ExcelWorksheet worksheet = workbook.Worksheets.Copy(firtSheetName, String.Format("{0} {1}", firstWorksheet, i.ToString()));
                }

                //3. Fill data sheet by sheet

                int sheets = excelPackage.Workbook.Worksheets.Count;

                for (int i = 0; i < sheets; i++)
                {
                    dataTable.BindingDataBillElectricBySheet(i, excelPackage, paramDefault);
                }
            }
        }

        /// <summary>
        /// Binding data into sheet
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sheetActive"></param>
        /// <param name="excelPackage"></param>
        /// <param name="paramDefault"></param>
        public static void BindingDataBillElectricBySheet(this DataTable data, int sheetActive, ExcelPackage excelPackage, ExcelParamDefault paramDefault)
        {
            int startRow = paramDefault.cellParam.StartRow;
            int nextStepRow = 20;
            ExcelWorksheet activeSheet = excelPackage.Workbook.Worksheets[sheetActive];

            for (int j = startRow; j < 35; j++)
            {
                int index = 0;

                if (j < startRow + nextStepRow)
                {
                    data.BindingDataBillElectric(activeSheet, sheetActive, paramDefault, true, j, index);
                }
                else  //hàng 2
                {
                    data.BindingDataBillElectric(activeSheet, sheetActive, paramDefault, false, j, index);
                }
            }
        }

        /// <summary>
        /// Binding data detail
        /// </summary>
        /// <param name="data"></param>
        /// <param name="activeSheet"></param>
        /// <param name="sheetActive"></param>
        /// <param name="paramDefault"></param>
        /// <param name="isTopRow"></param>
        /// <param name="rowExcelSelected"></param>
        /// <param name="index"></param>
        public static void BindingDataBillElectric(this DataTable data, ExcelWorksheet activeSheet, int sheetActive, ExcelParamDefault paramDefault, bool isTopRow, int rowExcelSelected, int index)
        {
            object valueCell = null;
            string columnName = String.Empty;
            int startColumn = paramDefault.cellParam.StartColumn;
            int maxColumn = paramDefault.cellParam.MaxColumn;
            int nextStepColumn = 4;
            var rowSelected = 0;
            int maxRow = data.Rows.Count;
            string valueString = String.Empty;

            for (int i = startColumn; i < maxColumn; i++)
            {
                valueCell = activeSheet.Cells[rowExcelSelected, i].Value;

                if (valueCell != null && valueCell.ToString().Contains(ExportExcelConstants.TEXT_MARKER))
                {
                    columnName = valueCell.ToString().Replace(ExportExcelConstants.TEXT_MARKER, String.Empty);
                    rowSelected = isTopRow ? (nextStepColumn * (sheetActive + sheetActive)) + index : (nextStepColumn * ((sheetActive + sheetActive) + 1)) + index;

                    if (rowSelected < maxRow)
                    {
                        DataRow row = data.Rows[rowSelected];
                        Type type = row[columnName].GetType();

                        if (type.Name != "DBNull")
                        {
                            valueString = DataAccess.CorrectValue(row[columnName], type).ToString();

                            int output;
                            bool success = int.TryParse(valueString, out output);

                            if (success)
                            {
                                activeSheet.Cells[rowExcelSelected, i].Value = output;
                                if (columnName == "Money" || columnName == "Postage" || columnName == "Total")
                                {
                                    activeSheet.Cells[rowExcelSelected, i].Style.Numberformat.Format = "#,##0";
                                    activeSheet.Cells[rowExcelSelected, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                }
                            }
                            else
                            {
                                activeSheet.Cells[rowExcelSelected, i].Value = valueString;
                            }
                        }

                        index++;
                    }
                }
            }
        }

        #endregion

        #region --- Binding data with Generic type ---

        public static byte[] LoadFileTemplate<T>(string pathTemplate, List<T> listData, ExcelParamDefault paramDefault) where T : class
        {
            if (File.Exists(pathTemplate))
            {
                FileInfo file = new FileInfo(pathTemplate);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    int countSheet = excelPackage.Workbook.Worksheets.Count;
                    if (countSheet == 1)
                    {
                        listData.BindingDataFirtSheet(excelPackage, paramDefault);
                    }
                    else
                    {

                    }

                    //Save your file
                    return excelPackage.GetAsByteArray();
                }
            }
            return null;
        }

        public static void BindingDataFirtSheet<T>(this List<T> data, ExcelPackage excelPackage, ExcelParamDefault paramDefault) where T : class
        {
            const int FIRST_SHEET = 0;
            //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
            ExcelWorksheet firstWorksheet = excelPackage.Workbook.Worksheets[FIRST_SHEET];

            //Get a WorkSheet by name. If the worksheet doesn't exist, throw an exeption
            //ExcelWorksheet namedWorksheet = excelPackage.Workbook.Worksheets["SomeWorksheet"];

            //If you don't know if a worksheet exists, you could use LINQ,
            //So it doesn't throw an exception, but return null in case it doesn't find it
            //ExcelWorksheet anotherWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "SomeWorksheet");

            //Get the content from cells A1 and B1 as string, in two different notations
            string retailName = paramDefault.fileNameParam.FileName;
            string fromDate = paramDefault.fileNameParam.FromDate;
            string toDate = paramDefault.fileNameParam.ToDate;
            string timeExport = paramDefault.fileNameParam.TimeExport.ToString("dd-MM-yyyy hh:mm");

            firstWorksheet.Cells[3, 1].Value = String.Format(ExportExcelConstants.FILE_NAME_TRANSACTION_FILE_NAME, retailName, fromDate, toDate);
            firstWorksheet.Cells[4, 1].Value = timeExport;

            if (data != null && data.Count > 0)
            {
                int index = 0;
                var valueCell = String.Empty;
                int sumMoney = 0;
                int sumTotal = 0;
                string columnName = String.Empty;
                int startRow = paramDefault.cellParam.StartRow;
                int startColumn = paramDefault.cellParam.StartColumn;
                int maxColumn = paramDefault.cellParam.MaxColumn;

                foreach (var item in data)
                {
                    index++;
                    var nextRow = startRow + index;

                    //copy tại rowStart => paste tại rowStart + 1
                    firstWorksheet.Cells[startRow, startColumn, startRow, maxColumn].Copy(firstWorksheet.Cells[nextRow, startColumn, nextRow, maxColumn]);

                    //fill row 4 with striped orange background
                    firstWorksheet.Row(nextRow).Style.Fill.PatternType = ExcelFillStyle.DarkHorizontal;
                    firstWorksheet.Row(nextRow).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));

                    for (int i = 1; i <= maxColumn; i++)
                    {
                        valueCell = firstWorksheet.Cells[nextRow, i].Value.ToString();

                        //if (!String.IsNullOrEmpty(valueCell) && valueCell.Contains(ExportExcelConstants.TEXT_MARKER))
                        //{
                        //    columnName = valueCell.Replace(ExportExcelConstants.TEXT_MARKER, String.Empty);
                        //    object type = row[columnName].GetType();
                        //    var value = DataAccess.CorrectValue(row[columnName], type);

                        //    if (type is int)
                        //    {
                        //        firstWorksheet.Cells[nextRow, i].Value = value;
                        //        firstWorksheet.Cells[nextRow, i].Style.Numberformat.Format = "#";

                        //    }
                        //    else
                        //    {
                        //        firstWorksheet.Cells[nextRow, i].Value = value;
                        //    }

                        //    //if (columnName == "money")
                        //    //{
                        //    //    sumMoney += Convert.ToInt32(value);
                        //    //}
                        //    //else if (columnName == "total")
                        //    //{
                        //    //    sumTotal += Convert.ToInt32(value);
                        //    //}
                        //}
                    }
                }

                firstWorksheet.DeleteRow(startRow, 1);

                //Add thêm dòng tính sum
                firstWorksheet.Cells[startRow, startColumn].Copy(firstWorksheet.Cells[startRow + 1, startColumn]);
            }

            //return null;
        }

        #endregion
    }
}

