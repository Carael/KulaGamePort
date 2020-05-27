using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KulaGame.Engine.Utils
{
    public enum GoOnlineStates
    {
        Before,
        Request,
        TimeOut,
        BadPassword,
        LogOn,
        NotFound,
        Registration,
        Register,
        Registered,
        ErrorRegister,
        Exist
    }
}
