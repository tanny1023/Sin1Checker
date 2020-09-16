/*
Class 功能說明:輸出錯誤說明
*/
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Sin1Checker
{
    public class cPopupBox
    {
        #region Property
        //--顯示的資料
        private StringBuilder Text = new StringBuilder();
        #endregion

        #region Private Method
        //--寫入標頭
        private void WriteHeader(string header)
        {
            MessageBox.Show(header + ":\n" + Text.ToString(), "測試訊息");
        }
        #endregion

        #region Public Method
        //--顯示資料
        public void ShowText()
        {
            Test(Text.ToString());
        }
        //--寫入資料
        public void WriteText(string _string)
        {
            Text.AppendLine(_string);
        }
        //--清除並寫入資料
        public void ClearAndWriteText(string text)
        {
            ClearText();
            WriteText(text);
        }
        //--清除資料
        public void ClearText()
        {
            Text.Clear();
        }
        //--測試訊息
        public void Test(string ex)
        {
            MessageBox.Show(ex, "測試訊息");
        }
        //--錯誤訊息
        public void Error(string ex)
        {
            MessageBox.Show(ex, "錯誤訊息");
        }
        //--提示訊息
        public void Message(string ex)
        {
            MessageBox.Show(ex, "訊息");
        }
        //--輸出LIST
        public void ShowList(List<double> loopPoints, string header)
        {
            foreach (double p in loopPoints)
                WriteText(p.ToString());
            WriteHeader(header);
            return;
        }
        //--輸出LIST
        public void ShowList(List<string> text, string header)
        {
            foreach (string t in text)
                WriteText(t);
            WriteHeader(header);
            return;
        }
        #endregion
    }
}
