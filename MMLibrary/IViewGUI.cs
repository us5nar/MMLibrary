using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
    public delegate void AddButtonEventHandler(IViewGUI sender);
    public delegate void SearchButtonEventHandler(IViewGUI sender);
    public delegate void SaveButtonHandler(IViewGUI sender);
    public delegate void OpenFormHandler(IViewGUI sender);
    public delegate void SearchBoxTextChangedHandler(IViewGUI sender);

    public interface IViewGUI
    {
        string Title { set; get; } // public property for transfering Title tag between classes
        string Year { set; get; } // public property for transfering Year tag between classes
        string Artist { set; get; } // public property for transfering Artist tag between classes
        string Album { set; get; } // public property for transfering Album tag between classes
        string Genre { set; get; } // public property for transfering Genre tag between classes
        string[] FilePaths { set; get; } // public property for transfering File Paths between classes
        string[] FileNames { set; get; } // public property for transfering File Names between classes
        int NewGridSize { set; get; }

        string[][] TableContr { set; get; } // public 2D array for transfering data rows between classes

        string SeachBoxText { set; get; } // public property for text to search for transfering between classes
        
        void GridWasChangedSignUp(IModel sender);
        
        event AddButtonEventHandler AddButtonPushed;
        event SearchButtonEventHandler SearchButtonPushed;
        event SaveButtonHandler SaveButtonPushed;
        event OpenFormHandler FormOpened;
        event SearchBoxTextChangedHandler SearchBoxTextChanged;
    }
}
