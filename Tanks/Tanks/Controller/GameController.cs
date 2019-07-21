using Model;
using Model.ObjectsView;
using Model.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Controller
{
    public class GameController
    {
        public int BlockSize;
        private IViewController view;
        private ListGameObject objects;
        private BindingList<Log> logs;
        private LogForm form;
        private KolobokView reservePlayer;

        public bool IsGame = false;
        private Stopwatch timer;
        private Random rnd;

        private int appleCount = 5;
        private int tankCount = 5;

        delegate void func(float dt);
        public void MainLoop()
        {
            float dt = timer.ElapsedMilliseconds / 1000f;
            timer.Restart();

            ResetReload(objects.Player, dt);
            objects.Tanks.ForEach(tank => ResetReload(tank, dt));

            CreateApple(2);
            CreateTank(1);

            RotateTank(0.5f);

            Collision(dt);

            ShootTanks();

            if (!LogForm.IsClosed)
            {
                RefreshLog();
                form.RefreshDgv(logs);
            }

            func f;
            if (IsGame)
            {
                f = objects.Player.SetSprite;
                objects.Booms.ForEach(i => f += i.SetSprite);
                f(dt);
            }
            

            objects.Booms.Where(boom => boom.OnFinish()).ToList()
                 .ForEach(bang => objects.Booms.Remove(bang));

            view.Render(IsGame);
        }

        public void SetCountObject(int tanks, int apples)
        {
            tankCount = tanks;
            appleCount = apples;
        }

        public GameController(IViewController view, ListGameObject objects)
        {
            this.view = view;
            this.objects = objects;
            BlockSize = view.FormWidth / 25;
            rnd = new Random();
        }

        public void NewGame()
        {
            StartGame();
        }

        public void StartGame()
        {
            LoadLevel(1);
            objects.Tanks = new List<TankView>();
            objects.Bullets = new List<BulletView>();
            objects.Apples = new List<AppleView>();
            objects.Booms = new List<BoomView>();
            objects.Score = 0;
            while (objects.Tanks.Count < tankCount)
            {
                CreateTank(100);
            }
            while (objects.Apples.Count < appleCount)
            {
                CreateApple(100);
            }
            timer = new Stopwatch();
            timer.Start();
            view.ActiveTimer = true;
            MainLoop();
        }

        private void LoadLevel(int lvl)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "levels", lvl.ToString() + ".lvl");

            if (File.Exists(path))
            {
                objects.Walls = new List<WallView>();
                objects.Water = new List<WaterView>();


                using (StreamReader sr = File.OpenText(path))
                {
                    int y = 0;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        for (int x = 0; x < line.Length; x++)
                        {
                            switch (line[x])
                            {
                                case 'p':
                                    reservePlayer = new KolobokView(BlockSize * x, BlockSize * y, direction.Up, 25, 25);
                                    break;
                                case '*':
                                    objects.Walls.Add(new WallView(BlockSize * x, BlockSize * y, BlockSize, BlockSize, true));
                                    break;
                                case '=':
                                    objects.Walls.Add(new WallView(BlockSize * x, BlockSize * y, BlockSize, BlockSize, false));
                                    break;
                                case '0':
                                    objects.Water.Add(new WaterView(BlockSize * x, BlockSize * y, BlockSize, BlockSize));
                                    break;
                            }
                        }

                        y++;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException($"Level {lvl} not found");
            }
        }

        public void KeyDown(Keys key)
        {
            if (!IsGame)
            {
                StartGame();
                return;
            }
            switch (key)
            {
                case Keys.A:
                case Keys.Left:
                    objects.Player.ChangeDirection(direction.Left);
                    break;
                case Keys.W:
                case Keys.Up:
                    objects.Player.ChangeDirection(direction.Up);
                    break;
                case Keys.D:
                case Keys.Right:
                    objects.Player.ChangeDirection(direction.Right);
                    break;
                case Keys.S:
                case Keys.Down:
                    objects.Player.ChangeDirection(direction.Down);
                    break;
                case Keys.Space:
                    CreateBullet(objects.Player);
                    break;
                case Keys.R:
                    StartGame();
                    break;
                case Keys.P:
                    InitializationLog();
                    break;
            }
        }

        public void Timer()
        {
            MainLoop();
        }

        private void ResetReload(IShooter shooter, float dt)
        {
            if (shooter != null)
            {
                if (shooter.Reload > 0)
                {
                    shooter.Reload -= dt;
                }
            }
        }

        private void Collision(float dt)
        {
            var delWalls = new List<WallView>();
            var delTanks = new List<TankView>();
            var delApples = new List<AppleView>();
            var delBullets = new List<BulletView>();

            if (IsGame)
            {
                MovableGameObject player = objects.Player.Clone() as MovableGameObject;
                player.Move(dt);

                if (ObjectInScreen(player) &&
                    objects.Water.Find(water => ObjectCollision(water, player)) == null &&
                    objects.Walls.Find(wall => ObjectCollision(wall, player)) == null)
                {
                    bool isFree = true;

                    foreach (var tank in objects.Tanks)
                    {
                        if (ObjectCollision(tank, player))
                        {
                            isFree = false;
                        }
                    }

                    if (isFree)
                    {
                        objects.Player.Move(dt);
                    }
                }

                objects.Apples.ForEach(apple =>
                {
                    if (ObjectCollision(player, apple))
                    {
                        delApples.Add(apple);
                        objects.Score++;
                    }
                });
            }

            objects.Tanks.ForEach(t =>
            {
                t.Move(dt);
                if (!ObjectInScreen(t) ||
                    objects.Walls.Find(wall => ObjectCollision(wall, t)) != null ||
                    objects.Water.Find(water => ObjectCollision(water, t)) != null ||
                    objects.Tanks.Find(tnk => tnk != t ? ObjectCollision(tnk, t) : false) != null)
                {
                    t.ChangeDirection();
                    t.Move(dt);
                }
            });

            objects.Bullets.ForEach(bullet =>
            {
                bool delBullet = false;

                if (!ObjectInScreen(bullet))
                {
                    delBullet = true;
                }

                objects.Walls.ForEach(wall =>
                {
                    if (ObjectCollision(bullet, wall))
                    {
                        if (wall.Destroyable)
                        {
                            delWalls.Add(wall);
                        }
                        delBullet = true;
                        CreateBang(bullet);
                    }
                });

                objects.Tanks.ForEach(tank =>
                {
                    if (ObjectCollision(tank, bullet) && bullet.Sender != tank)
                    {
                        delBullet = true;
                        delTanks.Add(tank);
                        CreateBang(tank);
                    }
                });

                if (IsGame)
                {
                    if (ObjectCollision(bullet, objects.Player) && bullet.Sender != objects.Player)
                    {
                        delBullet = true;
                        CreateBang(objects.Player);
                        GameOver(); // Конец игры
                    }
                }

                if (delBullet)
                {
                    delBullets.Add(bullet);
                }
                else
                {
                    bullet.Move(dt);
                }

            });

            delWalls.ForEach(wall => objects.Walls.Remove(wall));
            delTanks.ForEach(tank => objects.Tanks.Remove(tank));
            delApples.ForEach(apple => objects.Apples.Remove(apple));
            delBullets.ForEach(bullet => objects.Bullets.Remove(bullet));
        }

        private bool ObjectCollision(BasicGameObject obj1, BasicGameObject obj2)
        {
            return (obj1.X + obj1.Width > obj2.X) &&
                (obj1.X <= obj2.X + obj2.Width) &&
                (obj1.Y + obj1.Height > obj2.Y) &&
                (obj1.Y <= obj2.Y + obj2.Height);
        }

        private bool ObjectInScreen(BasicGameObject obj)
        {
            return (obj.X >= 0) && (obj.X + obj.Width <= view.MapWidth) &&
                (obj.Y >= 0) && (obj.Y + obj.Height <= view.MapHeight);
        }

        private void CreateTank(int percent)
        {
            if (objects.Tanks.Count < tankCount && rnd.Next(100) <= percent)
            {
                int x = rnd.Next(view.MapWidth - BlockSize);
                int y = rnd.Next(view.MapHeight - BlockSize);
                var tank = new TankView(x, y, (direction)rnd.Next(4), 30, 30);

                if (objects.Player != null)
                {
                    if (objects.Walls.Find(wall => ObjectCollision(wall, tank)) == null &&
                    ObjectCollision(objects.Player, tank) == false &&
                    objects.Tanks.Find(tnk => tnk != tank ? ObjectCollision(tnk, tank) : false) == null)
                    {
                        objects.Tanks.Add(tank);
                    }
                }
                else
                {
                    if (objects.Walls.Find(wall => ObjectCollision(wall, tank)) == null &&
                    ObjectCollision(reservePlayer, tank) == false &&
                    objects.Tanks.Find(tnk => tnk != tank ? ObjectCollision(tnk, tank) : false) == null)
                    {
                        objects.Tanks.Add(tank);
                    }
                }

            }
        }

        private void CreateApple(int percent)
        {
            if (objects.Apples.Count < appleCount && rnd.Next(100) <= percent)
            {
                int x = rnd.Next(view.MapWidth - BlockSize);
                int y = rnd.Next(view.MapHeight - BlockSize);
                var apple = new AppleView(x, y, BlockSize, BlockSize);

                if (objects.Player != null)
                {
                    if (objects.Walls.Find(wall => ObjectCollision(wall, apple)) == null &&
                        ObjectCollision(objects.Player, apple) == false)
                    {
                        objects.Apples.Add(apple);
                    }
                }
                else
                {
                    if (objects.Walls.Find(wall => ObjectCollision(wall, apple)) == null &&
                       ObjectCollision(reservePlayer, apple) == false)
                    {
                        objects.Apples.Add(apple);
                    }
                }

            }
        }

        private void GameOver()
        {
            IsGame = false;
            objects.Player.OutScreen();
        }

        private void RotateTank(float percent)
        {
            objects.Tanks.ForEach(tank =>
            {
                if (rnd.Next(100) < percent)
                {
                    tank.ChangeDirection((direction)rnd.Next(4));
                }
            });
        }

        private void CreateBullet(MovableGameObject obj)
        {
            if ((obj as IShooter).Reload > 0)
            {
                return;
            }

            float x = obj.X + obj.Width / 2 - 2;
            float y = obj.Y + obj.Height / 2 - 2;

            objects.Bullets.Add(new BulletView(x, y, obj.ObjectDirection, 10, 10, obj));
            (obj as IShooter).Reload = 0.3f;
        }

        private void ShootTanks()
        {
            if (IsGame)
            {
                Rectangle p = new Rectangle(objects.Player.X, objects.Player.Y, objects.Player.Width, objects.Player.Height);

                foreach (var tank in objects.Tanks)
                {
                    bool onTheWay = true;
                    direction lastDir = tank.ObjectDirection;

                    if (p.X + p.Width < tank.X && p.Y + p.Height > tank.Y + tank.Height / 2 && p.Y < tank.Y + tank.Height / 2)
                    {
                        tank.ObjectDirection = direction.Left;
                    }
                    else if (p.X > tank.X + tank.Width && p.Y + p.Height > tank.Y + tank.Height / 2 && p.Y < tank.Y + tank.Height / 2)
                    {
                        tank.ObjectDirection = direction.Right;
                    }
                    else if (p.Y + p.Width < tank.Y && p.X + p.Width > tank.X + tank.Width / 2 && p.X < tank.X + tank.Width / 2)
                    {
                        tank.ObjectDirection = direction.Up;
                    }
                    else if (p.Y > tank.Y + tank.Width && p.X + p.Width > tank.X + tank.Width / 2 && p.X < tank.X + tank.Width / 2)
                    {
                        tank.ObjectDirection = direction.Down;
                    }
                    else
                    {
                        onTheWay = false;
                    }

                    if (tank.ObjectDirection != lastDir && tank.Reload < 0.1f)
                    {
                        tank.Reload = 0.5f;
                    }

                    if (onTheWay)
                    {
                        CreateBullet(tank);
                    }

                }
            }
        }


        private void InitializationLog()
        {
            RefreshLog();
            form = new LogForm(logs);
            form.Show();
        }

        private void RefreshLog()
        {
            logs = null;

            logs = new BindingList<Log>();

            logs.Add(new Log(objects.Player.X, objects.Player.X, "Kolobok"));

            foreach (TankView tank in objects.Tanks)
            {
                logs.Add(new Log(tank.X, tank.X, "Tank"));
            }

            foreach (AppleView apple in objects.Apples)
            {
                logs.Add(new Log(apple.X, apple.X, "Apple"));
            }

            foreach (WallView wall in objects.Walls)
            {
                logs.Add(new Log(wall.X, wall.X, "Wall"));
            }

            foreach (BulletView bullet in objects.Bullets)
            {
                logs.Add(new Log(bullet.X, bullet.X, "Bullet"));
            }
        }

        private void CreateBang(BasicGameObject obj)
        {
            objects.Booms.Add(new BoomView(obj.X + (obj.Width - 30) / 2, obj.Y + (obj.Height - 30) / 2, BlockSize, BlockSize));
        }

        public void PlayerInitialization()
        {
            objects.Player = reservePlayer;
        }
    }
}
