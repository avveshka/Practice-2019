using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public interface IViewController
    {
        void Render(bool isGame = true);

        bool ActiveTimer { get; set; }

        int MapWidth { get; }
        int MapHeight { get; }
    }
}
