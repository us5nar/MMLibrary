using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private const string m_XmlFileName = @"Library.xml";
        //private openFileDialog openFileDialog1();
        public Form1()
        {
            InitializeComponent();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataSet1.WriteXml(m_XmlFileName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataSet1.ReadXml(m_XmlFileName);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataTable table = dataSet1.Tables["MLlist"].GetChanges();
            if (table.Rows.Count > 0)
                {
                MessageBox.Show("Content was modified!!!");
                }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sent = "Title should be here";//chatBox.Text;
            SeachBox.AppendText(sent);
            //displayBox.AppendText(sent);
            //displayBox.AppendText(Environment.NewLine);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string NewFile = File.ReadAllText(file);
                    size = NewFile.Length;
                }
                catch (IOException)
                {
                }
            }
            string Str1 = String.Format("Size = {0}; Result = {1}", size, result);
            MessageBox.Show(Str1, "Debug info");
            //Console.WriteLine(size); // <-- Shows file size in debugging mode.
            //Console.WriteLine(result); // <-- For debugging use.

        }
       
        private void SearchButton_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < dataGridView1.RowCount; i++)
            //    if (dataGridView1[1, i].FormattedValue.ToString().
            //        Contains(SeachBox.Text.Trim()))
            //    {
            //        dataGridView1.CurrentCell = dataGridView1[0, i];
            //        return;
            //    }

            string selectString = "Title Like '%" + SeachBox.Text.Trim() + "%'";

            DataRowCollection allRows =((DataTable)dataGridView1.DataSource).Rows;

            DataRow[] searchedRows = ((DataTable)dataGridView1.DataSource).Select(selectString);

            int rowIndex = allRows.IndexOf(searchedRows[0]);

            dataGridView1.CurrentCell = dataGridView1[0, rowIndex];
        }

    }
}
