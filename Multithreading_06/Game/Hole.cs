using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Multithreading_06
{
    class Hole
    {
        private Panel myPnlGame;

        private Point myPosition;
        private Size mySize;

        public Hole(Panel pnlGame, Point position, Size size)
        {
            this.myPnlGame = pnlGame;
            this.myPosition = position;
            this.mySize = size;
        }
    }
}
