using Controller;
using Model;
using Model.ObjectsView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace view
{
    public partial class MainForm : Form, IViewController
    {
        GameController gameController;
        ListGameObject objects;

        public bool ActiveTimer { get => timer.Enabled; set => timer.Enabled = value; }

        public int MapWidth => imgMap.Width;
        public int MapHeight => imgMap.Height;

        public MainForm(Size formSize, ListGameObject objects)
        {
            MaximumSize = formSize;
            MinimumSize = formSize;
            BackColor = Color.Black;
            InitializeComponent();
            imgMap.Size = new Size(formSize.Width - 16, formSize.Height - 41);
            this.objects = objects;
        }

        public void SetController(GameController gameController)
        {
            this.gameController = gameController;
        }

        public void Render(bool isGame = true)
        {
            Bitmap bitmap = new Bitmap(imgMap.Width, imgMap.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            objects.Walls.ForEach(wall => wall.Draw(graphics));

            objects.Bullets.ForEach(bullet => bullet.Draw(graphics));

            objects.Tanks.ForEach(tank => tank.Draw(graphics));

            objects.Apples.ForEach(apple => apple.Draw(graphics));

            if (isGame)
            {
                objects.Player.Draw(graphics);

                graphics.DrawImage(Sprite.Image, new Rectangle(5, 8, 32, 32), new Rectangle(0, 128, 32, 32), GraphicsUnit.Pixel);

                graphics.DrawString(objects.Score.ToString(), new Font(FontFamily.GenericSansSerif, 22, FontStyle.Bold),
                                new SolidBrush(Color.White), new PointF(35, 10));
            }
            else
            {
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Black)),
                   new Rectangle(0, 0, MapWidth, MapHeight));

                graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 72, FontStyle.Bold),
                                new SolidBrush(Color.LightGray), new PointF((MapWidth - 600) / 2, (MapHeight - 400) / 2));

                graphics.DrawString($"Score: {objects.Score}", new Font(FontFamily.GenericSansSerif, 32, FontStyle.Bold),
                                new SolidBrush(Color.LightGray), new PointF((MapWidth - 200) / 2, (MapHeight - 160) / 2));

                if (DateTime.Now.Millisecond < 500)
                {
                    graphics.DrawString("Try again", new Font(FontFamily.GenericSansSerif, 28, FontStyle.Bold),
                                    new SolidBrush(Color.LightGray), new PointF((MapWidth-200)/2, (MapHeight) / 2));
                }
            }

            imgMap.Image = bitmap;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            this.gameController.KeyDown(e.KeyCode);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.gameController.Timer();
        }
    }
}
