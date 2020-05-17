using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Multithreading_06
{
    class Game : ThreadObject
    {
        private List<Ball> myBalls;
        private Panel myPnlGame;

        private Ball mySelectedBall;
        private Point myMarkPos;
        private bool myIsMarked;

        private int myBallCount;
        private int myPoints;
        private int myHitsLeft;

        public List<Ball> Balls => myBalls;

        public Point MarkPos => myMarkPos;
        public bool IsMarked => myIsMarked;

        public Game(Panel pnlGame, int ballCount)
        {
            this.myPnlGame = pnlGame;
            this.myBallCount = ballCount;

            myBalls = new List<Ball>();

            myPoints = 0;
            myHitsLeft = ballCount + 4;

            AddBalls();

            StartThread();
        }

        /// <summary>
        /// Game controls the underlying update frequency of the game-logic
        /// </summary>
        public override void Update()
        {
            while (IsRunning)
            {
                Thread.Sleep((int)((1.0f / 30.0f) * 1000));

                for (int i = myBalls.Count - 1; i >= 0; i--)
                {
                    myBalls[i].Update();

                    //for (int j = 0; j < i; j++)
                    //{
                    //    if (Extensions.BallCollision(myBalls[i], myBalls[j]))
                    //    {

                    //    }
                    //}
                }

                myPnlGame.InvokeIfRequired(() =>
                {
                    myPnlGame.Refresh();
                });
            }
        }

        public void SelectBall(Point mousePosition)
        {
            if (mySelectedBall != null)
            {
                myMarkPos = mousePosition;
                myIsMarked = true;
            }
            else
            {
                myMarkPos = Point.Empty;
                myIsMarked = false;
            }

            for (int i = myBalls.Count - 1; i >= 0; i--)
            {
                myBalls[i].IsSelected = Extensions.WithinBall(myBalls[i], mousePosition);
            }

            for (int i = myBalls.Count - 1; i >= 0; i--)
            {
                if (Extensions.WithinBall(myBalls[i], mousePosition))
                {
                    if (mySelectedBall != myBalls[i])
                    {
                        if (mySelectedBall != null)
                        {
                            mySelectedBall.SetColor();
                        }

                        mySelectedBall = myBalls[i];
                        mySelectedBall.SetColor();
                    }
                }
            }
        }

        private void AddBalls()
        {
            for (int i = 0; i < myBallCount; i++)
            {
                myBalls.Add(new Ball(myPnlGame,
                    new Point(
                        0 + StaticRandom.RandomNumber(0, myPnlGame.Width - 32),
                        0 + StaticRandom.RandomNumber(0, myPnlGame.Height - 32)),
                    new Size(
                        Properties.Resources.billiardBall.Width,
                        Properties.Resources.billiardBall.Height)));
            }
        }
    }
}
