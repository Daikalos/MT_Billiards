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
        private PointF myVelocity;
        private PointF myMaxVelocity;
        private PointF myDestination;
        private Size mySize;

        private Color myColor;
        private Color myColorNormal;
        private Color myColorSelect;

        private bool myIsSelected;
        private bool myIsCollisionBall;
        private bool myIsCollisionCue;

        private float mySpeed;
        private float myDamping;
        private float myVelocityMin;
        private float myInitialDistance;

        public Rectangle DestinationRect => new Rectangle((int)Position.X - (Size.Width / 2), (int)Position.Y - (Size.Height / 2), Size.Width, Size.Height);
        public PointF Position => myPosition;
        public PointF Velocity { get => myVelocity; set => myVelocity = value; }
        public Size Size => mySize;
        public Color Color => myColor;

        public bool IsSelected { get => myIsSelected; set => myIsSelected = value; }
        public bool IsCollisionBall { get => myIsCollisionBall; set => myIsCollisionBall = value; }
        public bool IsCollisionCue { get => myIsCollisionCue; set => myIsCollisionCue = value; }

        public Ball(Panel pnlGame, PointF position, Size size, float speed)
        {
            this.myPnlGame = pnlGame;
            this.myPosition = position;
            this.mySize = size;
            this.mySpeed = speed;

            myIsSelected = false;
            myIsCollisionBall = false;
            myIsCollisionCue = false;

            myDamping = 0.95f;
            myVelocityMin = 0.1f;

            myColor = AssignRandomColor();

            myColorNormal = myColor;
            myColorSelect = Color.White;
        }

        public async Task Move()
        {
            await Task.Run(() => 
            {
                if (myIsCollisionCue)
                {
                    float dampSpeed = (1.0f - (myInitialDistance - Extensions.Length(myDestination.Subtract(myPosition))) / myInitialDistance);
                    myVelocity = (Extensions.Length(myVelocity) > myVelocityMin) ? myMaxVelocity.MultiplyValue(dampSpeed) : PointF.Empty;
                }
                else
                {
                    myVelocity = (Extensions.Length(myVelocity) > myVelocityMin) ? myVelocity.MultiplyValue(myDamping) : PointF.Empty;
                }

                myPosition = myPosition.Add(myVelocity);
            });
        }

        public void WallCollision()
        {
            if (myPosition.Add(myVelocity).X - (mySize.Width / 2) < 0)
            {
                myPosition.X = (mySize.Width / 2);
                myVelocity = new PointF(myVelocity.X * -1, myVelocity.Y);

                myIsCollisionCue = false;
            }
            if (myPosition.Add(myVelocity).X + (mySize.Width / 2) > myPnlGame.Width)
            {
                myPosition.X = myPnlGame.Width - (mySize.Width / 2);
                myVelocity = new PointF(myVelocity.X * -1, myVelocity.Y);

                myIsCollisionCue = false;
            }
            if (myPosition.Add(myVelocity).Y - (mySize.Height / 2) < 0)
            {
                myPosition.Y = (mySize.Height / 2);
                myVelocity = new PointF(myVelocity.X, myVelocity.Y * -1);

                myIsCollisionCue = false;
            }
            if (myPosition.Add(myVelocity).Y + (mySize.Height / 2) > myPnlGame.Height)
            {
                myPosition.Y = myPnlGame.Height - (mySize.Height / 2);
                myVelocity = new PointF(myVelocity.X, myVelocity.Y * -1);

                myIsCollisionCue = false;
            }
        }

        public void CueHitBall(PointF destination)
        {
            myIsCollisionCue = true;

            PointF direction = destination.Subtract(myPosition).Normalize();

            myDestination = direction.MultiplyValue(Extensions.Length(destination.Subtract(myPosition)).Clamp(0f, myPnlGame.Width / 2)).Add(myPosition);
            myInitialDistance = Extensions.Length(myDestination.Subtract(myPosition));


            myVelocity = direction.MultiplyValue(mySpeed);
            myMaxVelocity = myVelocity;
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
