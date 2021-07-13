using System;
using System.Collections.Generic;

namespace Mirror
{
    public interface IManager{
        List<Entity> FindEntityClasses();
        List<Player> FindPlayerClasses();
    }
}
