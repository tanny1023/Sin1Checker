/*
Class 功能說明:呼叫GMSH.exe
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Sin1Checker
{
    public class cProgramCaller
    {
        #region Property:
        //--相依的Class
        private cPopupBox PopupBox = new cPopupBox();
        //--應用程式位置
        private string ProgamPath;
        //--開啟檔案位置
        private string FilePath;
        //--開啟檔案名稱
        private string FileName;
        #endregion

        #region Method:
        //--初始化
        public void SetPorgram(string progamPath, string filePath, string fileName, string fileType)
        {
            ProgamPath = progamPath;
            FilePath = filePath;
            FileName = fileName + fileType;
        }
        //--呼叫GMSH.exe
        public void CallProgram()
        {
            //確認資料夾中是否有要開啟的檔案
            for (int i = 1; !File.Exists(FilePath + "\\" + FileName); i++)
            {
                PopupBox.Message($"等待生成檔案:{FilePath }{ FileName}");
                if (i == 3)//跳出警告三次，還是沒有找到要開啟的檔案則跳出迴圈
                {
                    PopupBox.Error($"無法生成{ FileName}");
                    break;
                }
            }
            try
            {
                //--設定開啟檔案位置
                string oldPath = "";
                oldPath = Path.Combine(FilePath, FileName);
                //--設定中文檔名
                byte[] testEncoding = Encoding.Default.GetBytes(oldPath);
                byte[] test = Encoding.Convert(Encoding.GetEncoding("big5"), Encoding.GetEncoding("UTF-8"), testEncoding);
                string newPath = Encoding.Default.GetString(test);
                //--開啟GMESH.exe 
                Process.Start(ProgamPath, newPath);
            }
            catch (Exception ex)
            {
                PopupBox.Error($"載入GMSH.exe失敗。\n{ex.ToString()}");
            }
            return;
        }
        #endregion
    }
}
