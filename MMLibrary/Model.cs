using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;
using System.Xml.Linq;
using System.Windows.Forms;

namespace MMLibrary
{
    public class Model : IModel
    {
        private string[][] TableFromModel; // 2-D array which transfer "updated/searched/" data to the Form1 class/GUI
        private const string old_XmlFileName = @"Library_Previous.xml"; // XML for storing "old" data  before doing any changes to the grid
        private const string new_XmlFileName = @"Library_Current.xml"; // XML for storing "current" data after search 
        DataTable MyTable;  // current table where Search and Add are done
        DataTable UpdatedTable; // intermediate table where search results are stored  
        private string searchBox; // text in the search box before deleting one character in the search box
        private string SearchBoxAfter; // text in the search box after deleting one character in the search box
        private int ModifiedGridSize; // the new size of the grid and 2D array, which is used for updating grid in GUI (Form1 class), for instance, after search done in Model class 

        public int NewGridSize  //the new size of the 2D array, which used in the events when the grid size is changed and is updated from the Form1 class and send to the Model class
        {                       // the property with the same name exists also in Form1 class, for sending new grid size from Model to Form1 class as well
            get
            {
                return ModifiedGridSize; // internal field
            }

            set
            {
                ModifiedGridSize = value;
            }
        }

        public event ChangedGridSizeHandler GridWasChanged; // event when the grid was changed inside the Model class, during Search

        public Model(IViewGUI FormToModel)
        {
            MyTable = new DataTable() { TableName = "TableAsInGrid" } ; // initialize internal DataTable for storing the data from the GUI/Form1 class
            UpdatedTable = new DataTable() { TableName = "TemporaryTableAfterSearch" }; // initialize temporary DataTable for storing intermidiate results after Search 
            ModifiedGridSize = 0; // set the DataGrid size to 0 at the 1st run
            MyTable.Columns.Add("Title", typeof(string)); // initialize columns for the DataTable
            MyTable.Columns.Add("Artist", typeof(string));
            MyTable.Columns.Add("Album", typeof(string));
            MyTable.Columns.Add("Genre", typeof(string));
            MyTable.Columns.Add("Year", typeof(string));
            MyTable.Columns.Add("FilePath", typeof(string));
            // initialize columns for the temporary DataTable
            UpdatedTable.Columns.Add("Title", typeof(string));
            UpdatedTable.Columns.Add("Artist", typeof(string));
            UpdatedTable.Columns.Add("Album", typeof(string));
            UpdatedTable.Columns.Add("Genre", typeof(string));
            UpdatedTable.Columns.Add("Year", typeof(string));
            UpdatedTable.Columns.Add("FilePath", typeof(string));

            FormToModel.GridWasChangedSignUp(this); // sign up for the event, which occures when the Grid size was changed in Form1 class
            SearchBoxAfter = "";
            searchBox = "";
        }
        // methods for preparing for the button push events in the GUI
        public void AddButtonSignUp(IViewGUI userView)
        {
            userView.AddButtonPushed += UserView_AddButtonPushed;
        }
        public void SearchButtonSignUp(IViewGUI userView)
        {
            userView.SearchButtonPushed += UserView_SearchButtonPushed; ;
        }
        public void SaveButtonSignUp(IViewGUI userView)
        {
            userView.SaveButtonPushed += UserView_SaveButtonPushed;
        }
        public void OpenFormSignUp(IViewGUI userView) // 1st start of the Form
        {
            userView.FormOpened += UserView_FormOpened;
        }
        public void SearchBoxTextChangedSignUp(IViewGUI userView) // any changes in the Search box
        {
            userView.SearchBoxTextChanged += UserView_SearchBoxTextChanged;
        }
        // this method is called when the text in the Search was  changed and corresponding event was fired
        private void UserView_SearchBoxTextChanged(IViewGUI sender)
        {
            SearchBoxAfter = sender.SeachBoxText; // new search box text is saved to local variable
            // if the new text is shorter than the previous text in the Search box, then search in the "old" XML file (before change)
            // the new text also should be not empty
            if (SearchBoxAfter.Length < searchBox.Length && SearchBoxAfter.Length >= 0)
            {
                ReadXMLPreviousResults(); // read the "old" XML
                TableFromModel = null;  // reset the 2D array
                TableFromModel = new string[MyTable.Rows.Count][]; // set new size (rows count) for the new data to be sent to the GUI
                // update the "new" 2D array with the updated data from the internal DataTable with the latest results
                for (int i = 0; i < MyTable.Rows.Count; i++)
                {
                    TableFromModel[i] = new string[6]; // initialize each new row in the 2D array
                    for (int j = 0; j < TableFromModel[i].Length; j++) // fill each row in the 2D array with the "updated" data from the DataTable
                    {
                        TableFromModel[i][j] = MyTable.Rows[i].Field<string>(j); // populate each column in the new row 
                    }
                }
                if (GridWasChanged != null) // check if Form1 class has signed up for the new grid size event
                {
                    GridWasChanged(this); // if the Grid was changed, then trigger the proper method in Form1 class
                    sender.TableContr = TableFromModel; // send the new data after search via the public 2D array
                    if (TableFromModel == null) // if nothing was found , then just set the new DataGrid size to 0, it will show empty DataGrid in GUI
                    {
                        sender.NewGridSize = 0; // send new 0 size if nothing was found
                    }
                    else
                    {
                        sender.NewGridSize = TableFromModel.Length; // send the new DataGrid size if at least 1 row was found
                    }
                }
            }
           
            UserView_SearchButtonPushed(sender); // perform automatic search with the new text from Search Box, it is similar to 
        }                                        // pushing the Search button, which is not visible in our GUI
        // The method is called when the Search button is pushed, but because this button is invisible, the method is called automatically,
        // when the text is changed the Search box
        private void UserView_SearchButtonPushed(IViewGUI sender)
        {
            searchBox = sender.SeachBoxText;// save the search word to local string

            if (TableFromModel != null) // if local 2D array is not empty, the perform Search on the Grid
            {
                foreach (DataRow resultedRow in MyTable.Rows)  // Creat new table from old table based on search filter
                {
                   // compare the text in the search box to each column (Tag) in the DataRow
                    if (resultedRow["Title"].ToString().ToLower().Contains(searchBox.ToLower()) ||
                        resultedRow["Artist"].ToString().ToLower().Contains(searchBox.ToLower()) ||
                        resultedRow["Album"].ToString().ToLower().Contains(searchBox.ToLower()) ||
                        resultedRow["Genre"].ToString().ToLower().Contains(searchBox.ToLower()) ||
                        resultedRow["Year"].ToString().ToLower().Contains(searchBox.ToLower()) ||
                        resultedRow["FilePath"].ToString().ToLower().Contains(searchBox.ToLower()))
                    {
                        DataRow newRow = UpdatedTable.NewRow(); // if found at least in one column , then initialize the new row in the temporary DataTable
                        newRow.ItemArray = resultedRow.ItemArray.Clone() as object[];  // copy one full row from the current DataTable to the temporary DataTable 
                        UpdatedTable.Rows.Add(newRow);  // copy from initial DataTable to the temporary DataTable
                    }
                }
                // if nothing was found than "new" and "old" DataTables should be the same size
                if (UpdatedTable.Rows.Count != MyTable.Rows.Count)
                {
                    ModifiedGridSize = UpdatedTable.Rows.Count; // update the new size of the DataGrid, for sending it to Form1 class
                    // if nothing was found then "new" Updated DataTable has 0 rows,
                    // otherwise copy all data from "new" DataTable to 2D array for sending it to Form1 via event "GridWasChanged"
                    if (UpdatedTable.Rows.Count > 0)
                    {
                        TableFromModel = null; // reset the 2D array before writing new data 
                        TableFromModel = new string[UpdatedTable.Rows.Count][]; // initialize 2D array with the new size
                        for (int i = 0; i < UpdatedTable.Rows.Count; i++) // 
                        {
                            TableFromModel[i] = new string[6]; // initialize new row with 6 columns like in the DataTable
                            for (int j = 0; j < TableFromModel[i].Length; j++)
                            {
                                TableFromModel[i][j] = UpdatedTable.Rows[i].Field<string>(j); // copy all data from the temporary DataTable with search results to the 
                            }                                                                 // 2D array for sending it to the Form1 / GUI
                        }
                        if (GridWasChanged != null)
                        {
                            GridWasChanged(this); // Grid was certainly changed after search
                            if (TableFromModel != null)
                            {
                                sender.TableContr = TableFromModel; // send new data to the GUI
                                if (TableFromModel == null)
                                {
                                    sender.NewGridSize = 0; // if nothing was found send the new 0 size
                                }
                                else
                                {
                                    sender.NewGridSize = TableFromModel.Length; // else send the poper "updated" Grid size
                                }
                            }
                        }
                    }
                    else // if search has found nothing, then send empty table to the GUI and new size of the Grid is 0
                    {
                        TableFromModel = null; // clear DataTable
                        if (GridWasChanged != null)
                        {
                            GridWasChanged(this);
                            sender.TableContr = TableFromModel; // send the empty results to the Grid/GUI
                            if (TableFromModel == null)
                            {
                                sender.NewGridSize = 0;
                            }
                            else
                            {
                                sender.NewGridSize = TableFromModel.Length;
                            }
                        }
                    }
                }
                MyTable.Clear(); // clear the internal results table, before writing the new data after search 
                MyTable = UpdatedTable.Copy(); // copy all results of the search from temporary table to the current DataTable
                UpdatedTable.Clear(); // clan the temporary DataTable
            }
            UpdateXMLNewResults(); // update the current XML file with the search results
        }
        // the methid is started when the Add button was pushed in GUI
        private void UserView_AddButtonPushed(IViewGUI sender)
        {
            // initialize new DataTable for storing temporary results, and comparing them to the current results, in order to avoid adding duplicates to the DataGrid
            DataTable CompareTable = new DataTable(); 
            // initialize standard columns for new temporary DataTable
            CompareTable.Columns.Add("Title", typeof(string));
            CompareTable.Columns.Add("Artist", typeof(string));
            CompareTable.Columns.Add("Album", typeof(string));
            CompareTable.Columns.Add("Genre", typeof(string));
            CompareTable.Columns.Add("Year", typeof(string));
            CompareTable.Columns.Add("FilePath", typeof(string));
            DataRow CurrentRow = CompareTable.NewRow(); // initialize a new row for the temporary DataTable
            ModifiedGridSize = 0; // size of the grid before adding is 0

            TableFromModel = sender.TableContr; // receive Tag data from the Controller class to the internal 2D array (TableFromModel)
            // if the result DataTable, and the Grid are empty , then there will be no duplicates, and there is no need to compare Table results with the new ones
            if (MyTable.Rows.Count == 0)
            {
                for (int i = 0; i < TableFromModel.GetLength(0); i++) // adding new rows to the DataTable according to the files from the OpenDialog in the GUI 
                {
                    // add new row with the tag data to the current DataTable
                    MyTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4], TableFromModel[i][5]);
                    ModifiedGridSize++; // the Grid size is increased
                }
                // the new size event is triggered in the Form1 class, and the new size with the new data is send to GUI
                if (GridWasChanged != null)
                {
                    GridWasChanged(this);
                    sender.TableContr = TableFromModel; // send the new data back to DataGrid
                    if (TableFromModel == null)
                    {
                        sender.NewGridSize = 0; // if there was no new data then send 0 results
                    }
                    else
                    {
                        sender.NewGridSize = TableFromModel.Length; //send the new size of the Grid to the GUI class - Form1
                    }
                }
            }
            else // if there was data in the DataGrid already then compare each row in the Grid against each new row 
            {
                for (int i = 0; i < TableFromModel.GetLength(0); i++)
                {   // Add new row from the new file to the temporary table
                    CompareTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4],TableFromModel[i][5]);
                    // compare this new row against each exisitng rowin the current DataTable by using LINQ intersect command simmilar to the SQL query with intersect
                    var result = CompareTable.AsEnumerable().Intersect(MyTable.AsEnumerable(), DataRowComparer.Default);
                    // if there is no duplicate rows in the exisiting results, then it is allowed to add a new row to the current DataTable
                    if (result.Count<DataRow>() == 0)
                    {
                        ModifiedGridSize++; // increase the Grid size
                        // add the new row to the current DataTable without any duplicates
                        MyTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4], TableFromModel[i][5]);
                    }
                    CompareTable.Clear(); // clean the temporary Datatable for the new compare cycle
                }
                // update the 2D array with the new non-duplicated results for sending it back to the Grid/GUI
                TableFromModel = null; // clean 2D array before populating
                TableFromModel = new string[MyTable.Rows.Count][]; // initialize 2D array with the new empty rows/new size
                for (int i = 0; i < MyTable.Rows.Count; i++)
                {
                    TableFromModel[i] = new string[6]; // initialize new row in the datatable
                    for (int j = 0; j < TableFromModel[i].Length; j++) // go through each column in the row
                    {
                        TableFromModel[i][j] = MyTable.Rows[i].Field<string>(j); // fill the 2D array with the new data 
                    }
                }
                if (GridWasChanged != null)
                {
                    GridWasChanged(this); // fire change Grid event
                    sender.TableContr = TableFromModel; // send the new data to the Grid via 2D array
                    if (TableFromModel == null)
                    {
                        sender.NewGridSize = 0; // send the 0 size if nothing was added
                    }
                    else
                    {
                        sender.NewGridSize = TableFromModel.Length; // send the new Grid size after adding
                    }
                }
            }
            UpdateXMLPreviousResults(); // Save current DataTable to XML after adding a new batch of files, because after adding we have an initial result set for the future searches
                                        // and in case if form is closed then at 1st run this data will be retreived from "previous" XML
        }
        // method called when the Save button was pushedin GUI
        private void UserView_SaveButtonPushed(IViewGUI sender)
        {
            TableFromModel = sender.TableContr; // receive data from Grid to the private double array (TableFromModel)
            MyTable.Clear();
            //UpdateXMLPreviousResults();
            if (TableFromModel != null)
            {
                for (int i = 0; i < TableFromModel.GetLength(0); i++)
                {
                    MyTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4], TableFromModel[i][5]);
                    //ModifiedGridSize++;
                }
            }
            UpdateXMLPreviousResults();
          }

        private void UserView_FormOpened(IViewGUI sender)
        {
            //ReadXMLNewResults();
            ReadXMLPreviousResults();
            TableFromModel = null; // prepare 2D array for sending results from  inner DataTable(with data from XML) to Form's Data Grid
            TableFromModel = new string[MyTable.Rows.Count][]; // initialize the new size of 2D array with the size of updated DataTable
            for (int i = 0; i < MyTable.Rows.Count; i++)
            {
                TableFromModel[i] = new string[6];
                for (int j = 0; j < TableFromModel[i].Length; j++)
                {
                    TableFromModel[i][j] = MyTable.Rows[i].Field<string>(j); // populate 2D array from inner Data Table (Model Class)
                }
            }
            if (GridWasChanged != null) 
            {
                GridWasChanged(this); // event triggered if the new Data Table size (and 2D array size too) are different from the previous size
                sender.TableContr = TableFromModel; // send new data via 2D array to the Form's Data Grid
                if (TableFromModel == null) // if nothing to send
                {
                    sender.NewGridSize = 0; // send the new size as 0 if there are no results in Data Table
                }
                else
                {
                    sender.NewGridSize = TableFromModel.Length;// send the new size as well
                }
            }
        }
        // write data from current DataTable to the local XML file, in order to save previous results before search for example
        private void UpdateXMLPreviousResults()
        {
            MyTable.WriteXml(old_XmlFileName);
        }
        // write data from current DataTable to the local 2nd XML file, in order to save cuurent results
        private void UpdateXMLNewResults()
        {
            MyTable.WriteXml(new_XmlFileName);
        }
        // read data from the "previous" XML file, during search or during 1st run, or when Save button is pushed
        private void ReadXMLPreviousResults()
        {
            if (File.Exists(old_XmlFileName)) // check if XML file already exist, if not then nothing to read
            {
                XElement x = XElement.Load(old_XmlFileName); // load data from XML as XElement

                // extract only usefull data from the XElement array, by filtering it with query
                IEnumerable<XElement> de = from el in x.Descendants()
                                           where !(el.Name == "TableAsInGrid" || el.Name == "TemporaryTableAfterSearch")
                                           select el;
                int i = 0; // iterator through rows from XML in XElement array
                bool visited = false; // if there is at least 1 element was extracted from XML this value will be true
                object[] rowArray = new object[6]; // initialize empty ItemArray/ new row for populating internal DataTable
                DataRow NodeToRow = null;
                MyTable.Clear(); // clear DataTable before populating
                foreach (var d in de) // iterate through the rows in XElement array , if there is no element in XML ,this loop will be skipped and we don;t need to add new row to our DataTable (visited is false)
                {
                    visited = true; // check if there are any rows extracted from the XML
                    if (i >= 6) // in extracted XML data we have all datafor different rows togather, for extracting we go through 6 rows which correspond to 1 row in the DataTable
                    {
                        // when 6 rows from XML are saved to one row in DataTable we need to initialize the new row in our DataTable
                        NodeToRow = MyTable.NewRow(); // initialize one empty row for internal DataTable
                        NodeToRow.ItemArray = rowArray; // initialize empty ItemArray for 1 row
                        MyTable.Rows.Add(NodeToRow); 
                        ModifiedGridSize++; // increase the size of the Grid

                        i = 0; // 
                        rowArray = new object[6]; // initialize new ItemArray for new DataRow
                    }
                    rowArray[i] = d.Value; // save value from XElement array to our DataRow element/column
                    i++;
                }
                if (visited) // this step is needed because foreach loop doesn't include the last iteration through XElement data and we need to do it separatly
                {
                    NodeToRow = MyTable.NewRow();
                    NodeToRow.ItemArray = rowArray;
                    MyTable.Rows.Add(NodeToRow);
                    ModifiedGridSize++;
                }
            }
        }
        // Read data from the current XML during search
        private void ReadXMLNewResults()
        {
            if (File.Exists(new_XmlFileName))
            {
                XElement x = XElement.Load(new_XmlFileName);  // load data from XML as XElement

                // extract only usefull data from the XElement array, by filtering it with query
                IEnumerable<XElement> de = from el in x.Descendants()
                                           where !(el.Name == "TableAsInGrid" || el.Name == "TemporaryTableAfterSearch")
                                           select el;
                int i = 0;
                bool visited = false;
                object[] rowArray = new object[6]; // initialize empty ItemArray/ new row for populating internal DataTable
                DataRow NodeToRow = null;
                MyTable.Clear(); // clear DataTable before populating
                // iterate through the rows in XElement array , if there is no element in XML ,this loop will be skipped and we don;t need to add new row to our DataTable (visited is false)
                foreach (var d in de)
                {
                    // when 6 rows from XML are saved to one row in DataTable we need to initialize the new row in our DataTable
                    if (i >= 6)
                    {
                        NodeToRow = MyTable.NewRow();
                        NodeToRow.ItemArray = rowArray;
                        MyTable.Rows.Add(NodeToRow);
                        ModifiedGridSize++;

                        i = 0;
                        rowArray = new object[6];
                    }
                    rowArray[i] = d.Value;
                    i++;
                }
                if (visited)
                {
                    NodeToRow = MyTable.NewRow();
                    NodeToRow.ItemArray = rowArray;
                    MyTable.Rows.Add(NodeToRow);
                    ModifiedGridSize++;
                }
            }
        }
    }
}
