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
        private Ball mySelectedBall;     //Current selected ball for the cue to hit
        private GameStates myGameStates; //Check current state of the game

        private int myFramesPerSeconds;

        private Point myMarkPos;         //Marked position for the direction the ball should go when hit by cue  
        private bool myIsMarked;         //If there player has marked a position

        private float myBallSpeed;       //Speed of the ball when moving
        private int myBallCount;         //Amount of balls to the table
        private int myBallSize;          //Size of the ball

        private int myPoints;            //Total amount of balls who reached goal
        private int myHitsLeft;          //Amount of tries player has before losing

        public List<Ball> Balls => myBalls;

        public Point MarkPos => myMarkPos;
        public bool IsMarked => myIsMarked;

        public Game(Panel pnlGame, GameStates gameStates)
        {
            this.myPnlGame = pnlGame;
            this.myGameStates = gameStates;

            myBalls = new List<Ball>();
            myHoles = new List<Hole>();

            myFramesPerSeconds = (int)((1.0f / 60.0f) * 1000); //60FPS

            myMarkPos = Point.Empty;
            myIsMarked = false;

            myBallSpeed = 20.0f;
            myBallCount = 8;
            myBallSize = 32;

            myPoints = 0;
            myHitsLeft = myBallCount + 4;

            myGameStates.SetState(GameState.GameWaiting);

            AddBalls();
            AddHoles();

            //Set reasonable min/max threads
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
                    //Update status for each ball, of which is selected or not, used for assigning correct color
                    myBalls[i].IsSelected = Extensions.WithinBall(myBalls[i], mousePosition);
                }

                for (int i = myBalls.Count - 1; i >= 0; i--)
                {
                    //If the current selected position hits a ball
                    if (Extensions.WithinBall(myBalls[i], mousePosition))
                    {
                        //If the current selected ball is not equal to this new ball
                        if (mySelectedBall != myBalls[i])
                        {
                            //If there already is a selected ball, reset its color
                            if (mySelectedBall != null)
                            {
                                mySelectedBall.SetColor();
                            }

                            //Assign new selected ball
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
                //Can only mark a position if there is a selected ball
                if (mySelectedBall != null)
                {
                    //If the current marked position is not inside the current selected ball
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
                //If there is a selected ball
                if (mySelectedBall != null)
                {
                    //If the current marked location is not inside the current selected ball
                    if (!Extensions.WithinBall(mySelectedBall, myMarkPos))
                    {
                        //If there is a marked location and the game is currently waiting for user input
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
                            //Get direction between eachother
                            PointF dirFirstToSecond = movingBalls[i].Position.Subtract(myBalls[j].Position).Normalize();
                            PointF dirSecondToFirst = myBalls[j].Position.Subtract(movingBalls[i].Position).Normalize();

                            //Multiply swapped velocity to direction
                            PointF velFirstToSecond = dirFirstToSecond.MultiplyValue(Extensions.Length(myBalls[j].Velocity));
                            PointF velSecondToFirst = dirSecondToFirst.MultiplyValue(Extensions.Length(movingBalls[i].Velocity));

                            //Apply velocity
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
                    //If there is a collision with any hole
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
            if (IsRunning)
            {
                //If there is any ball moving, the game is active, otherwise, waiting
                if (myBalls.Any(b => Extensions.Length(b.Velocity) > float.Epsilon))
                {
                    myGameStates.SetState(GameState.GameActive);
                }
                else
                {
                    myGameStates.SetState(GameState.GameWaiting);
                }

                //Check the end state when there is no ball moving
                if (myGameStates.GameState == GameState.GameWaiting)
                {
                    //If there are no more balls on plane, the player has won
                    if (myBalls.Count == 0)
                    {
                        myGameStates.SetState(GameState.GameWin);
                    }

                    //If the player has no more attempts left and there is still balls, game over
                    if (myBalls.Count > 0 && myHitsLeft <= 0)
                    {
                        myGameStates.SetState(GameState.GameOver);
                    }
                }

                //If the end state has been determined, sleep 5 seconds, then reset game
                if (myGameStates.GameState == GameState.GameOver || myGameStates.GameState == GameState.GameWin)
                {
                    Thread.Sleep(5000);

                    myGameStates.SetState(GameState.GameIdle);
                    IsRunning = false;
                }
            }
        }
        private void ClearGame()
        {
            myMarkPos = Point.Empty;
            myIsMarked = false;

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