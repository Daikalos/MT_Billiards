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

        private float myBallSpeed;

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

            myBallSpeed = 10.0f;

            myPoints = 0;
            myHitsLeft = ballCount - 1;

            AddBalls();

            StartThread();
        }

        /// <summary>
        /// Game controls the underlying update frequency of the game-logic
        /// </summary>
        public override async void Update()
        {
            while (IsRunning)
            {
                Thread.Sleep((int)((1.0f / 30.0f) * 1000));
                
                await Task.WhenAll(myBalls.FindAll(b => Extensions.PointLength(b.Velocity) > float.Epsilon).Select(b => b.Move()));

                await Task.WhenAll(myBalls.FindAll(b => Extensions.PointLength(b.Velocity) > float.Epsilon).Select(b => b.WallCollision()));

                for (int i = myBalls.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (Extensions.BallCollision(myBalls[i], myBalls[j]))
                        {
                            
                        }
                    }
                }

                myPnlGame.InvokeIfRequired(() =>
                {
                    //Refresh panel to show latest update
                    myPnlGame.Refresh();
                });
            }
        }

        public void SelectBall(Point mousePosition)
        {
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

        public void MarkDirection(Point mousePosition)
        {
            if (mySelectedBall != null)
            {
                if (!Extensions.WithinBall(mySelectedBall, mousePosition))
                {
                    myMarkPos = mousePosition;
                    myIsMarked = true;
                }
            }
        }

        public void BilliardCueHit()
        {
            if (mySelectedBall != null)
            {
                mySelectedBall.CueHitBall(myMarkPos);
            }
        }

        private void AddBalls()
        {
            for (int i = 0; i < myBallCount; i++)
            {
                myBalls.Add(new Ball(myPnlGame,
                    new Point(
                        16 + StaticRandom.RandomNumber(0, myPnlGame.Width - 32),
                        16 + StaticRandom.RandomNumber(0, myPnlGame.Height - 32)),
                    new Size(32, 32), myBallSpeed));
            }
        }
    }
}
