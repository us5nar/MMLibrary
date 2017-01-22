using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
   public delegate void AddButtonEventHandler(IViewGUI sender);
   public delegate void SearchButtonEventHandler(IViewGUI sender);
  // public delegate void ChangedGridSizeHandler(IViewGUI sender);

    public interface IViewGUI
    {
         //string FileName { set; get; }
         int FileSize { set; get; }
         int Duration { set; get; }
         string Title { set; get; }
         string Year { set; get; }
         string Artist { set; get; }
         string Album { set; get; }
        string Genre { set; get; }
        string[] FilePath { set; get; }
        string[] FileNames { set; get; }

        string[][] TableContr { set; get; }
        //string[][] TableBackFromModelToForm1 { set; get; }
        string SeachBoxText { set; get; }

        string CellValue { set; get; }
        void DataGriIsEmpty();
        void GridWasChangedSignUp(IModel sender);
        //LinkedList<string> CurrentRow;
        event AddButtonEventHandler AddButtonPushed;
        event SearchButtonEventHandler SearchButtonPushed;
       // event ChangedGridSizeHandler GridWasChanged;
    }
}
