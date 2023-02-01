using System;
using DocumentFormat.OpenXml.Drawing;

namespace SystemServiceAPICore3.Utilities.Constants
{
	public static class ExportExcelConstants
	{
        public const string RETAIL_NAME_1 = "CAFÉ NHỚ";

        public const string RETAIL_NAME_2 = "CHỊ XUÂN";

        public const string FILE_NAME_TRANSACTION_FILE_NAME = "LỊCH SỬ GIAO DỊCH TẠI CHI NHÁNH {0} TỪ NGÀY {1} ĐẾN HẾT NGÀY {2}";

        public const string FILE_NAME_LIST_ELECTRIC_BILL = "";

        public const string TEXT_MARKER = "$";
    }

    public class ExcelParamDefault
    {
        public FileNameParams fileNameParam { get; set; }

        public CellParams cellParam { get; set; }
    }

    public class FileNameParams
    {
        public string FileName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public DateTime TimeExport { get; set; }
    }

    public class CellParams
    {

        public int StartRow { get; set; }

        public int StartColumn { get; set; }

        public int MaxColumn { get; set; }
    }
}

