using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Multithreading_06
{
    public partial class MainForm : Form
    {
        private Game myGame;

        public MainForm()
        {
            InitializeComponent();
            EnableDoubleBuffer();

            myGame = new Game(PnlGame, 8);
        }

        private void PnlGame_Paint(object sender, PaintEventArgs e)
        {
            for (int i = myGame.Balls.Count - 1; i >= 0; i--)
            {
                Ball currentBall = myGame.Balls[i];
                e.Graphics.FillEllipse(new SolidBrush(currentBall.Color), currentBall.DestinationRect);
            }

            if (myGame.IsMarked)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Black), new Rectangle(
                    new Point(myGame.MarkPos.X - 3, myGame.MarkPos.Y - 3), 
                    new Size(6, 6)));
            }
        }

        private void EnableDoubleBuffer()
        {
            //Enable doublebuffer for game panel to reduce flicker
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                PnlGame, new object[] { true });
        }

        private void PnlGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                myGame.SelectBall(PnlGame.PointToClient(Cursor.Position));
            }
            if (e.Button == MouseButtons.Right)
            {
                myGame.MarkDirection(PnlGame.PointToClient(Cursor.Position));
            }
        }

        private void PnlGame_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            myGame.BilliardCueHit();
        }
    }
}
