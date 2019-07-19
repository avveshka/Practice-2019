using Controller;
using Model.ObjectsView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using view;

namespace Tanks
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //IGameObjects objects = new ListGameObjects();
            //MainForm form = new MainForm(objects);
            //IGameController gameController = new GameController(form, objects);
            //form.SetController(gameController);
            //gameController.NewGame();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form;
            if (args.Length > 0)
            {
                int width = int.Parse(args[0]);
                int height = int.Parse(args[1]);
                form = new MainForm(new Size(width, height));
                Application.Run(form);
            }
        }
    }
}
