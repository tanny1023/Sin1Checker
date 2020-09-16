using System;
using System.Data;
using System.Windows.Forms;

namespace Sin1Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainProgram mainProgram = new MainProgram();
            DataTable TableValue = mainProgram.GetTable(DataTableName.LineTable);
            dataGridView1.DataSource = TableValue;
        }
    }
}
