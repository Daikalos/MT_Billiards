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
            myHitsLeft = ballCount + 4;

            myGameStates.SetState(GameState.GameWaiting);

            AddBalls();
            AddHoles();

            ThreadPool.SetMinThreads(0, 0);
            ThreadPool.SetMaxThreads(200, 200);

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

                //Only update the balls that are currently moving
                List<Ball> updateBalls = myBalls.FindAll(b => Extensions.Length(b.Velocity) > float.Epsilon);

                Task.WaitAll(updateBalls.Select(b => b.Move()).ToArray());
                updateBalls.ForEach(b => b.WallCollision());

                HoleCollision(updateBalls);
                BallCollision(updateBalls);

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

        public Task SelectBall(Point mousePosition)
        {
            return Task.Run(() =>
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
            });
        }
        public Task MarkDirection(Point mousePosition)
        {
            return Task.Run(() =>
            {
                if (mySelectedBall != null)
                {
                    if (!Extensions.WithinBall(mySelectedBall, mousePosition))
                    {
                        myMarkPos = mousePosition;
                        myIsMarked = true;
                    }
                }
            });
        }
        public Task BilliardCueHit()
        {
            return Task.Run(() =>
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
            });
        }

        private void BallCollision(List<Ball> movingBalls)
        {
            for (int i = movingBalls.Count - 1; i >= 0; i--)
            {
                for (int j = myBalls.Count - 1; j >= 0; j--)
                {
                    if (movingBalls[i] != myBalls[j])
                    {
                        if (Extensions.CheckBallCollision(movingBalls[i], myBalls[j]) && (!movingBalls[i].IsCollisionBall && !myBalls[j].IsCollisionBall))
                        {
                            PointF dirFirstToSecond = movingBalls[i].Position.Subtract(myBalls[j].Position).Normalize();
                            PointF dirSecondToFirst = myBalls[j].Position.Subtract(movingBalls[i].Position).Normalize();

                            PointF velFirstToSecond = dirFirstToSecond.MultiplyValue(Extensions.Length(myBalls[j].Velocity));
                            PointF velSecondToFirst = dirSecondToFirst.MultiplyValue(Extensions.Length(movingBalls[i].Velocity));

                            movingBalls[i].Velocity = velFirstToSecond;
                            myBalls[j].Velocity = velSecondToFirst;

                            movingBalls[i].IsCollisionBall = true;
                            myBalls[j].IsCollisionBall = true;

                            movingBalls[i].IsCollisionCue = false;
                            myBalls[j].IsCollisionCue = false;
                        }
                        if (!Extensions.CheckBallCollision(movingBalls[i], myBalls[j]) && (movingBalls[i].IsCollisionBall && myBalls[j].IsCollisionBall))
                        {
                            movingBalls[i].IsCollisionBall = false;
                            myBalls[j].IsCollisionBall = false;
                        }
                    }
                }
            }
        }
        private void HoleCollision(List<Ball> movingBalls)
        {
            for (int i = movingBalls.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < myHoles.Count; j++)
                {
                    if (Extensions.CheckHoleCollision(movingBalls[i], myHoles[j]))
                    {
                        myPoints++;
                        myBalls.Remove(movingBalls[i]);

                        break;
                    }
                }
            }
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