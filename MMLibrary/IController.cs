using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMLibrary
{
    public interface IController
    {
        // View (GUI) -> Controller
        void AddButtonPrepare(IViewGUI userView); // sign up for the Add button event
    }
}
