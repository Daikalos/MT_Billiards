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
        private GameStates myGameStates;

        public static MainForm Form;

        public MainForm()
        {
            InitializeComponent();
            EnableDoubleBuffer();

            Form = this;

            myGameStates = new GameStates(myGame);
            myGameStates.SetState(GameState.GameIdle);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            myGame = new Game(PnlGame, myGameStates, 8);

            BtnStart.Enabled = false;
            BtnStop.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            myGame.IsRunning = false;

            BtnStart.Enabled = true;
            BtnStop.Enabled = false;
        }

        public void Restart()
        {
            PnlGame.InvokeIfRequired(() =>
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
            });
        }

        private void PnlGame_Paint(object sender, PaintEventArgs e)
        {
            if (myGame != null)
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

        public void UpdateGameStateText(string gameState)
        {
            lblGameState.InvokeIfRequired(() =>
            {
                lblGameState.Text = gameState;
            });
        }

        public void UpdatePointsLabel(int points)
        {
            LblPoints.InvokeIfRequired(() =>
            {
                LblPoints.Text = points.ToString();
            });
        }

        public void UpdateHitsLeftLabel(int hitsLeft)
        {
            LblHits.InvokeIfRequired(() =>
            {
                LblHits.Text = hitsLeft.ToString();
            });
        }

        private void EnableDoubleBuffer()
        {
            //Enable doublebuffer for game panel to reduce flicker
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                PnlGame, new object[] { true });
        }
    }
}
