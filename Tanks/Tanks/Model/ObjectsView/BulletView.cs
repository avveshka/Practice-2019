using Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectsView
{
    class BulletView : Bullet
    {
        protected Sprite sprite;

        public BulletView(float x, float y, direction direction, int width, int height, BasicGameObject sender) : base(x, y, direction, width, height, sender)
        {
            sprite = new Sprite(76, 173, width, height);
        }

        public void Draw(Graphics g)
        {
            sprite.Draw(g, X, Y);
        }
    }
}
