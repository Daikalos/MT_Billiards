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
    class Ball
    {
        private Panel myPnlGame;

        private PointF myPosition;
        private PointF myDestination;
        private PointF myDirection;
        private PointF myVelocity;
        private Size mySize;

        private Color myColor;
        private Color myColorNormal;
        private Color myColorSelect;

        private bool myIsSelected;

        private float mySpeed;
        private float myCurrentSpeed;
        private float myDamping;
        private float myInitialDistance;

        public Rectangle DestinationRect => new Rectangle((int)Position.X - (Size.Width / 2), (int)Position.Y - (Size.Height / 2), Size.Width, Size.Height);
        public PointF Position => myPosition;
        public PointF Velocity => myVelocity;
        public Size Size => mySize;
        public Color Color => myColor;

        public bool IsSelected { get => myIsSelected; set => myIsSelected = value; }

        public Ball(Panel pnlGame, PointF position, Size size, float speed)
        {
            this.myPnlGame = pnlGame;
            this.myPosition = position;
            this.mySize = size;
            this.mySpeed = speed;

            myDamping = 0.95f;

            myColor = AssignRandomColor();

            myColorNormal = myColor;
            myColorSelect = Color.White;
        }

        public Task Move()
        {
            return Task.Run(() => 
            {
                float dampSpeed = (1.0f - (myInitialDistance - Extensions.PointLength(myDestination.Subtract(myPosition))) / myInitialDistance);
                myCurrentSpeed = (mySpeed * dampSpeed < myCurrentSpeed) ? mySpeed * dampSpeed : myCurrentSpeed * myDamping;

                myVelocity = myDirection.Normalize().MultiplyValue(myCurrentSpeed);
                myPosition = myPosition.Add(myVelocity);
            });
        }

        public Task WallCollision()
        {
            return Task.Run(() => 
            { 
                if (myPosition.Add(myVelocity).X - (mySize.Width / 2) < 0)
                {
                    myDirection = new PointF(myDirection.X * -1, myDirection.Y);
                }
                if (myPosition.Add(myVelocity).X + (mySize.Width / 2) > myPnlGame.Width)
                {
                    myDirection = new PointF(myDirection.X * -1, myDirection.Y);
                }
                if (myPosition.Add(myVelocity).Y - (mySize.Height / 2) < 0)
                {
                    myDirection = new PointF(myDirection.X, myDirection.Y * -1);
                }
                if (myPosition.Add(myVelocity).Y + (mySize.Height / 2) > myPnlGame.Height)
                {
                    myDirection = new PointF(myDirection.X, myDirection.Y * -1);
                }
            });
        }

        public Task BallCollision(Ball ball)
        {
            return Task.Run(() =>
            {

            });
        }

        public void CueHitBall(PointF destination)
        {
            myDestination = destination;
            myCurrentSpeed = float.MaxValue;

            myDirection = myDestination.Subtract(myPosition);
            myVelocity = myDirection.Normalize();

            myInitialDistance = Extensions.PointLength(myDirection);
        }

        public void SetColor()
        {
            myColor = (myIsSelected) ? myColorSelect : myColorNormal;
        }

        private Color AssignRandomColor()
        {
            int randomNumber = StaticRandom.RandomNumber(0, 5);
            switch (randomNumber)
            {
                case 0:
                    return Color.Red;
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Cyan;
                case 3:
                    return Color.Lime;
                case 4:
                    return Color.Maroon;
                default:
                    return Color.Magenta;
            }
        }
    }
}
