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
//using UltraID3Lib;

namespace MMLibrary
{
    public class Model : IModel
    {
        private string[][] TableFromModel;
        private const string old_XmlFileName = @"Library_Previous.xml";
        private const string new_XmlFileName = @"Library_Current.xml";
        DataTable MyTable;
        DataTable UpdatedTable;
        private string searchBox;
        private string SearchBoxAfter;
        private int ModifiedGridSize;

        public int NewGridSize
        {
            get
            {
                return ModifiedGridSize;
            }

            set
            {
                ModifiedGridSize = value;
            }
        }

        public event ChangedGridSizeHandler GridWasChanged;

        public Model(IViewGUI FormToModel)
        {
            MyTable = new DataTable() { TableName = "TableAsInGrid" } ;
            UpdatedTable = new DataTable() { TableName = "TemporaryTableAfterSearch" }; 
            ModifiedGridSize = 0;
            MyTable.Columns.Add("Title", typeof(string));
            MyTable.Columns.Add("Year", typeof(string));
            MyTable.Columns.Add("Artist", typeof(string));
            MyTable.Columns.Add("Album", typeof(string));
            MyTable.Columns.Add("Genre", typeof(string));
            MyTable.Columns.Add("FilePath", typeof(string));

            UpdatedTable.Columns.Add("Title", typeof(string));
            UpdatedTable.Columns.Add("Year", typeof(string));
            UpdatedTable.Columns.Add("Artist", typeof(string));
            UpdatedTable.Columns.Add("Album", typeof(string));
            UpdatedTable.Columns.Add("Genre", typeof(string));
            UpdatedTable.Columns.Add("FilePath", typeof(string));

            FormToModel.GridWasChangedSignUp(this);
            SearchBoxAfter = "";
            searchBox = "";
        }

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
        public void OpenFormSignUp(IViewGUI userView)
        {
            userView.FormOpened += UserView_FormOpened;
        }
        public void SearchBoxTextChangedSignUp(IViewGUI userView)
        {
            userView.SearchBoxTextChanged += UserView_SearchBoxTextChanged;
        }

        private void UserView_SearchBoxTextChanged(IViewGUI sender)
        {
            SearchBoxAfter = sender.SeachBoxText;
            if (SearchBoxAfter.Length < searchBox.Length && SearchBoxAfter.Length >= 0)
            {
                ReadXMLPreviousResults();
                TableFromModel = null;
                TableFromModel = new string[MyTable.Rows.Count][];
                for (int i = 0; i < MyTable.Rows.Count; i++)
                {
                    TableFromModel[i] = new string[6];
                    for (int j = 0; j < TableFromModel[i].Length; j++)
                    {
                        TableFromModel[i][j] = MyTable.Rows[i].Field<string>(j);
                    }
                }
                if (GridWasChanged != null)
                {
                    GridWasChanged(this);
                    sender.TableContr = TableFromModel;
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
           
            UserView_SearchButtonPushed(sender);
        }

        private void UserView_SearchButtonPushed(IViewGUI sender)
        {
            searchBox = sender.SeachBoxText;
            //SearchBoxAfter = searchBox;
           // UpdateXMLPreviousResults();
            if (TableFromModel != null)
            {
                foreach (DataRow resultedRow in MyTable.Rows)  // Creat new table from old table based on search filter
                {
                    // copy from initial DataTable to the Updated one
                    if (resultedRow["Title"].ToString().ToLower().Contains(searchBox.ToLower()))
                    {
                        DataRow newRow = UpdatedTable.NewRow();
                        newRow.ItemArray = resultedRow.ItemArray.Clone() as object[];
                        UpdatedTable.Rows.Add(newRow);
                    }
                }
                // if nothing was found than "new" and "old" DataTables should be the same size
                if (UpdatedTable.Rows.Count != MyTable.Rows.Count)
                {
                    ModifiedGridSize = UpdatedTable.Rows.Count;
                    // if nothing was found then "new" Updated DataTable has 0 rows,
                    // otherwise copy all data from "new" DataTable to 2D array for sending it to Form1 via event "GridWasChanged"
                    if (UpdatedTable.Rows.Count > 0)
                    {
                        TableFromModel = null;
                        TableFromModel = new string[UpdatedTable.Rows.Count][];
                        for (int i = 0; i < UpdatedTable.Rows.Count; i++)
                        {
                            TableFromModel[i] = new string[6]; // 6 columns in the table
                            for (int j = 0; j < TableFromModel[i].Length; j++)
                            {
                                TableFromModel[i][j] = UpdatedTable.Rows[i].Field<string>(j);
                            }
                        }
                        if (GridWasChanged != null)
                        {
                            GridWasChanged(this);
                            if (TableFromModel != null)
                            {
                                sender.TableContr = TableFromModel;
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
                    else
                    {
                        TableFromModel = null;
                        if (GridWasChanged != null)
                        {
                            GridWasChanged(this);
                            sender.TableContr = TableFromModel;
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
                MyTable.Clear();
                MyTable = UpdatedTable.Copy();
                UpdatedTable.Clear();
            }
            UpdateXMLNewResults();
        }
        
        private void UserView_AddButtonPushed(IViewGUI sender)
        {
            DataTable CompareTable = new DataTable();
            CompareTable.Columns.Add("Title", typeof(string));
            CompareTable.Columns.Add("Year", typeof(string));
            CompareTable.Columns.Add("Artist", typeof(string));
            CompareTable.Columns.Add("Album", typeof(string));
            CompareTable.Columns.Add("Genre", typeof(string));
            CompareTable.Columns.Add("FilePath", typeof(string));
            DataRow CurrentRow = CompareTable.NewRow();
            ModifiedGridSize = 0;

            TableFromModel = sender.TableContr; // receive data from Grid to the private double array (TableFromModel)
            //UpdateXMLPreviousResults();
            if (MyTable.Rows.Count == 0)
            {
                for (int i = 0; i < TableFromModel.GetLength(0); i++) 
                {
                    MyTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4], TableFromModel[i][5]);
                    ModifiedGridSize++;
                }

                if (GridWasChanged != null)
                {
                    GridWasChanged(this);
                    sender.TableContr = TableFromModel;
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
            else
            {
                for (int i = 0; i < TableFromModel.GetLength(0); i++)
                {
                    CompareTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4],TableFromModel[i][5]);
                    var result = CompareTable.AsEnumerable().Intersect(MyTable.AsEnumerable(), DataRowComparer.Default);

                    if (result.Count<DataRow>() == 0)
                    {
                        ModifiedGridSize++;
                        MyTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4], TableFromModel[i][5]);
                    }
                    CompareTable.Clear();
                }

                TableFromModel = null;
                TableFromModel = new string[MyTable.Rows.Count][];
                for (int i = 0; i < MyTable.Rows.Count; i++)
                {
                    TableFromModel[i] = new string[6];
                    for (int j = 0; j < TableFromModel[i].Length; j++)
                    {
                        TableFromModel[i][j] = MyTable.Rows[i].Field<string>(j);
                    }
                }
                if (GridWasChanged != null)
                {
                    GridWasChanged(this);
                    sender.TableContr = TableFromModel;
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
            //UpdateXMLNewResults();// Save datatable to XML after adding a new batch of files
            UpdateXMLPreviousResults();
        }

        public void ReadFromXML()
        {
            throw new NotImplementedException();
        }
        private void UserView_SaveButtonPushed(IViewGUI sender)
        {
            TableFromModel = sender.TableContr; // receive data from Grid to the private double array (TableFromModel)
            MyTable.Clear();
            //UpdateXMLPreviousResults();
            for (int i = 0; i < TableFromModel.GetLength(0); i++)
            {
                MyTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4], TableFromModel[i][5]);
                //ModifiedGridSize++;
            }
            UpdateXMLPreviousResults();
          }

        private void UserView_FormOpened(IViewGUI sender)
        {
            //ReadXMLNewResults();
            ReadXMLPreviousResults();
            TableFromModel = null;
            TableFromModel = new string[MyTable.Rows.Count][];
            for (int i = 0; i < MyTable.Rows.Count; i++)
            {
                TableFromModel[i] = new string[6];
                for (int j = 0; j < TableFromModel[i].Length; j++)
                {
                    TableFromModel[i][j] = MyTable.Rows[i].Field<string>(j);
                }
            }
            if (GridWasChanged != null)
            {
                GridWasChanged(this);
                sender.TableContr = TableFromModel;
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

        private void UpdateXMLPreviousResults()
        {
            StringWriter writerOld = new StringWriter();
            //MyTable.WriteXml(old_XmlFileName, XmlWriteMode.WriteSchema, true);
            MyTable.WriteXml(old_XmlFileName/*, XmlWriteMode.IgnoreSchema, true*/);
        }
        private void UpdateXMLNewResults()
        {
            StringWriter writerNew = new StringWriter();
            //MyTable.WriteXml(new_XmlFileName, XmlWriteMode.WriteSchema, true);
            MyTable.WriteXml(new_XmlFileName/*, XmlWriteMode.IgnoreSchema, true*/);
        }

        private void ReadXMLPreviousResults()
        {
            //MyTable.Clear();
            //StringReader ReadOld = new StringReader();
            //MyTable.ReadXml(old_XmlFileName);
            MyTable.Clear();

            XElement x = XElement.Load(old_XmlFileName);

            IEnumerable<XElement> de = from el in x.Descendants()
                                       where !(el.Name == "TableAsInGrid" || el.Name == "TemporaryTableAfterSearch")
                                       select el;
            int i = 0;
            object[] rowArray = new object[6];
            DataRow NodeToRow = null;

            foreach (var d in de)
            {
                //    Console.WriteLine(d.Value);
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
            NodeToRow = MyTable.NewRow();
            NodeToRow.ItemArray = rowArray;
            MyTable.Rows.Add(NodeToRow);
            ModifiedGridSize++;
        }
    
        private void ReadXMLNewResults()
        {
            MyTable.Clear();

            XElement x = XElement.Load(new_XmlFileName);

            IEnumerable<XElement> de = from el in x.Descendants()
                                       where !(el.Name == "TableAsInGrid" || el.Name == "TemporaryTableAfterSearch")
                                       select el;
            int i = 0;
            object[] rowArray = new object[6];
            DataRow NodeToRow = null;

            foreach (var d in de)
            {
                //    Console.WriteLine(d.Value);
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
            NodeToRow = MyTable.NewRow();
            NodeToRow.ItemArray = rowArray;
            MyTable.Rows.Add(NodeToRow);
            ModifiedGridSize++;
        }
    }
}
