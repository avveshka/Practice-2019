using Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectsView
{
    class TankView : Tank
    {
        protected Sprite sprite;

        public TankView(int x, int y, direction direction, int height, int width) : base(x, y, direction, height, width)
        {
            sprite = new Sprite(32, 128, width, height);
        }

        public void Draw(Graphics g)
        {
            if (ObjectDirection == direction.Right)
            {
                sprite.SetPosition(64, 128);
            }
            else if (ObjectDirection == direction.Down)
            {
                sprite.SetPosition(0, 160);
            }
            else
            {
                sprite.SetPosition(32, 160);
            }
            sprite.Draw(g, X, Y);
        }
    }
}
