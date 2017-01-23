using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;
//using UltraID3Lib;

namespace MMLibrary
{
    public class Model : IModel
    {
        private string[][] TableFromModel;

        DataTable MyTable;
        DataTable UpdatedTable;
        private string searchBox;
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
            MyTable = new DataTable();
            UpdatedTable = new DataTable();
            //Form1 FormInst = new Form1();
            //GridWasChangedSignUp(this);
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
        }

        public void AddButtonSignUp(IViewGUI userView)
        {
            userView.AddButtonPushed += UserView_AddButtonPushed;
        }
        public void SearchButtonSignUp(IViewGUI userView)
        {
            userView.SearchButtonPushed += UserView_SearchButtonPushed; ;
        }

        private void UserView_SearchButtonPushed(IViewGUI sender)
        {
            searchBox = sender.SeachBoxText;
            
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
                        }
                    }
                }
                MyTable.Clear();
                MyTable = UpdatedTable.Copy();
                UpdatedTable.Clear();
            }
        }

        //var result = myDataTable.AsEnumerable().Where(myRow => myRow.Field<int>("RowNo") == 1);
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
                }
            }
            else
            {
                for (int i = 0; i < TableFromModel.GetLength(0); i++)
                {
                    CompareTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4],TableFromModel[i][5]);
                    var result = CompareTable.AsEnumerable().Intersect(MyTable.AsEnumerable(), DataRowComparer.Default);
                    //var foundRows = MyTable.AsEnumerable().Where(r => r.Field<String>("Title").Equals(TableFromModel[i][0]) && r.Field<String>("Year").Equals(TableFromModel[i][1]) && r.Field<String>("Artist").Equals(TableFromModel[i][2]) && r.Field<String>("Album").Equals(TableFromModel[i][3]) && r.Field<String>("Genre").Equals(TableFromModel[i][4]));
                    if (result.Count<DataRow>() == 0)
                    {
                        ModifiedGridSize++;
                        MyTable.Rows.Add(TableFromModel[i][0], TableFromModel[i][1], TableFromModel[i][2], TableFromModel[i][3], TableFromModel[i][4], TableFromModel[i][5]);
                        //TempTableFromModel[k] =  new string[5];
                        //TempTableFromModel[k] = TableFromModel[i];
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
                }
            }
            //CompareTable.Clear();
        }

        public void CheckfoDuplicates()
        {
            throw new NotImplementedException();
        }

        public void PlayAudio()
        {
            throw new NotImplementedException();
        }

        public void ReadFromXML()
        {
            throw new NotImplementedException();
        }

        public void SendNewDataToController()
        {}

        public void UpdateXML()
        {
            throw new NotImplementedException();
        }

        public bool AudioFileIsValid()
        {
            return false;
        }

        public bool TagIsValid()
        {
            return false;
        }
    }
}
