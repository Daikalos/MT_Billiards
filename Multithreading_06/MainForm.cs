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
                e.Graphics.FillEllipse(new SolidBrush(currentBall.Color), currentBall.DrawBox);
            }

            if (myGame.IsMarked)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Black), new Rectangle(
                    new Point(myGame.MarkPos.X - 8, myGame.MarkPos.Y - 8), 
                    new Size(16, 16)));
            }
        }

        private void EnableDoubleBuffer()
        {
            //Enable doublebuffer for game panel to reduce flicker
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                PnlGame, new object[] { true });
        }

        private void PnlGame_Click(object sender, EventArgs e)
        {
            myGame.SelectBall(PnlGame.PointToClient(Cursor.Position));
        }
    }
}
