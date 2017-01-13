using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
    interface IController
    {
        //===================================== API for GUI 
        // View (GUI) -> Controller
        void SearchButton(IViewGUI userView);
        void AddButton(IViewGUI userView);
        void SaveButton(IViewGUI userView);
        void PlayButton(IViewGUI userView);
        void SelectRaw(IViewGUI userView);
        void PlayNext(IViewGUI userView);
        void PlayPrevious(IViewGUI userView);
        void CleanSearch(IViewGUI userView); // clean search filter and re-load full XML info
        //   Controller -> View (GUI)
        void ChangesInGrid(IViewGUI userView);

        //===================================== API for Model 
        // Model -> Controller
        void NewInfoForController(IViewGUI userView);
        //  Controller -> Model
        void RequstNewData(IViewGUI userView);

        // Check for duplicates
    }
}
