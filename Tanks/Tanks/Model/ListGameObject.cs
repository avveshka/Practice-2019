using Model.ObjectsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ListGameObject
    {
        public KolobokView Player { get; set; }
        public List<TankView> Tanks { get; set; }
        public List<WallView> Walls { get; set; }
        public List<BulletView> Bullets { get; set; }
        public List<AppleView> Apples { get; set; }
        public List<WaterView> Water { get; set; }
        public List<BoomView> Booms { get; set; }
        public int Score { get; set; }
    }
}
