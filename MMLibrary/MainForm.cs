using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using MMLibrary;
using HundredMilesSoftware.UltraID3Lib;
using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form , IViewGUI, IModel
    {
        DataRow newMediaRow;
        private string[][] TableForm; // internal 2D array for storing and analyzing current data for datagrid view
        private int UpdatedGridSize;  // current/new datagrid size

        private string[] FilePathsField; // the array of filepathes received from dialog window, after pressing Add button
        private string[] FileNamesField;  // the array of file names received from dialog window, after pressing Add button

        private string TitleField; // private title for internal use , receives Title from Controller class
        private string YearField; // private year for internal use , receives Year tag from Controller class
        private string ArtistField; // private Artist for internal use , receives Artist tag from Controller class
        private string AlbumField; // private Album for internal use , receives Album tag from Controller class
        private string GenreField; // private Genre for internal use , receives Genre tag from Controller class

        private string SearchBoxField; // search field after deleting one character
        private string SearchBoxFieldBefore; // search field after deleting one character

        public Form1()
        {
            InitializeComponent(); // Initialize the Form Window

            Controller Contr = new Controller(); // Create the new instance of Controller class
            Model ModelInst = new Model(this); // Create the new instance of Model class 
            Contr.AddButtonPrepare(this); // sign up for Add button event from inside the Controller class
            ModelInst.AddButtonSignUp(this); // sign up for Add button event from inside the Model class
            ModelInst.SearchButtonSignUp(this); // sign up for Search button pushed event from inside the Model class
            ModelInst.SaveButtonSignUp(this); // sign up for Save button pushed event from inside the Model class
            ModelInst.OpenFormSignUp(this); // sign up for First time Window opened event from inside the Model class
            ModelInst.SearchBoxTextChangedSignUp(this);// sign up for the any text changes in the Search Box event
            FirstRun = false; // 
        }
        // public property for receiving/sending updated data from Model class and Controller class to the Form1/GUI and back
        public string[][] TableContr // 
        {
            get
            {
                return TableForm; // sending data from internal private 2D array/DataGridView to the Model class
            }
            set
            {
                TableForm = value; // updating internal private 2D array from the Model class or Controller class
            }
        }
        // public property for receiving/sending updated text from the Search box from Model class and back to GUI
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
        // public property for receiving Title from the Controller class, where it is extracted from Tag info
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
        // public property for receiving Artist name from the Controller class
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
        // public property for receiving/sending File paths 
        public string[] FilePaths
        {
            get
            {
                return FilePathsField;
            }
            set { }
        }
        // public property for receiving/sending File names 
        public string[] FileNames
        {
            get
            {
                return FileNamesField;
            }
            set { }
        }
        // public property for receiving/sending Year
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
        // public property for receiving/sending Album value
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
        // public property for receiving/sending Genre value
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
        // public property for receiving/sending Current grid size after any changes to the grid (search, add) values
        public int NewGridSize
        {
            get { return UpdatedGridSize; }
            set { UpdatedGridSize = value; }
        }
        // on the first program run, the data is retreived from XML file (previous run)
        private void Form1_Load(object sender, EventArgs e)
        {
            OnFormOpened(); // triggers event in the Model class, which triggers reading from the previous XML file
            if (UpdatedGridSize > 0) // check if the XML is empty (nothing to update in the grid), then skip updating the DataGridView
            {
                dataSet1.Clear(); // clear all unrelated data from DataSet, which automatically applies to DataGridView via binding
                for (int i = 0; i < TableForm.GetLength(0); i++) // update DataSet/DataGridView with the data from XML
                {
                    // add new Row to the DataTable, which is in DataSet, which is bind to the DataGridView
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow(); 
                    // populate the row in the DataTable from 2D array/ current data from XML , which is read in Model class
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
        // clean resources after closing application 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose(true);
        }
        // sign up for grid changed event which is generated in Model class , when the search is done (some rows will be deleted after search)
        public void GridWasChangedSignUp(IModel sender)
        {
            sender.GridWasChanged += Sender_GridWasChanged;
        }
        // when data is changed during search in Model class, then the new size is send to Form1 class via the public property
        // and new 2D array is allocated according to the new size of the grid
        private void Sender_GridWasChanged(IModel sender)
        {
            UpdatedGridSize = sender.NewGridSize; // change the grid size
            TableForm = new string[UpdatedGridSize][]; // allocate new 2D array. prepare it for receiving new data from Model class
        }
        
        public event AddButtonEventHandler AddButtonPushed; // public event for Add Button pushed
        // 
        private void OnAddButtonPushed() // function which checks if Pushed button event is valid 
        {
            if (AddButtonPushed != null)
            {
                AddButtonPushed(this); // call the method in Model/Controller class which have already signed up for Add Button pushed event
            }
        }
        // method which is called when Add button was pushed
        private void AddButton_Click(object sender, EventArgs e)
        {
            string[] filesName, paths; // temporary arrays for storing file paths and file names from OpenFile Dialog window
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result of opening the dialog window
            {
                filesName = openFileDialog1.SafeFileNames; // save file names from dialog window to the local array
                paths = openFileDialog1.FileNames;// save file paths from dialog window to the local array

                FilePathsField = paths;  // save paths from dialog window to the private field ( array)
                FileNamesField = filesName;  // save file names from dialog window to the private field/ array

                TableForm = new string[paths.Length][]; // initialize local 2D array with the empty data and the size, which corresponds to the number of added files

                OnAddButtonPushed(); // when "Add" was pressed, this triggers event for Controller , and sends reference to "this" Form1 object to the Controller class 
                                     //  where Controller class is able to extract info from Form1 via public properties, such as FilePath 
                                     // foreach (string value in files)
                if (UpdatedGridSize > 0 ) // if the new size is the same as before then skip - nothing to update in the grid
                {
                    dataSet1.Clear(); // clear the DataSet/DataGridView before filling it with new data
                    for (int i = 0; i < TableForm.GetLength(0); i++)
                    {
                        newMediaRow = dataSet1.Tables["MLlist"].NewRow(); // add new row to DataTable in DataSet , which binded to DataGridView
                        // DataTable has 6 columns with Tag data, which correspond to 6 columns in public 2D array, which can be updated from another classes
                        newMediaRow["Title"] = TableForm[i][0]; // transfer Title from "updated" from Model 2D array to the new row in DataTable 
                        newMediaRow["Artist"] = TableForm[i][1];
                        newMediaRow["Album"] = TableForm[i][2];
                        newMediaRow["Genre"] = TableForm[i][3];
                        newMediaRow["Year"] = TableForm[i][4];
                        newMediaRow["FilePath"] = TableForm[i][5];
                        dataSet1.Tables["MLlist"].Rows.Add(newMediaRow); // add "updated" row to the DataTable
                    }
                }
            }
        }
        // declare public events for all buttons and for the search box and for the open form first time event
        public event SearchButtonEventHandler SearchButtonPushed;
        public event ChangedGridSizeHandler GridWasChanged;
        public event SaveButtonHandler SaveButtonPushed;
        public event OpenFormHandler FormOpened;
        public event SearchBoxTextChangedHandler SearchBoxTextChanged;

        private void OnSearchButtonPushed() // private fucntion checks if receiver has signed up for the Search event
        {
            if (SearchButtonPushed != null)
            {
                SearchButtonPushed(this); // if the event has fired , and there was a class which is wating for the event, then call the relevant methods for the event
            }
        }
        // methods is called when the Search button was pushed
        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchBoxField = SearchBox.Text; // save text from the search text field
            OnSearchButtonPushed(); // call the function which fires search button pushed event
            if (UpdatedGridSize > 0) // if the size of the internal 2D array was not changed during search button pushed event , then skip changes to the GUI 
            {
                dataSet1.Clear(); // clear the DataGridView by clearing the DataSet
                for (int i = 0; i < UpdatedGridSize; i++) // Update the DataSet according to the changes in the public 2D array, which were done in Model class
                {                                         // according to the search creteria
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
            else if(UpdatedGridSize == 0 && TableForm == null) // if search gained no results, then clear the data in DataGridView/GUI
            {
                dataSet1.Clear();
            }
        }
        // if double click any row in the GUI the audio will be played back
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string lastCellValue = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString(); // read the path of audio file from the DataGrid
            axWindowsMediaPlayer1.URL = lastCellValue; // send the path to Windows Media Player API
        }
        // function for checking validity of the event when Form/GUI is started for the 1st time
        private void OnFormOpened()
        {
            if (FormOpened != null)
            {
                FormOpened(this); // if receiving class has signed uo for the event then call the relevant methods for the event in Model class
            }
        }
       // the method is called when the Play button is pushed
        private void playButton_Click(object sender, EventArgs e)
        {
            string lastCellValue = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString(); //column 5
            axWindowsMediaPlayer1.URL = lastCellValue; //send the path to windows media player
        }
        // event checker when Save button is pushed
        private void OnSaveButtonPushed()
        {
            if (SaveButtonPushed != null)
            {
                SaveButtonPushed(this); // if Saved button was pushed event is fired
            }
        }
        // When Save button is pushed this method is called 
        private void SaveButton_Click(object sender, EventArgs e)
        {
            OnSaveButtonPushed(); // the event is fired
        }
        // this method is called when user edit any cell in the DataGridView, the result is saved to XML in Model class
        private void DataGridView1_CellValueChanged(object sender, EventArgs e)
        {                 // this method is started at the 1st run, because each cell in the DataGrid is changed,
            if (FirstRun) // when the form is opened 1st time there is no need to make any changes in the DataGrid, because it is done via Add Button event
            {  }
            else if(dataSet1.Tables.Count > 0) // if DataGrid is not empty/clean, then fill the new/edited data, when the changes are made in the DataGrid
            {
                DataRow newMediaRow; // declare the new DataRow varaiabele for updated DataGrid
                for (int i = 0; i < dataSet1.Tables[0].Rows.Count; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].Rows[i];
                    // send the edited row to the Model class, by populating public 2D array
                    TableForm[i][0] = newMediaRow.ItemArray[0].ToString();// send new "Title" value back to the Model class
                    TableForm[i][1] = newMediaRow.ItemArray[1].ToString();// send new "Artist" value back to the Model class
                    TableForm[i][2] = newMediaRow.ItemArray[2].ToString(); // send new "Album" value back to the Model class
                    TableForm[i][3] = newMediaRow.ItemArray[3].ToString(); // send new "Genre" value back to the Model class
                    TableForm[i][4] = newMediaRow.ItemArray[4].ToString(); // send new "Year" value back to the Model class
                    TableForm[i][5] = newMediaRow.ItemArray[5].ToString(); // send new "FilePath" value back to the Model class
                }
                OnSaveButtonPushed(); // call the Save button event, the same is called when Save button is pushed
            }
        }
        // the method is called when Search is performed in Search Text Box
        private void OnSearchBoxTextChanged()
        {
            if (SearchBoxTextChanged != null)
            {
                SearchBoxTextChanged(this);  // event fired when Search box was modified, corresponding method is called in Model class
            }
        }
        // implemented when the text was changed in the Search box
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            SearchBoxField = SearchBox.Text; // save text fro the search text box
            OnSearchBoxTextChanged(); // call the event when 
            // if there is a text to search and the text was reduced in the search box, then clear the current results and update the Grid with the filtered results
            if (SearchBoxField.Length == 0 && SearchBoxFieldBefore.Length > SearchBoxField.Length)
            {
                dataSet1.Clear();  // clear the current data from the DataGrid
                for (int i = 0; i < UpdatedGridSize; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();
                    // populate the rows of the new DataGrid and DataSet, and DataTable with filtered data
                    newMediaRow["Title"] = TableForm[i][0];
                    newMediaRow["Artist"] = TableForm[i][1];
                    newMediaRow["Album"] = TableForm[i][2];
                    newMediaRow["Genre"] = TableForm[i][3];
                    newMediaRow["Year"] = TableForm[i][4];
                    newMediaRow["FilePath"] = TableForm[i][5];
                    dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
            }
            else if (UpdatedGridSize == 0 && TableForm == null) // if the 2D array is empty then the search returned 0 results
            {
                 dataSet1.Clear(); // clear the DataSet and the DataGrid because Search has not found any row
            }
            else if (UpdatedGridSize > 0) // if Search returned at least 1 result then new Grid size in Model class will be more than 0, and it will be send to the Form 1 class  during the Search event
            {
                dataSet1.Clear(); // clear previous results,
                // populate the DataGrid with new results after search
                for (int i = 0; i < UpdatedGridSize; i++)
                {
                    newMediaRow = dataSet1.Tables["MLlist"].NewRow();

                    newMediaRow["Title"] = TableForm[i][0];  // receive new results from the Model class after search
                    newMediaRow["Artist"] = TableForm[i][1];
                    newMediaRow["Album"] = TableForm[i][2];
                    newMediaRow["Genre"] = TableForm[i][3];
                    newMediaRow["Year"] = TableForm[i][4];
                    newMediaRow["FilePath"] = TableForm[i][5];
                    dataSet1.Tables["MLlist"].Rows.Add(newMediaRow);
                }
            }
            SearchBoxFieldBefore = SearchBoxField; // save the current text as "old" from the search 
        }                                          // because we would like to compare it with the next search text
                                                   // in order to understand if we need to search the "previous" XML file, or the current XML file
                                                   // if new text is shorter than the new one , then we have to come back to the search results before (stored in the current XML)

        private void button1_Click(object sender, EventArgs e) // Clean button is pushed
        {
            dataSet1.Clear();  //clear all results in DataGrid
            TableForm = null; // clear the 2D array in order to send 0 results to the Model class
            OnSaveButtonPushed(); // save the 0 results to the XML file
        }
    }
}
