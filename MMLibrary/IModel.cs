using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
    public interface IModel
    {
        //void Search(IViewGUI userView);
        bool AudioFileIsValid();
        bool TagIsValid();
        void UpdateXML();
        void ReadFromXML();
        void PlayAudio();
        void SendNewDataToController();
        // Check for duplicates
        void CheckfoDuplicates();
    }
}
