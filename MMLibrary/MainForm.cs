using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using MMLibrary;
using HundredMilesSoftware.UltraID3Lib;
using System.Collections.Generic;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form , IViewGUI, IModel
    {
        private const string m_XmlFileName = @"Library.xml";
        DataRow newMediaRow;
        private string[][] TableForm;// = new string[openFileDialog1.FileNames.Length][6];
        //private string[][] TableBackFromModelToFormPrivat;
        private int UpdatedGridSize;

        private string[] FilePathsField;
        private string[] FileNamesField;
        //private int FileSizeField;
        //private int DurationField;
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
            Model ModelInst = new Model(this);
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
                return TitleField; 
            }

            set
            {
                TitleField = value;
            }
        }

        public string Artist
        {
            get
            {
                return ArtistField;
            }

            set
            {
                ArtistField = value;
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
        public string[] FileNames
        {
            get
            {
                return FileNamesField;
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
        public string Album
        {
            get
            {
                return AlbumField;
            }

            set
            {
                AlbumField = value;
            }
        }

        public string Genre
        {
            get
            {
                return GenreField;
            }

            set
            {
                GenreField = value;
            }
        }



        public int NewGridSize
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
            Dispose(true);
        }
        public void GridWasChangedSignUp(IModel sender)
        {
            sender.GridWasChanged += Sender_GridWasChanged;
        }
        
        private void Sender_GridWasChanged(IModel sender)
        {
            UpdatedGridSize = sender.NewGridSize;
            TableForm = new string[UpdatedGridSize][];
        }
        /*
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sent = "Title should be here";//chatBox.Text;
            SearchBox.AppendText(sent);
            //displayBox.AppendText(sent);
            //displayBox.AppendText(Environment.NewLine);
        }*/
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

            //int size = -1;
            string[] filesName, paths;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filesName = openFileDialog1.SafeFileNames;
                paths = openFileDialog1.FileNames;

                FilePathsField = paths;
                FileNamesField = filesName;

                TableForm = new string[paths.Length][];

                OnAddButtonPushed(); // when "Add" was pressed, this triggers event for Controller , and sends reference to "this" Form1 object to the Controller class 
                                     //  where Controller class is able to extract info from Form1 via public properties, such as FilePath 
                                     // foreach (string value in files)
                if (UpdatedGridSize > 0 )
                {
                    dataSet1.Clear();
                    for (int i = 0; i < TableForm.GetLength(0); i++)
                    {
                        newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                        newMediaRow["Title"] = TableForm[i][0];
                        newMediaRow["Year"] = TableForm[i][1];
                        newMediaRow["Artist"] = TableForm[i][2];
                        newMediaRow["Album"] = TableForm[i][3];
                        newMediaRow["Genre"] = TableForm[i][4];
                        newMediaRow["FilePath"] = TableForm[i][5];//FilePathsField[m]; m++;
                        dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                    }
                }
            }
        }

        public event SearchButtonEventHandler SearchButtonPushed;
        public event ChangedGridSizeHandler GridWasChanged;

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
            //int k = 0;
            SearchBoxField = SearchBox.Text;
            OnSearchButtonPushed();
            if (UpdatedGridSize > 0)
            {
                dataSet1.Clear();
                for (int i = 0; i < UpdatedGridSize; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                    newMediaRow["Title"] = TableForm[i][0];
                    newMediaRow["Year"] = TableForm[i][1];
                    newMediaRow["Artist"] = TableForm[i][2];
                    newMediaRow["Album"] = TableForm[i][3];
                    newMediaRow["Genre"] = TableForm[i][4];
                    newMediaRow["FilePath"] = TableForm[i][5];//FilePathsField[k]; k++;
                    dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
            }
            else if(UpdatedGridSize == 0 && TableForm == null)
            {
                dataSet1.Clear();
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string lastCellValue = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
            axWindowsMediaPlayer1.URL = lastCellValue;
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
            axWindowsMediaPlayer1.URL = lastCellValue;
        }
    }
}
