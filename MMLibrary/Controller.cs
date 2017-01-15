using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMLibrary
{
    public class Controller : IController
    {
        private bool AddIsPushed;
        private string[] FilePathForController;

        public Controller(Form userView)
        {  }
        public void SearchButton(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void AddButtonPrepare(IViewGUI userView)
        {
            userView.AddButtonPushed += UserView_AddButtonPushed;
        }

        private void UserView_AddButtonPushed(IViewGUI sender) 
        {
            AddIsPushed = true;
            FilePathForController = sender.FilePath; // receive FilePaths from Form1 via public Property file path which is defined in View Interface. 
        }

        public void SaveButton(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void PlayButton(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void SelectRaw(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void PlayNext(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void PlayPrevious(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void CleanSearch(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void ChangesInGrid(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void NewInfoForController(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void RequstNewData(IViewGUI userView)
        {
            throw new NotImplementedException();
        }
    }
}
