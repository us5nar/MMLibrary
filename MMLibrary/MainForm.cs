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
        //private const string m_XmlFileName = @"Library.xml";
        DataRow newMediaRow;
        private string[][] TableForm;
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
        private string SearchBoxFieldBefore;
        public Form1()
        {
            InitializeComponent();
            Controller Contr = new Controller();
            Model ModelInst = new Model(this);
            Contr.AddButtonPrepare(this);
            ModelInst.AddButtonSignUp(this);
            ModelInst.SearchButtonSignUp(this);
            ModelInst.SaveButtonSignUp(this);
            ModelInst.OpenFormSignUp(this);
            ModelInst.SearchBoxTextChangedSignUp(this);
            FirstRun = false;
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

        public string[] FilePaths
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
            get { return UpdatedGridSize; }
            set { UpdatedGridSize = value; }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
           //MessageBox.Show("bindingSource1_CurrentChanged!!!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // dataSet1.ReadXml(m_XmlFileName);
            OnFormOpened();
            if (UpdatedGridSize > 0)
            {
                dataSet1.Clear();
                for (int i = 0; i < TableForm.GetLength(0); i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                    newMediaRow["Title"] = TableForm[i][0];
                    newMediaRow["Artist"] = TableForm[i][1];
                    newMediaRow["Album"] = TableForm[i][2];
                    newMediaRow["Genre"] = TableForm[i][3];
                    newMediaRow["Year"] = TableForm[i][4];
                    newMediaRow["FilePath"] = TableForm[i][5];
                    dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DataTable table = dataSet1.Tables["MLlist"].GetChanges();
            //if (table.Rows.Count > 0)
            //{
            //    MessageBox.Show("Content was modified!!!");
            //}
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
                        newMediaRow["Artist"] = TableForm[i][1];
                        newMediaRow["Album"] = TableForm[i][2];
                        newMediaRow["Genre"] = TableForm[i][3];
                        newMediaRow["Year"] = TableForm[i][4];
                        newMediaRow["FilePath"] = TableForm[i][5];
                        dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                    }
                }
            }
        }

        public event SearchButtonEventHandler SearchButtonPushed;
        public event ChangedGridSizeHandler GridWasChanged;
        public event SaveButtonHandler SaveButtonPushed;
        public event OpenFormHandler FormOpened;
        public event SearchBoxTextChangedHandler SearchBoxTextChanged;

        private void OnSearchButtonPushed()
        {
            if (SearchButtonPushed != null)
            {
                SearchButtonPushed(this);
            }
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchBoxField = SearchBox.Text;
            OnSearchButtonPushed();
            if (UpdatedGridSize > 0)
            {
                dataSet1.Clear();
                for (int i = 0; i < UpdatedGridSize; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                    newMediaRow["Title"] = TableForm[i][0];
                    newMediaRow["Artist"] = TableForm[i][1];
                    newMediaRow["Album"] = TableForm[i][2];
                    newMediaRow["Genre"] = TableForm[i][3];
                    newMediaRow["Year"] = TableForm[i][4];
                    newMediaRow["FilePath"] = TableForm[i][5];
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

        private void OnFormOpened()
        {
            if (FormOpened != null)
            {
                FormOpened(this);
            }
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
        private void OnSaveButtonPushed()
        {
            if (SaveButtonPushed != null)
            {
                SaveButtonPushed(this);
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            //dataSet1.WriteXml(m_XmlFileName);
            OnSaveButtonPushed();
        }
        private void DataGridView1_CellValueChanged(object sender, EventArgs e)
        {
            // DataTable TempTable = ((DataTable)this.dataGridView1.DataSource).Copy();
            // dataSet1.Clear();
            // dataSet1.Tables["MLlist"].Copy(TempTable);//.Add(TempTable);
            //dataSet1.Tables.Add(TempTable.Copy());
            if (FirstRun)
            {  }
            else if(dataSet1.Tables.Count > 0)
            {
                DataRow newMediaRow;
                for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].Rows[i];

                    TableForm[i][0] = newMediaRow.ItemArray[0].ToString();//["Title"].;
                    TableForm[i][1] = newMediaRow.ItemArray[1].ToString();// newMediaRow["Year"] ;
                    TableForm[i][2] = newMediaRow.ItemArray[2].ToString(); //TableForm[i][2];
                    TableForm[i][3] = newMediaRow.ItemArray[3].ToString(); //TableForm[i][3];
                    TableForm[i][4] = newMediaRow.ItemArray[4].ToString(); //TableForm[i][4];
                    TableForm[i][5] = newMediaRow.ItemArray[5].ToString(); //TableForm[i][5];
                                                                           //dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
                OnSaveButtonPushed();
            }
        }

        public void UpdateXML()
        {
            throw new NotImplementedException();
        }

        public void ReadFromXML()
        {
            throw new NotImplementedException();
        }
        private void OnSearchBoxTextChanged()
        {
            if (SearchBoxTextChanged != null)
            {
                SearchBoxTextChanged(this);
            }
        }
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            SearchBoxField = SearchBox.Text;
            OnSearchBoxTextChanged();

            if (SearchBoxField.Length == 0 && SearchBoxFieldBefore.Length > SearchBoxField.Length)
            {
                dataSet1.Clear();
                for (int i = 0; i < UpdatedGridSize; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                    newMediaRow["Title"] = TableForm[i][0];
                    newMediaRow["Artist"] = TableForm[i][1];
                    newMediaRow["Album"] = TableForm[i][2];
                    newMediaRow["Genre"] = TableForm[i][3];
                    newMediaRow["Year"] = TableForm[i][4];
                    newMediaRow["FilePath"] = TableForm[i][5];
                    dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
            }
            else if (UpdatedGridSize == 0 && TableForm == null)
            {
                 dataSet1.Clear();
            }
            else if (UpdatedGridSize > 0)
            {
                dataSet1.Clear();
                for (int i = 0; i < UpdatedGridSize; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                    newMediaRow["Title"] = TableForm[i][0];
                    newMediaRow["Artist"] = TableForm[i][1];
                    newMediaRow["Album"] = TableForm[i][2];
                    newMediaRow["Genre"] = TableForm[i][3];
                    newMediaRow["Year"] = TableForm[i][4];
                    newMediaRow["FilePath"] = TableForm[i][5];
                    dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
            }
            SearchBoxFieldBefore = SearchBoxField;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataSet1.Clear();
            TableForm = null;
            OnSaveButtonPushed();
        }
    }
}
