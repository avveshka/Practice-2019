using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    public class Bullet : MovableGameObject
    {
        public BasicGameObject Sender;

        private float bulletSpeed;
        public Bullet(float x, float y, direction direction, int width, int height, BasicGameObject sender) : base (x, y, direction, width, height)
        {
            bulletSpeed = Speed * 3;
            Sender = sender;
        }

        override public void Move(float dt)
        {
            float step = bulletSpeed * dt;

            if (ObjectDirection == direction.Up)
            {
                y -= step;
            }
            else if (ObjectDirection == direction.Right)
            {
                x += step;
            }
            else if (ObjectDirection == direction.Down)
            {
                y += step;
            }
            else
            {
                x -= step;
            }
        }
    }
}
