/*
Class 功能說明: 多線程讀取Excel（1 主線程、4 副線程）
*/
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;


//--引用EXCEL資料表格
namespace Sin1Checker
{
    class MainProgram
    {
        #region Property
        private string GmshPath = Path.Combine(@"C:\ProgramData\Autodesk\Revit\Addins\2018", "gmsh.exe");
        //--存檔路徑
        private string SaveFilePath;
        private DataTable PointDataTable = new DataTable();
        private DataTable LineDataTable = new DataTable();
        private DataTable LinePropertyTable = new DataTable();
        List<cPoint> PointList = new List<cPoint>();
        List<cLine> LineList = new List<cLine>();
        #endregion

        #region Constructor
        public MainProgram()
        {
            //--工作表名稱
            string PointSheetName = "C.結構點位";
            string LineNumberSheetName = "D.桿件編號";
            string LinePropertySheetName = "E.桿件資訊";
            //--開啟EXCEL檔案的位置

            OpenFileDialog openFileDialogFunction = FindExcel();
            if (openFileDialogFunction.ShowDialog() != DialogResult.OK)
                MessageBox.Show("檔案開啟錯誤");
            else
            {
                string filePath = openFileDialogFunction.FileName;
                string fileName = Path.GetFileName(filePath);
                SaveFilePath = filePath.Remove(filePath.Length - fileName.Length, fileName.Length);
                PointDataTable = ImportExcel(PointSheetName, 2, filePath, fileName);
                LineDataTable = ImportExcel(LineNumberSheetName, 2, filePath, fileName);
                LinePropertyTable = ImportExcel(LinePropertySheetName, 2, filePath, fileName);
            }
            //--建立點
            GetPoint();
            //--建立線
            GetLine();
            //--加入線的屬性
            SetLineProperty();
            //--建立GEO檔案
            cGeoExporter geoFile = new cGeoExporter(SaveFilePath, "2Ddata",PointList, LineList);
            //--呼叫GMSH.exe
            cProgramCaller gmshCaller = new cProgramCaller();
            gmshCaller.SetPorgram(GmshPath, SaveFilePath, "2Ddata", ".geo");
            gmshCaller.CallProgram();
        }
        #endregion

        #region Method
        //--建立點
        public void GetPoint()
        {
            //提取點的屬性信息
            for (int i = 0; i < PointDataTable.Rows.Count; i++)
            {
                int number = Convert.ToInt32(PointDataTable.Rows[i][0]);
                double x = Convert.ToDouble(PointDataTable.Rows[i][1]);
                double y = Convert.ToDouble(PointDataTable.Rows[i][2]);
                double z = Convert.ToDouble(PointDataTable.Rows[i][3]);
                PointList.Add(new cPoint(number, x, y, z));
            }
        }
        //--建立線
        public void GetLine()
        {
            //提取線的屬性信息
            for (int i = 0; i < LineDataTable.Rows.Count; i++)
            {
                int number = Convert.ToInt32(LineDataTable.Rows[i][0]);
                int startPt = Convert.ToInt32(LineDataTable.Rows[i][1]);
                int endPt = Convert.ToInt32(LineDataTable.Rows[i][2]);
                LineList.Add(new cLine(number, startPt, endPt));
            }
        }
        //--尋找相同編號的Line加入屬性
        public void FindSameLineNumber(int number, string lineProperty)
        {
            foreach (cLine line in LineList)
                line.SetLineProperty(number, lineProperty);
        }
        //--加入線的屬性
        public void SetLineProperty()
        {
            //提取線的屬性信息
            for (int i = 0; i < LinePropertyTable.Rows.Count; i++)
            {
                int number = Convert.ToInt32(LinePropertyTable.Rows[i][0]);
                string lineProperty = LinePropertyTable.Rows[i][2].ToString();
                FindSameLineNumber(number, lineProperty);
            }

        }
        //--得到資料表
        public DataTable GetTable(DataTableName dataTableName)
        {
            DataTable table = new DataTable();
            switch (dataTableName)
            {
                case DataTableName.PointTable:
                    table = PointDataTable;
                    break;
                case DataTableName.LineTable:
                    table = LineDataTable;
                    break;
                case DataTableName.LinePropertyTable:
                    table = LinePropertyTable;
                    break;
            }
            return table;
        }
        //--開啟EXCEL檔案的位置
        public OpenFileDialog FindExcel()
        {
            OpenFileDialog openFileDialogFunction = new OpenFileDialog();
            //--// 預設開啟檔案的類型
            openFileDialogFunction.Filter = "Excel files|*.xlsx";
            //--視窗標題
            openFileDialogFunction.Title = "匯入Excel資料";
            return openFileDialogFunction;
        }
        //--載入Excel檔案
        public DataTable ImportExcel(string sheetName, int headLine, string filePath, string fileName)
        {
            DataTable dataTable = new DataTable();

            FileStream fileStream = null;
            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IWorkbook workBook = null;
            //--副檔名類型
            switch (Path.GetExtension(fileName).ToUpper())
            {
                case ".XLS":
                    workBook = new HSSFWorkbook(fileStream);
                    break;
                case ".XLSX":
                    workBook = new XSSFWorkbook(fileStream);
                    break;
            }
            //--獲取指定名稱的工作表
            ISheet sheet = workBook.GetSheet(sheetName);
            //--獲得指定列的標題
            IRow headerRow = sheet.GetRow(headLine);
            //--處理標題列
            for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
            {
                string columnName = headerRow.GetCell(i).StringCellValue.Trim();
                //--判斷列名是否重複
                if (dataTable.Columns.Contains(columnName))
                    columnName += 2;
                dataTable.Columns.Add(columnName);

            }
            IRow row = null;
            DataRow dataRow = null;
            CellType ct = CellType.Blank;
            //標題列之後的資料
            for (int i = headLine + 1; i <= sheet.LastRowNum; i++)
            {
                dataRow = dataTable.NewRow();
                row = sheet.GetRow(i);
                if (row == null) continue;
                for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                {
                    ct = row.GetCell(j).CellType;
                    //如果此欄位格式為公式 則去取得CachedFormulaResultType
                    if (ct == CellType.Formula)
                        ct = row.GetCell(j).CachedFormulaResultType;
                    if (ct == CellType.Numeric)
                        dataRow[j] = row.GetCell(j).NumericCellValue;
                    else
                        dataRow[j] = row.GetCell(j).ToString().Replace("$", "");
                }
                dataTable.Rows.Add(dataRow);
            }
            fileStream.Close();
            fileStream.Dispose();
            return dataTable;
        }
        #endregion
    }
}