using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UltraID3Lib;

namespace MMLibrary
{
    public class Model : IModel
    {
        private string[][] TableFormModel;
        DataTable MyTable;
        private string searchBox;

        public Model()
        {
            MyTable = new DataTable();
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
            if (TableFormModel != null)
            {
                for (int i = 0; i < TableFormModel.GetLength(0); i++)
                {
                    //for (int j = 0; j < TableFormModel.GetLength(1); j++)
                    //{
                    var result = MyTable.AsEnumerable().Where(myRow => myRow.Field<string>("Title").Contains(searchBox));
                    if (result != null)
                    {
                        MyTable.Rows.Add(result);
                    }
                    //}
                }
            }
        }
        private void Search()
        {
            //sender.SeachBoxText
            //var result = MyTable.AsEnumerable().Where(myRow => myRow.Field<int>("RowNo") == 1);
        }
        //var result = myDataTable.AsEnumerable().Where(myRow => myRow.Field<int>("RowNo") == 1);
        private void UserView_AddButtonPushed(IViewGUI sender)
        {
            TableFormModel = sender.TableContr;

            MyTable.Columns.Add("Title", typeof(string));
            MyTable.Columns.Add("Year", typeof(string));
            MyTable.Columns.Add("Artist", typeof(string));
            MyTable.Columns.Add("Album", typeof(string));
            MyTable.Columns.Add("Genre", typeof(string));
            MyTable.Columns.Add("FilePath", typeof(string));

            for (int i = 0; i < TableFormModel.GetLength(0); i++)
            {
                MyTable.Rows.Add(TableFormModel[i][0], TableFormModel[i][1], TableFormModel[i][2], TableFormModel[i][3], TableFormModel[i][4]);
            }
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
