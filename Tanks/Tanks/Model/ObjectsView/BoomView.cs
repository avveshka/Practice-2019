using Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectsView
{
    public class BoomView : Boom
    {
        protected SpriteAnimate sprite;

        public BoomView(float x, float y, int w, int h) : base(x, y, w, h)
        {
            sprite = new SpriteAnimate(0, 224, w, h, 8, 3);
        }

        public void SetSprite(float dx)
        {
            sprite.SetSprite(dx);
        }

        public void Draw(Graphics g)
        {
            sprite.Draw(g, X, Y);
        }

        public bool OnFinish()
        {
            return sprite.OnFinish();
        }
    }
}
