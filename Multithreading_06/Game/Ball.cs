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

        private Point myPosition;
        private Size mySize;
        private Color myColor;
        private Color myColorNormal;
        private Color myColorSelect;

        private bool myIsSelected;

        public Rectangle DrawBox => new Rectangle(Position.X, Position.Y, Size.Width, Size.Height);
        public Point Position => myPosition;
        public Size Size => mySize;
        public Color Color => myColor;

        public bool IsSelected { get => myIsSelected; set => myIsSelected = value; }

        public Ball(Panel pnlGame, Point position, Size size)
        {
            this.myPnlGame = pnlGame;
            this.myPosition = position;
            this.mySize = size;  

            myColor = AssignRandomColor();

            myColorNormal = myColor;
            myColorSelect = Color.White;
        }

        public void Update()
        {

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
