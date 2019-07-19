using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    public class MovableGameObject : BasicGameObject
    {
        public direction ObjectDirection;

        public float speed = 100;

        public MovableGameObject(float x, float y, direction direction, int width, int height)
        {
            ObjectDirection = direction;
            this.x = x;
            this.y = y;
            Width = width;
            Height = height;
        }

        public void ChangeDirection(direction dir)
        {
            ObjectDirection = dir;
        }

        public void ChangeDirection()
        {
            if(ObjectDirection == direction.Up)
            {
                ObjectDirection = direction.Down;
            }
            else if(ObjectDirection == direction.Right)
            {
                ObjectDirection = direction.Left;
            }
            else if(ObjectDirection == direction.Down)
            {
                ObjectDirection = direction.Up;
            }
            else
            {
                ObjectDirection = direction.Right;
            }
        }

        public void Move(float dt)
        {
            float step = speed * dt;

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
