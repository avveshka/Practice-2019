using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Log
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }

        public Log(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }
    }
}
