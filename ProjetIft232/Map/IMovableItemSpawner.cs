using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public interface IMovableItemSpawner
    {
        MovableItem Spawn(Position goal);
    }
}
