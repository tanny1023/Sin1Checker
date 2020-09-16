/*
Class 功能說明:生成GMSH腳本檔
*/
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Sin1Checker
{
    public class cGeoExporter
    {
        #region Property
        //--內文
        private StringBuilder FileText = new StringBuilder();
        //--檔案路徑
        private string FilePath;
        //--檔案名稱
        private string FileName;
        #endregion

        #region Constructor
        public cGeoExporter(string filePath, string fileName, List<cPoint> pointList, List<cLine> lineList)
        {
            FilePath = filePath;
            FileName = fileName;
            //--將文檔內容輸出到geo檔
            Output_geo(pointList, lineList);
        }
        #endregion

        #region Method:
        //--將文檔內容輸出到geo檔
        public void Output_geo(List<cPoint> pointList, List<cLine> lineList)
        {
            //--路徑\檔案名稱.geo
            string fp = FilePath + "\\" + FileName + ".geo";
            FileStream file_path = new FileStream(fp, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(file_path);
            //--建立點
            WritePoints(pointList);
            // --建立線
            WriteLines(lineList);
            //--建立顯示條件
            WriteParameter();
            //--將資料寫入檔案中 
            streamWriter.WriteLine(FileText);
            MessageBox.Show($"檔案完成\n{fp}");
            //--關閉檔案
            streamWriter.Close();
            file_path.Close();
        }
        //--建立點
        public void WritePoints(List<cPoint> pointList)
        {
            foreach (cPoint point in pointList)
                FileText.AppendLine(point.PrintPt());
        }
        //--建立線
        public void WriteLines(List<cLine> lineList)
        {
            foreach (cLine line in lineList)
                FileText.AppendLine(line.PrintLine());
        }
        //--建立顯示條件
        public void WriteParameter()
        {
            FileText.AppendLine("Geometry.PointNumbers = 0;");
            FileText.AppendLine("Geometry.LineNumbers = 1;");
        }
        #endregion
    }
}
