using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    public class Wall : StaticGameObject 
    {
        public bool Destroyable;

        public Wall(int x, int y, int width, int height, bool destroyable = false) : base(x, y, width, height)
        {
            Destroyable = destroyable;
        }
    }
}
