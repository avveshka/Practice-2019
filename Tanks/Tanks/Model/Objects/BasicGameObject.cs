using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Objects
{
    public class BasicGameObject
    {
        protected float x;
        protected float y;

        public int X
        {
            get
            {
                return (int)x;
            }

            protected set
            {
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return (int)y;
            }

            protected set
            {
                y = value;
            }
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
    }
}
