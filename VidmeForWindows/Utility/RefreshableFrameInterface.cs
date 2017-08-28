using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidmeForWindows.Utility
{

    
    interface RefreshableFrameInterface
    {

        object getPageParameter();
        Boolean multipleSupported();
        Boolean isRefreshable();
        string getTitleText();
        void loadedAction(Action loaded);
    }
}
