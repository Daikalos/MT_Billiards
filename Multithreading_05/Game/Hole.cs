using System.Drawing;
using System.Windows.Forms;

namespace Multithreading_05
{
    internal class Hole
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