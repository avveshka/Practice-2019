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

        public Bullet(float x, float y, direction direction, int width, int height, BasicGameObject sender) : base (x, y, direction, width, height)
        {
            speed = speed * 3;
            Sender = sender;
        }
    }
}
