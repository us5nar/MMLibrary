﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HundredMilesSoftware.UltraID3Lib;
using System.Data;

namespace MMLibrary
{
    public class Controller : IController, IModel
    {
        private bool AddIsPushed;
        private string[] FilePathForController;
        //private string[] FileNameForController;

        public Controller(Form userView)
        {  }
        //protected <FullFileInfo>

        //public class FullFileInfo
        //{
        //    public string FilePath { set; get; }
        //    public string FileNames { set; get; }
        //    public int FileSize { set; get; }
        //    public int Duration { set; get; }
        //    public string Title { set; get; }
        //    public string Year { set; get; }
        //    public string Artist { set; get; }
        //    public string Album { set; get; }
        //    public string Genre { set; get; }
        //}
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
            AddIsPushed = true;
            FilePathForController = sender.FilePath; // receive FilePaths from Form1 via public Property file path which is defined in View Interface. 
                                                     //FileNameForController = sender.FileNames; // file name
            DataTable ContrTable = new DataTable();
            ContrTable.Columns.Add("Title", typeof(string));
            ContrTable.Columns.Add("Year", typeof(string));
            ContrTable.Columns.Add("Artist", typeof(string));
            ContrTable.Columns.Add("Album", typeof(string));
            ContrTable.Columns.Add("Genre", typeof(string));
            ContrTable.Columns.Add("FilePath", typeof(string));
            LinkedList<string> CurrentRow = null;
            for (int i = 0; i < FilePathForController.Length; i++)
            {
                UltraID3 myMp3 = new UltraID3();
                try
                {
                    myMp3.Read(FilePathForController[i]);
                    //FullFileInfo InfoObj = new FullFileInfo();
                    sender.Title = string.Format("{0}", myMp3.Title);
                    //CurrentRow.AddLast(sender.Title);
                    sender.Year = string.Format("{0}", myMp3.Year);
                    sender.Artist = string.Format("{0}", myMp3.Artist);
                    sender.Album = string.Format("{0}", myMp3.Album);
                    sender.Genre = string.Format("{0}", myMp3.Genre);
                    // datastructure
                    ContrTable.Rows.Add(sender.Title, sender.Year, sender.Artist, sender.Album, sender.Genre);
                    sender.TableContr[i] = new string []{ sender.Title, sender.Year, sender.Artist, sender.Album, sender.Genre};                }
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
    }
}
