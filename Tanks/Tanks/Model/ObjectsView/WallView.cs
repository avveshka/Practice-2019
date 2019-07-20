using Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectsView
{
    public class WallView : Wall
    {
        protected Sprite sprite;

        public WallView(int x, int y, int width, int height, bool destroyable = false) : base(x, y, width, height, destroyable)
        {
            if (destroyable)
            {
                sprite = new Sprite(0, 192, width, height);
            }
            else
            {
                sprite = new Sprite(32, 192, width, height);
            }
        }

        public void Draw(Graphics g)
        {
            sprite.Draw(g, X, Y);
        }
    }
}
