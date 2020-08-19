using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Multithreading_05
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
            myGame = new Game(PnlGame, myGameStates);

            BtnStart.Enabled = false;
            BtnStop.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            myGame.IsRunning = false;
            myGameStates.SetState(GameState.GameIdle);

            BtnStart.Enabled = true;
            BtnStop.Enabled = false;
        }

        public void Restart()
        {
            PnlGame.InvokeIfRequired(() =>
            {
                LblPoints.Text = "0";
                LblHits.Text = "0";

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
            });
        }

        private void PnlGame_Paint(object sender, PaintEventArgs e)
        {
            if (myGame != null)
            {
                //Draw each ball on billiard table
                for (int i = myGame.Balls.Count - 1; i >= 0; i--)
                {
                    Ball currentBall = myGame.Balls[i];
                    e.Graphics.FillEllipse(new SolidBrush(currentBall.Color), currentBall.DrawRect);
                }

                //If the player has marked a position,
                if (myGame.IsMarked)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Black), new Rectangle(
                        new Point(myGame.MarkPos.X - 3, myGame.MarkPos.Y - 3),
                        new Size(6, 6)));
                }
            }
        }

        private async void PnlGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                await myGame.SelectBall(PnlGame.PointToClient(Cursor.Position));
            }
            if (e.Button == MouseButtons.Right)
            {
                await myGame.MarkDirection(PnlGame.PointToClient(Cursor.Position));
            }
        }

        private async void PnlGame_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            await myGame.BilliardCueHit();
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