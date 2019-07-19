using Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectsView
{
    public class KolobokView : Kolobok
    {
        protected SpriteAnimate sprite;

        public KolobokView(int x, int y, direction direction, int height, int width) : base(x, y, direction, height, width)
        {
            // Тут передаем координаты на спрайте
            sprite = new SpriteAnimate(0, 0, width, height, 16, 3);
        }

        public void SetSprite(float dx)
        {
            sprite.SetSprite(dx);
        }

        public void Draw(Graphics g)
        {
            sprite.SetPosition(0, (int)ObjectDirection * Height);
            sprite.Draw(g, X, Y);
        }
    }
}
