using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
   public delegate void AddButtonEventHandler(IViewGUI sender);

    public interface IViewGUI
    {
         string FileNames { set; get; }
         int FileSize { set; get; }
         int Duration { set; get; }
         string Title { set; get; }
         string Year { set; get; }
         string Artist { set; get; }
         string Album { set; get; }
        string Genre { set; get; }
        string[] FilePath { set; get; }

        string[][] TableContr { set; get; }
        string SeachBoxText { set; get; }

        string CellValue { set; get; }
        void DataGriIsEmpty();
        //LinkedList<string> CurrentRow;
        event AddButtonEventHandler AddButtonPushed;
    }
}
