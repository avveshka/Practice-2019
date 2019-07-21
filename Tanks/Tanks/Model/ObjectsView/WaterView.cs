using Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectsView
{
    public class WaterView : Water
    {
        protected Sprite sprite;

        public WaterView(float x, float y, int w, int h) : base(x, y, w, h)
        {
            sprite = new Sprite(64, 192, w, h);
        }

        public void Draw(Graphics g)
        {
            sprite.Draw(g, X, Y);
        }
    }
}
