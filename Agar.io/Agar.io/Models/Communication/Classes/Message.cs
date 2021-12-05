using Agar.io.Models;
using Agar.IO.Server.Models.Communication.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.IO.Server.Models.Communication.Classes
{
    public class Message
    {
        private EventType EvType { get; set; }
        private Player Player { get; set; }

        public Message(EventType evType, Player player)
        {
            EvType = evType;
            Player = player;
        }

        public override string ToString()
        {
            return "Message: { eventType:" + EvType + ", player =" + Player.Name + "}";
        }
    }
}
