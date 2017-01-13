using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
   // public delegate void NewUserEventHandler(INewUserView sender);

    public interface IViewGUI
    {
        string SeachBoxText { set; get; }
        string CellValue { set; get; }
        void DataGriIsEmpty();

        //event NewUserEventHandler CreateNewUser;
    }
}
