using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    public class Kolobok : MovableGameObject, IShooter
    {
        public Kolobok(int x, int y, direction direction, int width, int height) : base(x, y, direction, width, height)
        {
        }

        public float Reload { get; set; }
    }
}
