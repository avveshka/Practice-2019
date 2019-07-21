using Controller;
using Model;
using Model.Objects;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm form;

            ListGameObject gameObjects = new ListGameObject();

            int width;
            int height;

            if (args.Length > 0)
            {
                width = int.Parse(args[0]);
                height = int.Parse(args[1]);
            }
            else
            {
                width = 800;
                height = 600;
            }
            if (args.Length > 2)
            {
                MovableGameObject.Speed = int.Parse(args[2]);
            }

            form = new MainForm(new Size(width, height), gameObjects);

            GameController gameController = new GameController(form, gameObjects);
            if (args.Length > 4)
            {
                gameController.SetCountObject(int.Parse(args[3]), int.Parse(args[4]));
            }

            form.SetController(gameController);

            if (args.Length > 5)
            {
                MovableGameObject.Speed = int.Parse(args[5]);
            }

            gameController.NewGame();

            Application.Run(form);
        }
    }
}
