using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Model
{
    public class Sprite
    {
        public static Image Image;
        protected int x;
        protected int y;
        protected int width;
        protected int height;

        public Sprite(int x, int y, int width, int height)
        {
            if (Image == null)
            {
                LoadSprite();
            }
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        private void LoadSprite()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "sprites.png");
            if (File.Exists(path))
            {
                Image = Image.FromFile(path);
            }
            else
            {
                throw new FileNotFoundException("File sprites not found");
            }
        }

        public virtual void Draw(Graphics g, int x, int y)
        {
            g.DrawImage(Image, new Rectangle(x, y, width, height), new Rectangle(this.x, this.y, width, height), GraphicsUnit.Pixel);
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
