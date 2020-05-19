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

        private PointF myPosition;
        private Size mySize;

        public Rectangle DestinationRect => new Rectangle((int)Position.X - (Size.Width / 2), (int)Position.Y - (Size.Height / 2), Size.Width, Size.Height);
        public PointF Position => myPosition;
        public Size Size => mySize;

        public Hole(Panel pnlGame, PointF position, Size size)
        {
            this.myPnlGame = pnlGame;
            this.myPosition = position;
            this.mySize = size;
        }
    }
}
