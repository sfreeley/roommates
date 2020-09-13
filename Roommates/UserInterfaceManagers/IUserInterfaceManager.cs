using System;
using System.Collections.Generic;
using System.Text;

namespace Roommates.UserInterfaceManagers
{
    public interface IUserInterfaceManager
    {
        //this consists of an IUserInterfaceManager type class method Execute() where each class that inherits this IUserInterfaceManager
        //will be able to invoke Execute method that does specific and different actions for tha particular class;
        IUserInterfaceManager Execute();
    }
}
