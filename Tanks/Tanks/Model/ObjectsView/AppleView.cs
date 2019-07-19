using Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectsView
{
    class AppleView : Apple
    {
        protected Sprite sprite;

        public AppleView(float x, float y, int width, int height) : base(x, y, width, height)
        {
            sprite = new Sprite(0, 128, width, height);
        }

        public void Draw(Graphics g)
        {
            sprite.Draw(g, X, Y);
        }
    }
}
