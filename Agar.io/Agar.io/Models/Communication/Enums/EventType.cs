using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.IO.Server.Models.Communication.Enums
{
    public enum EventType
    {
        Spawn,
        CoordsChange,
        Die,
        SizeChange,
        SpawnMyself
    }
}
