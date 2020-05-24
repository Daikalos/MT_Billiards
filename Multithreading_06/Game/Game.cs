using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multithreading_06
{
    internal class Game : ThreadObject
    {
        private readonly List<Ball> myBalls;
        private readonly List<Hole> myHoles;

        private Panel myPnlGame;
        private Ball mySelectedBall;
        private GameStates myGameStates;

        private int myFramesPerSeconds;

        private Point myMarkPos;
        private bool myIsMarked;

        private float myBallSpeed;
        private int myBallSize;

        private int myBallCount;
        private int myPoints;
        private int myHitsLeft;

        public List<Ball> Balls => myBalls;

        public Point MarkPos => myMarkPos;
        public bool IsMarked => myIsMarked;

        public Game(Panel pnlGame, GameStates gameStates, int ballCount)
        {
            this.myPnlGame = pnlGame;
            this.myBallCount = ballCount;
            this.myGameStates = gameStates;

            myBalls = new List<Ball>();
            myHoles = new List<Hole>();

            myFramesPerSeconds = (int)((1.0f / 60.0f) * 1000); //60FPS

            myMarkPos = Point.Empty;
            myIsMarked = false;

            myBallSpeed = 20.0f;
            myBallSize = 32;

            myPoints = 0;
            myHitsLeft = ballCount + 2;

            myGameStates.SetState(GameState.GameWaiting);

            AddBalls();
            AddHoles();

            StartThread();
        }

        /// <summary>
        /// Game controls the underlying update frequency of the game-logic
        /// </summary>
        public override void Update()
        {
            while (IsRunning)
            {
                Thread.Sleep(myFramesPerSeconds);

                Task.WhenAll(myBalls.FindAll(b => Extensions.Length(b.Velocity) > float.Epsilon).Select(b => b.Move()).ToArray());

                for (int i = myBalls.Count - 1; i >= 0; i--)
                {
                    myBalls[i].WallCollision();
                    for (int j = i - 1; j >= 0; j--)
                    {
                        BallCollision(myBalls[i], myBalls[j]);
                    }
                    for (int j = 0; j < myHoles.Count; j++)
                    {
                        if (HoleCollision(myBalls[i], myHoles[j]))
                        {
                            break;
                        }
                    }
                }

                myPnlGame.InvokeIfRequired(() =>
                {
                    //Refresh panel to show latest update
                    myPnlGame.Refresh();
                });

                MainForm.Form.UpdateHitsLeftLabel(myHitsLeft);
                MainForm.Form.UpdatePointsLabel(myPoints);

                StateCheck();
            }

            ClearGame();
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
                if (!Extensions.WithinBall(mySelectedBall, myMarkPos))
                {
                    if (myIsMarked && myGameStates.GameState == GameState.GameWaiting)
                    {
                        myHitsLeft--;
                        mySelectedBall.CueHitBall(myMarkPos);

                        myMarkPos = Point.Empty;
                        myIsMarked = false;
                    }
                }
            }
        }

        private void BallCollision(Ball firstBall, Ball secondBall)
        {
            if (firstBall != secondBall)
            {
                if (Extensions.CheckBallCollision(firstBall, secondBall) && (!firstBall.IsCollisionBall && !secondBall.IsCollisionBall))
                {
                    PointF dirFirstToSecond = firstBall.Position.Subtract(secondBall.Position).Normalize();
                    PointF dirSecondToFirst = secondBall.Position.Subtract(firstBall.Position).Normalize();

                    PointF velFirstToSecond = dirFirstToSecond.MultiplyValue(Extensions.Length(secondBall.Velocity));
                    PointF velSecondToFirst = dirSecondToFirst.MultiplyValue(Extensions.Length(firstBall.Velocity));

                    firstBall.Velocity = velFirstToSecond;
                    secondBall.Velocity = velSecondToFirst;

                    firstBall.IsCollisionBall = true;
                    secondBall.IsCollisionBall = true;

                    firstBall.IsCollisionCue = false;
                    secondBall.IsCollisionCue = false;
                }
                if (!Extensions.CheckBallCollision(firstBall, secondBall) && (firstBall.IsCollisionBall && secondBall.IsCollisionBall))
                {
                    firstBall.IsCollisionBall = false;
                    secondBall.IsCollisionBall = false;
                }
            }
        }
        private bool HoleCollision(Ball ball, Hole hole)
        {
            if (Extensions.CheckHoleCollision(ball, hole))
            {
                myPoints++;
                myBalls.Remove(ball);

                return true;
            }
            return false;
        }

        private void AddBalls()
        {
            for (int i = 0; i < myBallCount;)
            {
                PointF spawnPos = new PointF(
                        (myBallSize / 2) + StaticRandom.RandomNumber(0, myPnlGame.Width - myBallSize),
                        (myBallSize / 2) + StaticRandom.RandomNumber(0, myPnlGame.Height - myBallSize));

                bool canSpawn = true;
                for (int j = 0; j < myBalls.Count; j++)
                {
                    //only spawn a ball at a position that is not colliding with any other ball
                    if (Extensions.Length(spawnPos.Subtract(myBalls[j].Position)) < myBallSize)
                    {
                        canSpawn = false;
                    }
                }

                if (canSpawn)
                {
                    myBalls.Add(new Ball(myPnlGame, spawnPos, new Size(myBallSize, myBallSize), myBallSpeed));
                    i++;
                }
            }
        }
        private void AddHoles()
        {
            for (int i = 0; i < 6; i++)
            {
                PointF position = new PointF(
                    ((i % 3) * (myPnlGame.Width / 2)),
                    ((i / 3) * (myPnlGame.Height)));

                if (i % 3 != 1)
                {
                    myHoles.Add(new Hole(myPnlGame, position,
                        new Size(32, 32)));
                }
                else
                {
                    myHoles.Add(new Hole(myPnlGame, position,
                        new Size(16, 16)));
                }
            }
        }

        private void StateCheck()
        {
            if (myBalls.Any(b => Extensions.Length(b.Velocity) > float.Epsilon))
            {
                myGameStates.SetState(GameState.GameActive);
            }
            else
            {
                myGameStates.SetState(GameState.GameWaiting);
            }

            if (myGameStates.GameState == GameState.GameWaiting)
            {
                if (myBalls.Count == 0)
                {
                    myGameStates.SetState(GameState.GameWin);
                }

                if (myHitsLeft <= 0)
                {
                    myGameStates.SetState(GameState.GameOver);
                }
            }

            if (myGameStates.GameState == GameState.GameOver || myGameStates.GameState == GameState.GameWin)
            {
                Thread.Sleep(5000);

                myGameStates.SetState(GameState.GameIdle);
                IsRunning = false;
            }
        }
        private void ClearGame()
        {
            myBalls.Clear();
            myPnlGame.InvokeIfRequired(() =>
            {
                //Refresh panel to show latest update
                myPnlGame.Refresh();
            });

            MainForm.Form.Restart();
        }
    }
}