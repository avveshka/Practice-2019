using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SpriteAnimate : Sprite
    {
        protected float speedAnimate;
        protected int countFrame;
        protected float index;

        public SpriteAnimate(int x, int y, int width, int height, float speed, int count) : base(x, y, width, height)
        {
            speedAnimate = speed;
            countFrame = count;
            index = 0;
        }

        public virtual void SetSprite(float dx)
        {
            index += dx * speedAnimate;
            if (index > countFrame)
            {
                index -= countFrame;
            }
        }

        public override void Draw(Graphics g, int x, int y)
        {
            g.DrawImage(Image, new Rectangle(x, y, width, height), new Rectangle(this.x + (int)index * width, this.y, width, height), GraphicsUnit.Pixel);
        }

    }
}