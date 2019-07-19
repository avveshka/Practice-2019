using Controller;
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
        public MainForm(Size formSize)
        {
            MaximumSize = formSize;
            MinimumSize = formSize;
            BackColor = Color.Black;
            InitializeComponent();
        }

        public void SetController(GameController gameController)
        {
            this.gameController = gameController;
        }

        public void Render()
        {
            Bitmap bitmap = new Bitmap(imgMap.Width, imgMap.Height);
            Graphics graphics = Graphics.FromImage(bitmap);


            
            imgMap.Image = bitmap;
        }

    }
}
