using System;
using System.Windows.Forms;
using System.Drawing;

namespace Multithreading_06
{
    static class Extensions
    {
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static bool WithinBall(Ball ball, Point point)
        {
            Point ballCenter = new Point(
                ball.Position.X + (ball.Size.Width / 2),
                ball.Position.Y + (ball.Size.Height / 2));
            float ballRadians = (ball.Size.Width / 2);

            float tempDistance = (float)Math.Sqrt(
                Math.Pow(ballCenter.X - point.X, 2) +
                Math.Pow(ballCenter.Y - point.Y, 2));

            if (tempDistance <= ballRadians)
            {
                return true;
            }

            return false;
        }

        public static bool BallCollision(Ball firstBall, Ball secondBall)
        {
            Point firstBallCenter = new Point(
                firstBall.Position.X + (firstBall.Size.Width / 2),
                firstBall.Position.Y + (firstBall.Size.Height / 2));
            float firstBallRadians = (firstBall.Size.Width / 2);

            Point secondBallCenter = new Point(
                secondBall.Position.X + (secondBall.Size.Width / 2),
                secondBall.Position.Y + (secondBall.Size.Height / 2));
            float secondBallRadians = (secondBall.Size.Width / 2);

            float tempDistance = (float)Math.Sqrt(
                Math.Pow(firstBallCenter.X - secondBallCenter.X, 2) +
                Math.Pow(firstBallCenter.Y - secondBallCenter.Y, 2));

            if (tempDistance <= firstBallRadians + secondBallRadians)
            {
                return true;
            }

            return false;
        }
    }
}
