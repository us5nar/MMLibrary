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
        string[] FilePath { set; get; }
        int FileSize { set; get; }
        string Title { set; get; }
        string Singer { set; get; }
        int Duration { set; get; }

        string SeachBoxText { set; get; }
        string CellValue { set; get; }
        void DataGriIsEmpty();

        event AddButtonEventHandler AddButtonPushed;
    }
}
