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
    public class Controller : IController // Extract tags from the audio files and send the data via 2D array/property to the Model Class
    {
        private string[] FilePathForController;
        private string[] FileNameForController;

        public Controller()
        {  }
        // sign up for Add Button event
        public void AddButtonPrepare(IViewGUI userView)
        {
            userView.AddButtonPushed += UserView_AddButtonPushed;
        }
        // triggered when Add button is pushed in GUI, after this all available tags are extracted from the audio files
        private void UserView_AddButtonPushed(IViewGUI sender)  // sender is the object of Form1 class, through which we can access public properties
        {
            FilePathForController = sender.FilePaths; // receive FilePaths from Form1 via public Property file path which is defined in IView Interface. 
            FileNameForController = sender.FileNames; // receive FileName from Form1 class / GUI for Title, if its abasent
            int k = 0; // iterator thgough filenames for Title
            for (int i = 0; i < FilePathForController.Length; i++)
            {
                UltraID3 myMp3 = new UltraID3(); // initialize Tag class (external reference)
                try
                {
                    myMp3.Read(FilePathForController[i]);
                    // if there is no titles in the Tag then substitute it with file name of the audio file
                    if (myMp3.Title == "")
                    {
                        sender.Title = FileNameForController[k].Substring(0, FileNameForController[k].IndexOf('.')); // extract file name without extension (.mp3)
                        k++;
                    }
                    else
                    {
                        sender.Title = string.Format("{0}", myMp3.Title); // send the extracted tag/Title to the Form1 class
                    }
                    sender.Year = string.Format("{0}", myMp3.Year); // send all tags to the Form1 class via public Properties to the Form1 class
                    sender.Artist = string.Format("{0}", myMp3.Artist);
                    sender.Album = string.Format("{0}", myMp3.Album);
                    sender.Genre = string.Format("{0}", myMp3.Genre);
                    // send all extracted Tags to the Form1 class via 2D array, where it is saved in private fields and where it is send to final DataGrid View 
                    sender.TableContr[i] = new string[] { sender.Title, sender.Artist, sender.Album, sender.Genre, sender.Year, sender.FilePaths[i]};
                }
                catch (HundredMilesSoftware.UltraID3Lib.ID3FileException) // Tags are not valid
                {
                    MessageBox.Show("Reading ID3 Tag from file is wrong");
                }
            }
        }
    }
}
