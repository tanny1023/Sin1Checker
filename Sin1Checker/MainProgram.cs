using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Sin1Checker
{
    class MainProgram
    {
        #region Property
        private string PointSheetName = "";
        private string LineNumberSheetName = "";
        private string LinePropertySheetName = "";

        #endregion

        #region Constructor
        public MainProgram()
        {
            PointSheetName = " C.結構點位";
            LineNumberSheetName = "D.桿件編號";
            LinePropertySheetName = "E.桿件資訊";
        }
        #endregion
        #region Method
        public DataTable ImportExcel()
        {
            string windowFilter = "Excel files|*.xlsx";
            string windowTitle = "匯入Excel資料";

            OpenFileDialog openFileDialogFunction = new OpenFileDialog();
            openFileDialogFunction.Filter = windowFilter; //開窗搜尋副檔名
            openFileDialogFunction.Title = windowTitle; //開窗標題

            DataTable dataTable = new DataTable();

            if (openFileDialogFunction.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialogFunction.FileName;
                //連線字串
                string connectString = $"Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";

                using (OleDbConnection Connect = new OleDbConnection(connectString))
                {
                    Connect.Open();
                    string queryString = "SELECT * FROM [" + PointSheetName + "$]";
                    try
                    {
                        using (OleDbDataAdapter dr = new OleDbDataAdapter(queryString, Connect))
                        {
                            dr.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("異常訊息:" + ex.Message, "異常訊息");
                    }
                }
            }
            return dataTable;
        }
        #endregion
    }
}
