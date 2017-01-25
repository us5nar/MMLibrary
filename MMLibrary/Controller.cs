using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HundredMilesSoftware.UltraID3Lib;
using System.Data;

namespace MMLibrary
{
    public class Controller : IController//, IModel
    {
       // private bool AddIsPushed;
        private string[] FilePathForController;
        private string[] FileNameForController;

        public Controller()
        {  }

        public void SearchButton(IViewGUI userView)
        {
            throw new NotImplementedException();
        }

        public void AddButtonPrepare(IViewGUI userView)
        {
            userView.AddButtonPushed += UserView_AddButtonPushed;
        }

        private void UserView_AddButtonPushed(IViewGUI sender)  // sender is the object of Form1 class, through which we can access public properties
        {
            //AddIsPushed = true;
            FilePathForController = sender.FilePaths; // receive FilePaths from Form1 via public Property file path which is defined in View Interface. 
                                                     //FileNameForController = sender.FileNames; // file name+
            FileNameForController = sender.FileNames;
            int k = 0;
            for (int i = 0; i < FilePathForController.Length; i++)
            {
                UltraID3 myMp3 = new UltraID3();
                try
                {
                    myMp3.Read(FilePathForController[i]);
                    
                    if (myMp3.Title == "")
                    {
                        sender.Title = FileNameForController[k].Substring(0, FileNameForController[k].IndexOf('.'));
                        k++;
                    }
                    else
                    {
                        sender.Title = string.Format("{0}", myMp3.Title);
                    }
                    //CurrentRow.AddLast(sender.Title);
                    sender.Year = string.Format("{0}", myMp3.Year);
                    sender.Artist = string.Format("{0}", myMp3.Artist);
                    sender.Album = string.Format("{0}", myMp3.Album);
                    sender.Genre = string.Format("{0}", myMp3.Genre);
                    // datastructure
                    
                    sender.TableContr[i] = new string[] { sender.Title, sender.Artist, sender.Album, sender.Genre, sender.Year, sender.FilePaths[i]};
                }
                catch (HundredMilesSoftware.UltraID3Lib.ID3FileException)
                {
                    MessageBox.Show("Reading ID3 Tag from file is wrong");
                }
            }
        }
        private void ExtractTagInfo()
        {
            int i = 0;
            UltraID3 myMp3 = new UltraID3();
            myMp3.Read(FilePathForController[i]);
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

        public bool AudioFileIsValid()
        {
            throw new NotImplementedException();
        }

        public bool TagIsValid()
        {
            throw new NotImplementedException();
        }

        public void UpdateXML()
        {
            throw new NotImplementedException();
        }

        public void ReadFromXML()
        {
            throw new NotImplementedException();
        }

        public void PlayAudio()
        {
            throw new NotImplementedException();
        }

        public void SendNewDataToController()
        {
            throw new NotImplementedException();
        }

        public void CheckfoDuplicates()
        {
            throw new NotImplementedException();
        }

        public void SearchButtonPrepare(IViewGUI userView)
        {
            throw new NotImplementedException();
        }
    }
}
