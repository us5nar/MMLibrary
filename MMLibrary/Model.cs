using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
    public class Model : IModel
    {
        public void PlayAudio()
        {
            throw new NotImplementedException();
        }

        public void ReadFromXML()
        {
            throw new NotImplementedException();
        }

        public void SendNewDataToController()
        {
            throw new NotImplementedException();
        }

        public void UpdateXML()
        {
            throw new NotImplementedException();
        }

        bool IModel.AudioFileIsValid()
        {
            throw new NotImplementedException();
        }

        bool AudioFileIsValid()   { return false; }

        bool IModel.TagIsValid()
        {
            throw new NotImplementedException();
        }

        bool TagIsValid() { return false; }
    }
}
