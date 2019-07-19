using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    public abstract class StaticGameObject : BasicGameObject
    {
        public StaticGameObject(float x, float y, int width, int height)
        {
            this.x = x;
            this.y = y;
            Width = width;
            Height = height;
        }
    }
}
