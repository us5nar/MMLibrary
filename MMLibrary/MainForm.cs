using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using MMLibrary;
using HundredMilesSoftware.UltraID3Lib;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form , IViewGUI
    {
        private const string m_XmlFileName = @"Library.xml";
        DataRow newMediaRow;
        private string[][] TableForm;// = new string[openFileDialog1.FileNames.Length][6];

        private string[] FilePathsField;
        private string FileNamesField;
        private int FileSizeField;
        private int DurationField;
        private string TitleField;
        private string YearField;
        private string ArtistField;
        private string AlbumField;
        private string GenreField;

        private string SearchBoxField;

        public Form1()
        {
            InitializeComponent();
            Controller Contr = new Controller();
            Model ModelInst = new Model();
            Contr.AddButtonPrepare(this);
            ModelInst.AddButtonSignUp(this);
            ModelInst.SearchButtonSignUp(this);
        }

        public string[][] TableContr
        {
            get
            {
                return TableForm;
            }
            set
            {
                TableForm = value;
            }
        }
        public string SeachBoxText
        {
            get
            {
                return SearchBoxField;
            }

            set
            {
                SearchBoxField = value;
            }
        }

        public string CellValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int FileSize
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Title
        {
            get
            {
                return TitleField; //return string.Format("{0}", newMediaRow["Title"]);
            }

            set
            {
                TitleField = value;//newMediaRow["Title"] = value;
            }
        }

        public string Singer
        {
            get
            {
                return ArtistField;//return string.Format("{0}", newMediaRow["Artist"]);
            }

            set
            {
                ArtistField = value;//newMediaRow["Artist"] = value;
            }
        }

        public int Duration
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string[] FilePath
        {
            get
            {
                return FilePathsField;
            }
            set { }
        }

        public string Year
        {
            get
            {
                return YearField;
            }

            set
            {
                YearField = value;
            }
        }

        public string Artist
        {
            get
            {
                return ArtistField;//string.Format("{0}", newMediaRow["Singer"]);
            }

            set
            {
                ArtistField = value;//newMediaRow["Singer"] = string.Format("{0}", value);
            }
        }

        public string Album
        {
            get
            {
                return AlbumField;//string.Format("{0}", newMediaRow["Album"]);
            }

            set
            {
                AlbumField = value;//newMediaRow["Album"] = value;
            }
        }

        public string Genre
        {
            get
            {
                return GenreField;//string.Format("{0}", newMediaRow["Genre"]);
            }

            set
            {
                GenreField = value;//newMediaRow["Genre"] = value;
            }
        }

        string IViewGUI.FileName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
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
           // dataSet1.ReadXml(m_XmlFileName);
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
            SearchBox.AppendText(sent);
            //displayBox.AppendText(sent);
            //displayBox.AppendText(Environment.NewLine);
        }
        public event AddButtonEventHandler AddButtonPushed;

        private void OnAddButtonPushed()
        {
            if (AddButtonPushed != null)
            {
                AddButtonPushed(this);
                //AddIsPushed = true;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            //How to add a DataRow to the DataSet:
            //==========================================================
            //DataRow newMediaRow = dataSet1.Tables["MLlist"].NewRow();
            //newMediaRow["fileName"] = "ALFKI";
            //newMediaRow["Title"] = "Alfreds Futterkiste";
            //dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
            //==========================================================

            int size = -1;
            string[] files, paths;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                files = openFileDialog1.SafeFileNames;
                paths = openFileDialog1.FileNames;
                FilePathsField = paths;

                TableForm = new string[paths.Length][];
                OnAddButtonPushed(); // when "Add" was pressed, this triggers event for Controller , and sends reference to "this" Form1 object to the Controller class 
                                     //  where Controller class is able to extract info from Form1 via public properties, such as FilePath 
                                     // foreach (string value in files)

                for (int i = 0; i < paths.Length; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                    newMediaRow["Title"] = TableForm[i][0];
                    newMediaRow["Year"] = TableForm[i][1];
                    newMediaRow["Singer"] = TableForm[i][2];
                    newMediaRow["Album"] = TableForm[i][3];
                    newMediaRow["Genre"] = TableForm[i][4];
                    newMediaRow["fileName"] = paths[i];
                    dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
                //string file = openFileDialog1.FileName;
              
            }
            //Console.WriteLine(size); // <-- Shows file size in debugging mode.
            //Console.WriteLine(result); // <-- For debugging use.
        }

        /*  private void Form1_AddButtonPushed(IViewGUI sender)
          {
           //   Contr. = true;
          }*/
        /*
        private void UserView_AddButtonPushed(IViewGUI sender)
        {
            MessageBox.Show("The Add button was pushed , event fired in Controller");
        }*/
        public event SearchButtonEventHandler SearchButtonPushed;
        private void OnSearchButtonPushed()
        {
            if (SearchButtonPushed != null)
            {
                SearchButtonPushed(this);
            }
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            //The example how to search
            //for (int i = 0; i < dataGridView1.RowCount; i++)
            //    if (dataGridView1[1, i].FormattedValue.ToString().
            //        Contains(SeachBox.Text.Trim()))
            //    {
            //        dataGridView1.CurrentCell = dataGridView1[0, i];
            //        return;
            //    }
            SearchBoxField = SearchBox.Text;
            OnSearchButtonPushed();
            /*DataRowCollection allRows;
            string selectString = "Title Like '%" + SeachBox.Text.Trim() + "%'";
         
            if (dataGridView1.Rows.Count != 0 && dataGridView1.Rows != null)
                {
                allRows = ((DataTable)dataGridView1.DataSource).Rows;

                DataRow[] searchedRows = ((DataTable)dataGridView1.DataSource).Select(selectString);

                int rowIndex = allRows.IndexOf(searchedRows[0]);

                dataGridView1.CurrentCell = dataGridView1[0, rowIndex];
            }*/
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //CONTINUE HERE!!!
            //      private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
            //{
            //    axWindowsMediaPlayer1.URL = paths[listBox1.SelectedIndex];
            //}
            //string[] fileNameFromDataset = dataGridView1.f
            //string firstCellValue = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            //string secondCellValue = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            //MessageBox.Show(firstCellValue, "Debug info");
            //axWindowsMediaPlayer1.URL = firstCellValue;
        }

        public void DataGriIsEmpty()
        {
            throw new NotImplementedException();
        }


        public void PlayButton(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void SelectRaw(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void PlayNext(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void PlayPrevious(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void CleanSearch(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void ChangesInGrid(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void NewInfoForController(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void RequstNewData(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            string lastCellValue = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString(); //column 5
            //string secondCellValue = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            //MessageBox.Show(firstCellValue, "Debug info");
            axWindowsMediaPlayer1.URL = lastCellValue;
        }
    }
}
