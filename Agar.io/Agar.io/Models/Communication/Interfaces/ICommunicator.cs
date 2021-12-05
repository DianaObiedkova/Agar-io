using Agar.IO.Server.Models.Communication.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.IO.Server.Models.Communication.Interfaces
{
    public interface ICommunicator
    {
        void Send(Message message);
    }
}
