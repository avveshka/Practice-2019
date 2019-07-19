using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    class Tank : MovableGameObject, IShooter
    {
        public Tank(int x, int y, direction direction, int width, int height) : base(x, y, direction, width, height)
        {
        }

        public float Reload { get; set; }
    }
}
